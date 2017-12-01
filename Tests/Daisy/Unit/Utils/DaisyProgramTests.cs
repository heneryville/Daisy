namespace Ancestry.Daisy.Tests.Daisy.Unit.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.AST;
    using Ancestry.Daisy.Linking;
    using Ancestry.Daisy.Program;
    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.TestObjects;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Unit")]
    public class DaisyProgramTests
    {
        private TestCaseData[] itRunsPrograms =
            {
                new TestCaseData("t")
                .SetName("It executes statements that are true")
                .Returns(true),
                new TestCaseData("f")
                .SetName("It executes statements that are false")
                .Returns(false),
                new TestCaseData("t\r\nt")
                .SetName("It evaluates ands - happy")
                .Returns(true),
                new TestCaseData("t\r\nf")
                .SetName("It evaluates ands - sad")
                .Returns(false),
                new TestCaseData("t\r\nOR f")
                .SetName("It evaluates ors - happy")
                .Returns(true),
                new TestCaseData("f\r\nOR f")
                .SetName("It evaluates ors - sad")
                .Returns(false),
                new TestCaseData("NOT f")
                .SetName("It evaluates nots - happy")
                .Returns(true),
                new TestCaseData("NOT t")
                .SetName("It evaluates nots - sad")
                .Returns(false),
                new TestCaseData("t\nAND\n  f\n  OR t")
                .SetName("It evaluates groups - happy")
                .Returns(true),
                new TestCaseData("t\nAND\n  f\n  OR f")
                .SetName("It evaluates groups - sad")
                .Returns(false),
                new TestCaseData(TestData.Code_f)
                .SetName("It evaluates a complicated program")
                .Returns(false),
            };

        /// <summary>
        /// Its the runs programs.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        [TestCaseSource("itRunsPrograms")]
        public bool ItRunsPrograms(string code)
        {
            var ast = DaisyParser.Parse(code);
            AddLink(ast, "t", i => true);
            AddLink(ast, "f", i => false);
            var program = new DaisyProgram<int>(ast,DaisyMode.Debug);
            var result = program.Execute(1).Outcome;
            return result;
        }

        /// <summary>
        /// Its the executes aggregates.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        [TestCase("any\n  t", "1,2,3,4,5" ,Result = true)]
        [TestCase("any\n  even", "1,2,3,4,5" ,Result = true)]
        [TestCase("any\n  even", "1,3,5" ,Result = false)]
        public bool ItExecutesAggregates(string code,string values)
        {
            var ast = DaisyParser.Parse(code);
            AddLink(ast, "even", i => i%2 == 0);
            AddLink(ast, "t", i => true);
            AddAggregateLink(ast, "any");
            var program = new DaisyProgram<IEnumerable<int>>(ast,DaisyMode.Debug);
            var result = program.Execute(values.Split(',').Select(int.Parse)).Outcome;
            return result;
        }

        /// <summary>
        /// Adds the link.
        /// </summary>
        /// <param name="ast">The ast.</param>
        /// <param name="rawStatement">The raw statement.</param>
        /// <param name="predicate">The predicate.</param>
        public void AddLink(DaisyAst ast, string rawStatement, Func<int,bool> predicate)
        {
            var statement = new FakeStatement(rawStatement, predicate).Link(rawStatement);
            new AstCollector(ast)
                .OfType<StatementNode>()
                .Where(x => x.Text == rawStatement)
                .Select(x => { x.LinkedStatement = statement; return x; }) //Sigh, I wish I had a for each
                .ToList();
        }

        private void AddAggregateLink(DaisyAst ast, string rawStatement)
        {
            var statement = new FakeAggregate<int,int>(rawStatement).Link(rawStatement);
            new AstCollector(ast)
                .OfType<StatementNode>()
                .Where(x => x.Text == rawStatement)
                .Select(x => { x.LinkedStatement = statement; return x; }) //Sigh, I wish I had a for each
                .ToList();
        }
    }
}
