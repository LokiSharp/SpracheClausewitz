using NUnit.Framework;
using Sprache;

namespace Clausewitz.Dsl.Tests
{
    [TestFixture]
    public class ClausewitzParserTests
    {
        
        [Test]
        public void AnIdentifierIsAInteger()
        {
            var input = "1";
            var parsed = ClausewitzParser.INT.End().Parse(input);
            Assert.AreEqual(1, parsed);
        }
        
        [Test]
        public void AnIdentifierIsAPCT()
        {
            var input = "1%";
            var parsed = ClausewitzParser.PCT.End().Parse(input);
            Assert.AreEqual(0.01, parsed);
        }
        
        [Test]
        public void AnIdentifierIsAREAL()
        {
            var input = "1.1";
            var parsed = ClausewitzParser.REAL.End().Parse(input);
            Assert.AreEqual(1.1, parsed);
        }
    }
}