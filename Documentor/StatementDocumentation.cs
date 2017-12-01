namespace Ancestry.Daisy.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Documentation.Utils;
    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Utils;

    public class StatementDocumentation
    {
        public string Title { get; set; }
        public IList<ParameterDocumentation> Parameters { get; set; }
        public string Description { get; set; }
        public Type ScopeType { get; set; }

        internal static string ParseTitle(string title, IList<StatementParameter> parameters)
        {
            //Condition this to look a lot prettier
            title = title.StartsWith("^") ? title.Substring(1) : title;
            title = title.EndsWith("$") ? title.Substring(0,title.Length-1) : title;
            title = title.Replace(@"\s+", " ").Replace(@"\s*", " ").ConsolidateWhitespace();
            foreach (var param in parameters)
            {
                var span = RegexParser.FirstGroup(title);
                if (span == null) break;
                title = RegexParser.Replace(title, span.Value, "[" + param.Name + "]");
            }
            return title;
        }

        public static StatementDocumentation FromReflection(ReflectionStatementDefinition reflectionStatementDefinition, ICommentDocumentation commentDocumentation)
        {
            var methodDocs = commentDocumentation.ForMethod(reflectionStatementDefinition.MethodInfo);
            return new StatementDocumentation() { 
                Parameters = (reflectionStatementDefinition.Parameters ?? Enumerable.Empty<StatementParameter>())
                              .Select(p => new ParameterDocumentation(){
                                  Name = p.Name,
                                  Type = p.Type,
                                  Description = methodDocs.With(d => d.Parameters.With(p.Name)),
                                  TransformsTo = (p as ProceedParameter).With(z => z.TransformsTo)
                              })
                              .ToList(),
                Description = methodDocs.With(x => x.Summary),
                ScopeType = reflectionStatementDefinition.ScopeType,
                Title = ExtractTitle(reflectionStatementDefinition)
            };
        }

        private static string ExtractTitle(ReflectionStatementDefinition reflectionStatementDefinition)
        {
            //Use attribute if present
            var titleAttr = reflectionStatementDefinition
                .MethodInfo.GetCustomAttributes(typeof(TitleAttribute),true)
                .OfType<TitleAttribute>()
                .Select(x => x.Title)
                .FirstOrDefault(x => !string.IsNullOrEmpty(x));
            if (titleAttr != null) return titleAttr;
            var expression = reflectionStatementDefinition.MatchingCriteria.ToString();
            if (Regex.IsMatch(expression, @"\(\?")) return reflectionStatementDefinition.MethodInfo.Name; //Compilcated regexes just use method name
            return ParseTitle(expression, reflectionStatementDefinition.Parameters);
        }
    }

    public class ParameterDocumentation
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string Description { get; set; }
        public Type TransformsTo { get; set; }
    }
}
