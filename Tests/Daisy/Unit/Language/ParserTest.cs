namespace Ancestry.Daisy.Tests.Daisy.Unit.Language
{
    using System;

    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.Walks;
    using Ancestry.Daisy.Utils;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Unit")]
    public class ParserTest
    {
        /// <summary>
        /// Its the parses languages.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="expectedTree">The expected tree.</param>
        [TestCase("a\nAND b","AND\r\n-a\r\n-b\r\n",TestName = "It parses ands")]
        [TestCase("a\nOR b","OR\r\n-a\r\n-b\r\n",TestName = "It parses ors")]
        [TestCase("a\nOR NOT b","OR\r\n-a\r\n-NOT\r\n--b\r\n",TestName = "It parses nots")]
        [TestCase("a\nNOT b","AND\r\n-a\r\n-NOT\r\n--b\r\n",TestName = "It parses nots after implicit ands")]
        [TestCase("a\nb","AND\r\n-a\r\n-b\r\n",TestName = "It parses implicit ands")]
        [TestCase("a\nAND b\nAND c","AND\r\n-AND\r\n--a\r\n--b\r\n-c\r\n",TestName = "It parses chained statements")]
        [TestCase(
@"a
AND b
AND c
OR NOT d
AND NOT e",
            "AND\r\n-OR\r\n--AND\r\n---AND\r\n----a\r\n----b\r\n---c\r\n--NOT\r\n---d\r\n-NOT\r\n--e\r\n",
            TestName = "It parses deeply chained statements")]
        [TestCase(
@"a
AND
  b
  OR c",
            "AND\r\n-a\r\n-GROUP\r\n--OR\r\n---b\r\n---c\r\n",
            TestName = "It parses anonymous groups")]
        [TestCase(
@"a
AND d
  b
  OR c",
            "AND\r\n-a\r\n-GROUP@d\r\n--OR\r\n---b\r\n---c\r\n",
            TestName = "It parses named groups")]
        [TestCase(
@"a
  b",
            "GROUP@a\r\n-b\r\n",
            TestName = "It parses groups")]
        public void ItParsesLanguages(string code, string expectedTree)
        {
            var llstream = new LookAheadStream<Token>(new Lexer(code.ToStream()).Lex());
            var parser = new DaisyParser(llstream);
            var tree = parser.Parse();
            Assert.IsNotNull(tree);
            var actualTree = DaisyAstPrinter.Print(tree.Root);
            if(expectedTree != actualTree)
            {
                Console.WriteLine(expectedTree);
                Console.WriteLine("----------------");
                Console.WriteLine(actualTree);
            }
            Assert.AreEqual(expectedTree, actualTree);
        }
    }
}
