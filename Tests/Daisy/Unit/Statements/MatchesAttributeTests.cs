namespace Ancestry.Daisy.Tests.Daisy.Unit.Statements
{
    using Ancestry.Daisy.Statements;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Unit")]
    public class MatchesAttributeTests
    {
        /// <summary>
        /// Its the addes implicit starts and ends.
        /// </summary>
        /// <param name="in">The in.</param>
        /// <returns></returns>
        [TestCase("a", Result = "^a$")]
        [TestCase("^a", Result = "^a$")]
        [TestCase("a$", Result = "^a$")]
        [TestCase("^a$", Result = "^a$")]
        public string ItAddesImplicitStartsAndEnds(string @in)
        {
            return new MatchesAttribute(@in).RegularExpression.ToString();
        }
    }
}
