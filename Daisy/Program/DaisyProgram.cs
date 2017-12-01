using System.Collections.Generic;
using System.Diagnostics;
using Ancestry.Daisy.Language.AST.Trace;

namespace Ancestry.Daisy.Program
{
    using System;

    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.AST;
    using Ancestry.Daisy.Statements;

    public class DaisyProgram<T>
    {
        public DaisyMode Mode { get; private set; }
        private readonly DaisyAst ast;

        public DaisyProgram(DaisyAst ast, DaisyMode mode)
        {
            Mode = mode;
            this.ast = ast;
        }

        public IDaisyExecution Execute(T scope)
        {
            return Execute(scope, new ContextBundle());
        }

        public IDaisyExecution Execute(T scope, ContextBundle context)
        {
            var execution = new DaisyExecution(ast, Mode);
            execution.DebugInfo.PerformanceCounter.Start();
            var traced = Execute(scope, ast.Root, execution, context);
            execution.Outcome = traced.Outcome;
            execution.DebugInfo.Trace = traced;
            execution.DebugInfo.PerformanceCounter.End();
            return execution;
        }

        private ITracer BuildTracer()
        {
            if(Mode == DaisyMode.Debug) return new Tracer();
            return new NoOpTracer();
        }

        [DebuggerStepThrough]
        private TraceNode Execute(object scope, IDaisyAstNode node, DaisyExecution daisyExecution, ContextBundle context)
        {
            daisyExecution.DebugInfo.PerformanceCounter.CountOp();
            if(node is AndOperatorNode)
            {
                var and = node as AndOperatorNode;
                var leftTrace = Execute(scope, and.Left, daisyExecution, context);
                if (!leftTrace.Outcome) return new AndOperatorTrace(leftTrace, null,scope, false);
                var rightTrace = Execute(scope, and.Right, daisyExecution, context);
                return new AndOperatorTrace(leftTrace, rightTrace, scope, leftTrace.Outcome && rightTrace.Outcome);
            }
            else if(node is OrOperatorNode)
            {
                var or = node as OrOperatorNode;
                var leftTrace = Execute(scope, or.Left, daisyExecution, context);
                if (leftTrace.Outcome) return new OrOperatorTrace(leftTrace, null,scope, true);
                var rightTrace = Execute(scope, or.Right, daisyExecution, context);
                return new OrOperatorTrace(leftTrace, rightTrace,scope, leftTrace.Outcome || rightTrace.Outcome);
            }
            else if(node is NotOperatorNode)
            {
                var not = node as NotOperatorNode;
                var trace = Execute(scope, not.Inner, daisyExecution, context);
                return new NotOperatorTrace(trace,scope, !trace.Outcome);
            }
            else if(node is GroupOperatorNode)
            {
                var group = node as GroupOperatorNode;
                if (group.HasCommand)
                {
                    var link = group.LinkedStatement;
                    if (link == null) throw new DaisyRuntimeException(string.Format("Group '{0}' was never linked", group.Text));
                    var frames = new List<TraceNode>();
                    var tracer = BuildTracer();
                    var result =  link.Execute(new InvokationContext() {
                        Scope = scope,
                        Proceed = o =>
                        {
                            var frame = Execute(o, @group.Root, daisyExecution, context);
                            frames.Add(frame);
                            return frame.Outcome;
                        },
                        Context = context,
                        Attachments = daisyExecution.Attachments,
                        Tracer = tracer,
                        PerformanceCounter = daisyExecution.DebugInfo.PerformanceCounter
                    });
                    return new GroupOperatorTrace(group.Text, tracer.Tracings, frames,scope, result);
                } else
                {
                    var trace = Execute(scope, @group.Root, daisyExecution, context);
                    return trace;
                }
            }
            else if(node is StatementNode)
            {
                var statement = node as StatementNode;
                var link = statement.LinkedStatement;
                if (link == null) throw new DaisyRuntimeException(string.Format("Statement '{0}' was never linked.", statement.Text));
                var tracer = BuildTracer();
                var result = link.Execute(new InvokationContext() {
                        Scope = scope,
                        Proceed = o => true,  //I don't know. True since the empty statement is true?
                        Context = context,
                        Attachments = daisyExecution.Attachments,
                        Tracer = tracer,
                        PerformanceCounter = daisyExecution.DebugInfo.PerformanceCounter
                    });
                return new StatementTrace(statement.Text,tracer.Tracings, scope, result);
            }
            throw new Exception("Don't know how to walk nodes of type: " + node.GetType());
        }
    }
}
