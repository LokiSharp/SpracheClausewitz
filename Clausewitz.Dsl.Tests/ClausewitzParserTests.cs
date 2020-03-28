using System;
using System.Collections.Generic;
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
            Assert.AreEqual("Hello", parsed);
        }

        [TestMethod]
        public void ParsedIsComment()
        {
            var input = "# Hello";
            var parsed = ClausewitzParser.Comment.End().Parse(input);
            Assert.AreEqual("Hello", parsed);
        }

        [TestMethod]
        public void ParsedIsDate()
        {
            var input = "1444.11.11";
            var parsed = ClausewitzParser.Date.End().Parse(input);
            Assert.AreEqual(DateTime.Parse("1444-11-11"), parsed);
        }

        [TestMethod]
        public void ParsedIsInteger()
        {
            var input = "1";
            var parsed = ClausewitzParser.Integer.End().Parse(input);
            Assert.AreEqual(1, parsed);
        }

        [TestMethod]
        public void ParsedIsList()
        {
            var input = @"{ 1 1% 1.1 1444.11.11 
	{ 
		2 
		2% 
		2.1 
		1444.11.11 
		abc_def 
	} 
}";
            var parsed = (List<object>) ClausewitzParser.List.End().Parse(input);
            Assert.AreEqual(1, parsed[0]);
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
            Assert.AreEqual(0.01, parsed);
        }

        [TestMethod]
        public void ParsedIsReal()
        {
            var input = "1.1";
            var parsed = ClausewitzParser.Real.End().Parse(input);
            Assert.AreEqual(1.1, parsed);
        }
    }
}