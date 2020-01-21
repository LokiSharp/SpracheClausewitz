using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sprache;

namespace Clausewitz.Dsl.Tests
{
    [TestFixture]
    public class ClausewitzParserTests
    {
        [Test]
        public void AnIdentifierIsAAssignment()
        {
            var input = @"happened = { prestige = 100 legitimacy = 100 stability = 3 }";
            var parsed = ClausewitzParser.Assignment.End().Parse(input);
            Assert.AreEqual("Hello", parsed);
        }

        [Test]
        public void AnIdentifierIsAComment()
        {
            var input = "# Hello";
            var parsed = ClausewitzParser.Comment.End().Parse(input);
            Assert.AreEqual("Hello", parsed);
        }

        [Test]
        public void AnIdentifierIsADate()
        {
            var input = "1444.11.11";
            var parsed = ClausewitzParser.Date.End().Parse(input);
            Assert.AreEqual(DateTime.Parse("1444-11-11"), parsed);
        }

        [Test]
        public void AnIdentifierIsAInteger()
        {
            var input = "1";
            var parsed = ClausewitzParser.Integer.End().Parse(input);
            Assert.AreEqual(1, parsed);
        }

        [Test]
        public void AnIdentifierIsAList()
        {
            var input = "{ 1 1% 1.1 1444.11.11 { 2 2% 2.1 1444.11.11 abc_def } }";
            var parsed = (List<object>) ClausewitzParser.List.End().Parse(input);
            Assert.AreEqual(1, parsed[0]);
        }

        [Test]
        public void AnIdentifierIsAOperator()
        {
            var parser = ClausewitzParser.Operator.End();
            Assert.AreEqual("<>", parser.Parse("<>"));
            Assert.AreEqual("<=", parser.Parse("<="));
            Assert.AreEqual(">=", parser.Parse(">="));
            Assert.AreEqual("<", parser.Parse("<"));
            Assert.AreEqual(">", parser.Parse(">"));
            Assert.AreEqual("=", parser.Parse("="));
        }

        [Test]
        public void AnIdentifierIsAPercent()
        {
            var input = "1%";
            var parsed = ClausewitzParser.Percent.End().Parse(input);
            Assert.AreEqual(0.01, parsed);
        }

        [Test]
        public void AnIdentifierIsAReal()
        {
            var input = "1.1";
            var parsed = ClausewitzParser.Real.End().Parse(input);
            Assert.AreEqual(1.1, parsed);
        }
    }
}