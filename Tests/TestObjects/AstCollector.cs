using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.TestObjects
{
    using System.Collections;

    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.AST;

    /// <summary>
    /// 
    /// </summary>
    public class AstCollector : IEnumerable<IDaisyAstNode>
    {
        private readonly DaisyAst ast;

        /// <summary>
        /// Initializes a new instance of the <see cref="AstCollector"/> class.
        /// </summary>
        /// <param name="ast">The ast.</param>
        public AstCollector(DaisyAst ast)
        {
            this.ast = ast;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<IDaisyAstNode> GetEnumerator()
        {
            return Collect().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Collects this instance.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IDaisyAstNode> Collect()
        {
            var queue = new Stack<IDaisyAstNode>();
            queue.Push(ast.Root);
            while (queue.Count != 0)
            {
                var node = queue.Pop();
                yield return node;
                if (node is AndOperatorNode)
                {
                    var and = node as AndOperatorNode;
                    queue.Push(and.Right);
                    queue.Push(and.Left);
                }
                else if (node is OrOperatorNode)
                {
                    var and = node as OrOperatorNode;
                    queue.Push(and.Right);
                    queue.Push(and.Left);
                }
                else if (node is NotOperatorNode)
                {
                    var and = node as NotOperatorNode;
                    queue.Push(and.Inner);
                }
                else if (node is GroupOperatorNode)
                {
                    var and = node as GroupOperatorNode;
                    queue.Push(and.Root);
                }
            }
        }
    }
}
