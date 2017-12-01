namespace Ancestry.Daisy.Tests.Daisy.Unit.Utils
{
    using Ancestry.Daisy.Utils;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Unit")]
    public class StringExtentionsTests
    {
        /// <summary>
        /// Its the splits based on casing.
        /// </summary>
        /// <param name="in">The in.</param>
        /// <param name="expected">The expected.</param>
        [TestCase("","")]
        [TestCase("a","a")]
        [TestCase("A","A")]
        [TestCase("AaBb","Aa|Bb")]
        [TestCase("TheDogIsIn","The|Dog|Is|In")]
        [TestCase("theDogIsIn","the|Dog|Is|In")]
        [TestCase("a_b_c","a|b|c")]
        [TestCase("and_then_ItHappened","and|then|It|Happened")]
        public void ItSplitsBasedOnCasing(string @in, string expected)
        {
            var @out = StringExtentions.SplitNameParts(@in);
            var normed = string.Join("|", @out);
            Assert.AreEqual(expected, normed);
        }
    }
}
