namespace Ancestry.Daisy.Language.Walks
{
    using System;

    using Ancestry.Daisy.Language.AST;

    public abstract class AstTreeWalker<T> where T : IDaisyAstNode
    {
        public T Root { get; private set; }

        protected AstTreeWalker(T root)
        {
            Root = root;
        }

        protected void Walk()
        {
            Walk(Root);
        }

        private void Walk(T node)
        {
            if (node is IAndOperatorNode<T>)
            {
                var and = node as IAndOperatorNode<T>;
                if(PreVisit(and))
                {
                    Walk(and.Left);
                    InfixVisit(and);
                    Walk(and.Right);
                    PostVisit(and);
                }
            }
            else if (node is IOrOperatorNode<T>)
            {
                var or = node as IOrOperatorNode<T>;
                if(PreVisit(or))
                {
                    Walk(or.Left);
                    InfixVisit(or);
                    Walk(or.Right);
                    PostVisit(or);
                }
            }
            else if (node is INotOperatorNode<T>)
            {
                var not = node as INotOperatorNode<T>;
                if(PreVisit(not))
                {
                    Walk(not.Inner);
                    PostVisit(not);
                }
            }
            else if (node is IGroupOperatorNode<T>)
            {
                var group = node as IGroupOperatorNode<T>;
                if(PreVisit(group))
                {
                    Walk(group.Root);
                    PostVisit(group);
                }
            }
            else if (node is IStatementNode)
            {
                var statement = node as IStatementNode;
                Visit(statement);
            }
            else if(node == null)
            {
                Visit();
            }
            else
            {
                throw new Exception("Don't know how to walk " + node);
            }
        } 

        protected virtual bool PreVisit(IAndOperatorNode<T> node) { return true; }
        protected virtual bool PreVisit(IOrOperatorNode<T> node) { return true; }
        protected virtual bool PreVisit(INotOperatorNode<T> node) { return true; }
        protected virtual bool PreVisit(IGroupOperatorNode<T> node) { return true; }

        protected virtual void InfixVisit(IAndOperatorNode<T> node) {}
        protected virtual void InfixVisit(IOrOperatorNode<T> node) { }

        protected virtual void PostVisit(IAndOperatorNode<T> node) {}
        protected virtual void PostVisit(IOrOperatorNode<T> node) {}
        protected virtual void PostVisit(INotOperatorNode<T> node) {}
        protected virtual void PostVisit(IGroupOperatorNode<T> node) {}

        protected virtual void Visit(IStatementNode node) {}

        protected virtual void Visit() {}
    }
}
