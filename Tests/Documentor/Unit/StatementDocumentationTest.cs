namespace Ancestry.Daisy.Tests.Documentor.Unit
{
    using System.Linq;

    using Ancestry.Daisy.Documentation;
    using Ancestry.Daisy.Statements;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Unit")]
    public class StatementDocumentationTest
    {
        /// <summary>
        /// Its the parses titles.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="expected">The expected.</param>
        [TestCase("A","","A",TestName = "It uses method name when matches isn't available")]
        [TestCase("^A$","","A",TestName = "It replaces regex special tokens")]
        [TestCase(@"Beast\s+here","", "Beast here", TestName = "It converts matches expression names into something nice")]
        [TestCase(@"Date\s+between\s+years\s+(\d+)\s+to\s+(\d+)", "start,end"
            , "Date between years [start] to [end]"
            , TestName = "It uses parameter names in match expressions")]
        public void ItParsesTitles(string regex,string parameters, string expected )
        {
            var title = StatementDocumentation.ParseTitle(regex, parameters.Split(',').Select(x => new StatementParameter { Name = x }).ToList());
            Assert.AreEqual(expected, title);
        }
    }
}
