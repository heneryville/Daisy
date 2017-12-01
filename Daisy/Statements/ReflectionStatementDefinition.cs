using System.Diagnostics;

namespace Ancestry.Daisy.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Linq;
    using Ancestry.Daisy.Utils;
    using Fasterflect;

    public class ReflectionStatementDefinition : IStatementDefinition
    {
        internal class ReflectionLinkedStatement : ILinkedStatement
        {
            private FastFlect.ObjectActivator activator;
            private MemberSetter delegateForAttachmentsSetter;
            private MemberSetter delegateForContextSetter;
            private MemberSetter delegateForScopeSetter;
            private MemberSetter delegateForTracerSetter;
            private MethodInvoker delegateForInvokation;
            private StaticAnalysis.TransformPredicate scopeConverter;
            public string Statement { get; private set; }

            private readonly ReflectionStatementDefinition definition;
            private readonly Match match;
            private bool transformsToValueType;

            private object[] mappedParameters;

            private object proceedHolder = new object(); //This object is placed into the parameters array as a
              // place holder for the proceed function. It should be swapped out when the proceed function is
              // provided in execute

            internal ReflectionLinkedStatement(ReflectionStatementDefinition definition, string statement, Match match)
            {
                Statement = statement;
                this.definition = definition;
                this.match = match;
                Definition = definition;
                PrecacheReflections(definition.ControllerType);
                PrecacheParameterBindings();
            }

            private void PrecacheParameterBindings()
            {
                var objs = new List<object>();
                var ptrGroups = 1;
                foreach (var param in definition.Parameters)
                {
                    if (param is ProceedParameter)
                    {
                        objs.Add(proceedHolder);
                    }
                    else
                    {
                        if(ptrGroups>=match.Groups.Count)
                        {
                            throw new CannotLinkStatementException(
                                definition.MethodInfo,
                                string.Format("Statement {1} has more parameters than captures available to it." 
                                + "{0} are available.",
                                match.Groups.Count -1, definition.Name));
                        }
                        // check for next valid group to ignore non-capturing groups
                        var nextGroup = match.Groups[ptrGroups++];
                        while (!nextGroup.Success && ptrGroups < match.Groups.Count)
                        {
                            nextGroup = match.Groups[ptrGroups++];
                        }
                        var obj = nextGroup
                            .Captures
                            .If(x => x.Count > 0)
                            .With(x =>
                            {
                                var array = new List<string>();
                                for (var i = 0; i < x.Count; i++)
                                    array.Add(x[i].Value);
                                return array;
                            });
                        if (obj != null)
                            objs.Add(Cast(obj, param));
                    }
                }
                if(objs.Count != definition.Parameters.Count)
                {
                    throw new CannotLinkStatementException(
                        definition.MethodInfo,
                        string.Format("Statement {2} wants {0} parameters from statement," 
                        + " but only {1} could be matched.",
                        definition.Parameters.Count, objs.Count, definition.Name));
                }
                var nonCapturingGroups = match.Groups.Cast<Group>().Sum(castedGroup => castedGroup.Success ? 0 : 1);
                if (ptrGroups < match.Groups.Count - nonCapturingGroups)
                {
                    throw new CannotLinkStatementException(
                        definition.MethodInfo,
                        string.Format("Statement {2} did not use all captures available to it." 
                        + "It used {0} but {1} are available.",
                        ptrGroups-1, match.Groups.Count -1, definition.Name));
                }
                mappedParameters = objs.ToArray();
            }

            protected object Cast(List<string> objList, StatementParameter param)
            {
                try 
                {
                    if (objList.Count == 1)
                    {
                        if (param.Type == typeof(List<string>))
                        {
                            return objList;
                        } else
                            // Convert a simple string
                            return Convert.ChangeType(objList[0], param.Type);
                    }
                    else
                        // Or convert a more complex array
                        return Convert.ChangeType(objList, param.Type);
                }
                catch
                {
                    throw new CannotLinkStatementException(definition.MethodInfo,
                        string.Format("Cannot convert '{0}' into {1}, to match parameter '{2}'",
                        objList, param.Type, param.Name))
                        {
                            Statement = Statement
                        };
                }
            }

            private void PrecacheReflections(Type controllerType)
            {
                var defaultConstructor = controllerType.GetConstructors().FirstOrDefault(x => x.GetParameters().Length == 0);
                if (defaultConstructor == null) throw new ArgumentException("StatementHandler " + controllerType.Name + " does not have a default constructor");
                activator = FastFlect.GetActivator(defaultConstructor);
                delegateForScopeSetter = controllerType.DelegateForSetPropertyValue("Scope");
                delegateForContextSetter = controllerType.DelegateForSetPropertyValue("Context");
                delegateForAttachmentsSetter = controllerType.DelegateForSetPropertyValue("Attachments");
                delegateForTracerSetter = controllerType.DelegateForSetPropertyValue("Tracer");
                delegateForInvokation = definition.MethodInfo.DelegateForCallMethod();
                if (definition.TransformsScopeTo != null)
                {
                    scopeConverter = StaticAnalysis.CreateConverter(definition.TransformsScopeTo);
                    transformsToValueType = definition.TransformsScopeTo.IsValueType;
                }
            }

            public IStatementDefinition Definition { get; private set; }

            [DebuggerStepThrough]
            public bool Execute(InvokationContext context)
            {
                var controllerInstance = CreateController();
                InitializeController(controllerInstance,context);
                var methodParams = Definition.TransformsScopeTo == null ?
                    mappedParameters :
                    mappedParameters.Select(x => x == proceedHolder ?  transformsToValueType ? scopeConverter(context.Proceed) : context.Proceed : x)
                    .ToArray();
                context.PerformanceCounter.Count(context,definition);
                return Execute(controllerInstance, methodParams);
            }

            [DebuggerStepThrough]
            internal bool Execute(object inst, object[] methodParams)
            {
                return (bool)delegateForInvokation(inst, methodParams);
            }

            internal protected object CreateController()
            {
                object controller;
                try
                {
                    controller = activator();
                }
                catch
                {
                    throw new CannotLinkStatementException(definition.MethodInfo,
                        string.Format("Cannot build statement {0} because it's controller could not be constructed.",
                            definition.MethodInfo.Name));
                }
                return controller;
            }

            internal protected void InitializeController(object controller ,InvokationContext invokationContext)
            {
                delegateForScopeSetter(controller, invokationContext.Scope);
                delegateForContextSetter(controller, invokationContext.Context);
                delegateForAttachmentsSetter(controller, invokationContext.Attachments);
                delegateForTracerSetter(controller, invokationContext.Tracer);
            }
        }

        public MethodInfo MethodInfo { get; private set; }

        public Type ControllerType { get; private set; }

        public Type ScopeType { get; private set; }

        public Type TransformsScopeTo { get; private set; }

        public IList<StatementParameter> Parameters { get; private set; }

        public Regex MatchingCriteria { get; private set; }

        public string Name { get { return MethodInfo.Name; } }

        public ReflectionStatementDefinition(MethodInfo methodInfo, Type controllerType)
        {
            MethodInfo = methodInfo;
            ControllerType = controllerType;
            MatchingCriteria = GetMatchingCriteria();
            ScopeType = controllerType.GetProperty("Scope").PropertyType;
            IsolateParameters(methodInfo);
        }

        public ILinkedStatement Link(string statement)
        {
            var match = MatchingCriteria.Match(statement);
            if (!match.Success) return null;
            return new ReflectionLinkedStatement(this, statement, match);
        }

        private void IsolateParameters(MethodInfo methodInfo)
        {
            Parameters = methodInfo.GetParameters()
                .Select(p => {
                    var proceedFunctionType = StaticAnalysis.ExtractProceedFunctionType(p.ParameterType);
                    if (proceedFunctionType != null)
                    {
                        TransformsScopeTo = proceedFunctionType;
                        return new ProceedParameter() {
                            Name = p.Name,
                            Type = p.ParameterType,
                            TransformsTo = proceedFunctionType
                        };
                    }
                    else
                    {
                        return new StatementParameter() { Name = p.Name, Type = p.ParameterType };
                    }
                })
                .ToList();
        }

        public Match Matches(MatchingContext matchingContext)
        {
            return MatchingCriteria.Match(matchingContext.Statement);
        }

        public Regex GetMatchingCriteria()
        {
            var attr = MethodInfo
                .GetCustomAttributes<MatchesAttribute>()
                .FirstOrDefault();
            return attr.With(x => x.RegularExpression).Recover(() => MethodNameToRegex(MethodInfo.Name));
        }

        internal static Regex MethodNameToRegex(string name)
        {
            return new Regex("^"+NormalizeMethodName(name)+"$",RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        }

        internal static string NormalizeMethodName(string name)
        {
            var parts = StringExtentions.SplitNameParts(name);
            var spacings = string.Join("\\s+", parts);
            return spacings;
        }
    }
}