using System;
using Clausewitz.Dsl.SyntaxTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;

namespace Clausewitz.Dsl.Tests
{
    [TestClass]
    public class ClausewitzParserTests
    {
        [TestMethod]
        public void ParsedIsAssignment()
        {
            var input = @"sub_units = {
	amphibious_armor = {
		sprite = amphibious_armor
		map_icon_category = armored
		priority = 2501
		ai_priority = 2000
		active = yes
		special_forces = yes
		marines = yes
		type = {
			armor
		}
		
		group = armor
		
		categories = {
			category_tanks
			category_front_line
			category_all_armor
			category_army
		}
	}
}";
            var parsed = ClausewitzParser.Assignment.End().Parse(input);
            Assert.AreEqual("sub_units", parsed.Item1);
        }

        [TestMethod]
        public void ParsedIsDate()
        {
            var input = "1444.11.11";
            var parsed = ClausewitzParser.Date.End().Parse(input);
            Assert.AreEqual(DateTime.Parse("1444-11-11"), parsed.Get());
        }

        [TestMethod]
        public void ParsedIsInteger()
        {
            var input = "-1";
            var parsed = ClausewitzParser.Integer.End().Parse(input);
            Assert.AreEqual(-1, parsed.Get());
        }

        [TestMethod]
        public void ParsedIsList()
        {
            var input = @"{
	1
	1%
	1.1
	1444.11.11
}";
            var parsed = (ClausewitzList) ClausewitzParser.List.End().Parse(input);
            Assert.AreEqual("1", parsed.Elements[0].ToString());
        }

        [TestMethod]
        public void ParsedIsMap()
        {
            var input = @"{
	sprite = amphibious_armor
	map_icon_category = armored
	priority = 2501
	ai_priority = 2000
	active = yes
	special_forces = yes
	marines = yes
}";
            var parsed = (ClausewitzMap) ClausewitzParser.Map.End().Parse(input);
            Assert.AreEqual("amphibious_armor", parsed.Pairs["sprite"].ToString());
        }

        [TestMethod]
        public void ParsedIsOperator()
        {
            var parser = ClausewitzParser.Operator.End();
            Assert.AreEqual(OperatorType.InEqual, parser.Parse("<>"));
            Assert.AreEqual(OperatorType.LessThanOrEqual, parser.Parse("<="));
            Assert.AreEqual(OperatorType.GreaterThanOrEqual, parser.Parse(">="));
            Assert.AreEqual(OperatorType.LessThan, parser.Parse("<"));
            Assert.AreEqual(OperatorType.GreaterThan, parser.Parse(">"));
            Assert.AreEqual(OperatorType.Equal, parser.Parse("="));
        }

        [TestMethod]
        public void ParsedIsPercent()
        {
            var input = "1%";
            var parsed = ClausewitzParser.Percent.End().Parse(input);
            Assert.AreEqual(0.01, parsed.Get());
        }

        [TestMethod]
        public void ParsedIsReal()
        {
            var input = "1.1";
            var parsed = ClausewitzParser.Real.End().Parse(input);
            Assert.AreEqual(1.1, parsed.Get());
        }

        [TestMethod]
        public void ParsedIsClausewitzPair()
        {
            var input = "abc = def";
            var parsed = ClausewitzParser.ClausewitzPair.End().Parse(input);
            Assert.AreEqual("abc", parsed.Key);
        }
    }
}