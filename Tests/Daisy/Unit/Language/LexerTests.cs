namespace Ancestry.Daisy.Tests.Daisy.Unit.Language
{
    using System.Collections.Generic;
    using System.Linq;

    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Utils;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Unit")]
    public class LexerTests
    {
        private readonly TestCaseData[] itInterpretsLines =
            {
                new TestCaseData("a",new []{new Token(){Kind = TokenKind.Statement, Value = "a"}}),
                new TestCaseData("",new Token[]{}),
                new TestCaseData("AND",new []{new Token(){Kind = TokenKind.And}}),
                new TestCaseData("OR",new []{new Token(){Kind = TokenKind.Or}}),
                new TestCaseData("NOT",new []{new Token(){Kind = TokenKind.Not}}),
                new TestCaseData("AND ",new []{new Token(){Kind = TokenKind.And}}),
                new TestCaseData("OR ",new []{new Token(){Kind = TokenKind.Or}}),
                new TestCaseData("NOT ",new []{new Token(){Kind = TokenKind.Not}}),
                new TestCaseData("AND a",new []{new Token(){Kind = TokenKind.And},new Token(){Kind = TokenKind.Statement, Value = "a"}}),
                new TestCaseData("OR a",new []{new Token(){Kind = TokenKind.Or},new Token(){Kind = TokenKind.Statement, Value = "a"}}),
                new TestCaseData("NOT a",new []{new Token(){Kind = TokenKind.Not},new Token(){Kind = TokenKind.Statement, Value = "a"}}),
                new TestCaseData("AND  a",new []{new Token(){Kind = TokenKind.And},new Token(){Kind = TokenKind.Statement, Value = "a"}}),
                new TestCaseData("OR  a",new []{new Token(){Kind = TokenKind.Or},new Token(){Kind = TokenKind.Statement, Value = "a"}}),
                new TestCaseData("NOT  a",new []{new Token(){Kind = TokenKind.Not},new Token(){Kind = TokenKind.Statement, Value = "a"}}),
                new TestCaseData("ORa",new []{new Token(){Kind = TokenKind.Statement, Value = "ORa"}}),
                new TestCaseData("AND NOT",new []{new Token(){Kind = TokenKind.And},new Token(){Kind = TokenKind.Not}}),
                new TestCaseData("OR NOT",new []{new Token(){Kind = TokenKind.Or},new Token(){Kind = TokenKind.Not}}),
                new TestCaseData("AND  NOT",new []{new Token(){Kind = TokenKind.And},new Token(){Kind = TokenKind.Not}}),
                new TestCaseData("OR  NOT",new []{new Token(){Kind = TokenKind.Or},new Token(){Kind = TokenKind.Not}}),
                new TestCaseData("OR  NOT  a a a and a",new []{new Token(){Kind = TokenKind.Or},new Token(){Kind = TokenKind.Not}, new Token(){Kind = TokenKind.Statement, Value = "a a a and a"}}),
                new TestCaseData("a//A comment",new []{new Token(){Kind = TokenKind.Statement, Value = "a"}}),
                new TestCaseData("a //A comment",new []{new Token(){Kind = TokenKind.Statement, Value = "a"}}),
                new TestCaseData("//A comment",new Token[]{}),
            };

        /// <summary>
        /// Its the interprets lines.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="expected">The expected.</param>
        [TestCaseSource("itInterpretsLines")]
        public void ItInterpretsLines(string line, IEnumerable<Token> expected)
        {
            var tokens = new Lexer(line.ToStream()).InterpretLine(line).ToArray();
            AssertTokenStreamEquals(expected.ToArray(), tokens);
        }

        /// <summary>
        /// Its the counts and trims leading spaces.
        /// </summary>
        /// <param name="in">The in.</param>
        /// <param name="count">The count.</param>
        /// <param name="out">The out.</param>
        [TestCase("a",0,"a")]
        [TestCase(" a",1,"a")]
        [TestCase("  a",2,"a")]
        [TestCase("  a a a  ",2,"a a a  ")]
        public void ItCountsAndTrimsLeadingSpaces(string @in, int count, string @out)
        {
            var aCount = Lexer.TrimLeadingSpaces(ref @in);
            Assert.AreEqual(@out, @in);
            Assert.AreEqual(count, aCount);
        }

        private readonly TestCaseData[] itFiguresOutGroupStartOrEnd =
            {
                new TestCaseData(0,"a",new []{new Token(){Kind = TokenKind.Statement, Value = "a"}}),
                new TestCaseData(0," a",new []{new Token(){Kind = TokenKind.StartGroup},new Token(){Kind = TokenKind.Statement, Value = "a"}}),
                new TestCaseData(1,"a",new []{new Token(){Kind = TokenKind.EndGroup},new Token(){Kind = TokenKind.Statement, Value = "a"}}),
            };

        /// <summary>
        /// Its the figoures out group start or end.
        /// </summary>
        /// <param name="spacesBefore">The spaces before.</param>
        /// <param name="line">The line.</param>
        /// <param name="expected">The expected.</param>
        [TestCaseSource("itFiguresOutGroupStartOrEnd")]
        public void ItFigouresOutGroupStartOrEnd(int spacesBefore,string line, IEnumerable<Token> expected)
        {
            var lexer = new Lexer(line.ToStream());
            lexer.InterpretLine(string.Empty.PadRight(spacesBefore));
            var tokens = lexer.InterpretLine(line).ToArray();
            AssertTokenStreamEquals(expected.ToArray(), tokens);
        }

        /// <summary>
        /// Its the lexes multple close end groups.
        /// </summary>
        [Test]
        public void ItLexesMultpleCloseEndGroups()
        {
            var lexer = new Lexer("a\n\tb\n\t\tc\nd".ToStream());
            var stream = lexer.Lex().ToArray();
            AssertTokenStreamEquals(stream, new []
                {
                    new Token() { Kind = TokenKind.Statement, Value = "a"},
                    new Token() { Kind = TokenKind.StartGroup },
                    new Token() { Kind = TokenKind.Statement, Value = "b"},
                    new Token() { Kind = TokenKind.StartGroup },
                    new Token() { Kind = TokenKind.Statement, Value = "c"},
                    new Token() { Kind = TokenKind.EndGroup },
                    new Token() { Kind = TokenKind.EndGroup },
                    new Token() { Kind = TokenKind.Statement, Value = "d"},
                    new Token() { Kind = TokenKind.EOF},
                });
        }

        /// <summary>
        /// Its the dies on inconsistent whitespace.
        /// </summary>
        /// <param name="code">The code.</param>
        [TestCase("a\n\tb\n c")]
        [TestCase("a\n   b\n  c")]
        public void ItDiesOnInconsistentWhitespace(string code)
        {
            var lexer = new Lexer(code.ToStream());
            Assert.Catch<InconsistentWhitespaceException>(() =>lexer.Lex().ToArray());
        }

        private readonly TestCaseData[] itLexesPrograms =
            {
                new TestCaseData("a",new []
                    {
                        new Token(){Kind = TokenKind.Statement, Value = "a"},
                        new Token(){Kind = TokenKind.EOF},
                    }),
                new TestCaseData(@"a
  b
c",new []
                    {
                        new Token(){Kind = TokenKind.Statement, Value = "a"},
                        new Token(){Kind = TokenKind.StartGroup},
                        new Token(){Kind = TokenKind.Statement, Value = "b"},
                        new Token(){Kind = TokenKind.EndGroup},
                        new Token(){Kind = TokenKind.Statement, Value = "c"},
                        new Token(){Kind = TokenKind.EOF},
                    }),
                new TestCaseData(@"a
  b",new []
                    {
                        new Token(){Kind = TokenKind.Statement, Value = "a"},
                        new Token(){Kind = TokenKind.StartGroup},
                        new Token(){Kind = TokenKind.Statement, Value = "b"},
                        new Token(){Kind = TokenKind.EndGroup},
                        new Token(){Kind = TokenKind.EOF},
                    }),
                new TestCaseData(@"a
//This is a comment
  b //Comment 2
  //Comment 3",new []
                    {
                        new Token(){Kind = TokenKind.Statement, Value = "a"},
                        new Token(){Kind = TokenKind.StartGroup},
                        new Token(){Kind = TokenKind.Statement, Value = "b"},
                        new Token(){Kind = TokenKind.EndGroup},
                        new Token(){Kind = TokenKind.EOF},
                    }),
            };

        /// <summary>
        /// Its the lexes programs.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="expected">The expected.</param>
        [TestCaseSource("itLexesPrograms")]
        public void ItLexesPrograms(string line, IEnumerable<Token> expected)
        {
            var lexer = new Lexer(line.ToStream());
            var tokens = lexer.Lex();
            AssertTokenStreamEquals(expected.ToArray(), tokens.ToArray());
        }

        private void AssertTokenStreamEquals(IList<Token> eTokens, IList<Token> tokens)
        {
            Assert.AreEqual(eTokens.Count, tokens.Count);
            foreach (var tokenPair in eTokens.Zip(tokens, (token, token1) => new {expected = token, actual = token1}))
            {
                Assert.AreEqual(tokenPair.expected.Kind, tokenPair.actual.Kind);
                Assert.AreEqual(tokenPair.expected.Value, tokenPair.actual.Value);
            }
        }
    }
}
