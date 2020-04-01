using System;
using System.Globalization;
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
            var parsed = ClausewitzParser.ClausewitzAssignment.End().Parse(input);
            Assert.AreEqual("sub_units", parsed.Key);
            Assert.AreEqual("amphibious_armor", parsed.Value["amphibious_armor"]["sprite"].ToString());
            Assert.AreEqual("armored", parsed.Value["amphibious_armor"]["map_icon_category"].ToString());
            Assert.AreEqual("2501", parsed.Value["amphibious_armor"]["priority"].ToString());
            Assert.AreEqual("2000", parsed.Value["amphibious_armor"]["ai_priority"].ToString());
            Assert.AreEqual("yes", parsed.Value["amphibious_armor"]["active"].ToString());
            Assert.AreEqual("yes", parsed.Value["amphibious_armor"]["special_forces"].ToString());
            Assert.AreEqual("yes", parsed.Value["amphibious_armor"]["marines"].ToString());
            Assert.AreEqual("armor", parsed.Value["amphibious_armor"]["type"][0].ToString());
            Assert.AreEqual("armor", parsed.Value["amphibious_armor"]["group"].ToString());
            Assert.AreEqual("category_tanks", parsed.Value["amphibious_armor"]["categories"][0].ToString());
            Assert.AreEqual("category_front_line", parsed.Value["amphibious_armor"]["categories"][1].ToString());
            Assert.AreEqual("category_all_armor", parsed.Value["amphibious_armor"]["categories"][2].ToString());
            Assert.AreEqual("category_army", parsed.Value["amphibious_armor"]["categories"][3].ToString());
        }

        // [TestMethod]
        // public void ParsedIsDate()
        // {
        //     var input = "1444.11.11";
        //     var parsed = ClausewitzParser.Date.End().Parse(input);
        //     Assert.AreEqual(DateTime.Parse("1444-11-11").ToString(CultureInfo.CurrentCulture), parsed.ToString());
        // }

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
	""1444.11.11""
	{
		1
		1%
		1.1
		""1444.11.11""
	}
}";
            var parsed = (ClausewitzList) ClausewitzParser.List.End().Parse(input);
            Assert.AreEqual("1", parsed[0].ToString());
            Assert.AreEqual("0.01", parsed[1].ToString());
            Assert.AreEqual("1.1", parsed[2].ToString());
            Assert.AreEqual("1444.11.11", parsed[3].ToString());
            Assert.AreEqual("1", parsed[4][0].ToString());
            Assert.AreEqual("0.01", parsed[4][1].ToString());
            Assert.AreEqual("1.1", parsed[4][2].ToString());
            Assert.AreEqual("1444.11.11", parsed[4][3].ToString());
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
	amphibious_armor = {
		sprite = amphibious_armor
		map_icon_category = armored
		priority = 2501
		ai_priority = 2000
		active = yes
		special_forces = yes
		marines = yes
	}
}";
            var parsed = (ClausewitzMap) ClausewitzParser.Map.End().Parse(input);
            Assert.AreEqual("amphibious_armor", parsed["sprite"].ToString());
            Assert.AreEqual("armored", parsed["map_icon_category"].ToString());
            Assert.AreEqual("2501", parsed["priority"].ToString());
            Assert.AreEqual("2000", parsed["ai_priority"].ToString());
            Assert.AreEqual("yes", parsed["active"].ToString());
            Assert.AreEqual("yes", parsed["special_forces"].ToString());
            Assert.AreEqual("yes", parsed["marines"].ToString());
            Assert.AreEqual("amphibious_armor", parsed["amphibious_armor"]["sprite"].ToString());
            Assert.AreEqual("armored", parsed["amphibious_armor"]["map_icon_category"].ToString());
            Assert.AreEqual("2501", parsed["amphibious_armor"]["priority"].ToString());
            Assert.AreEqual("2000", parsed["amphibious_armor"]["ai_priority"].ToString());
            Assert.AreEqual("yes", parsed["amphibious_armor"]["active"].ToString());
            Assert.AreEqual("yes", parsed["amphibious_armor"]["special_forces"].ToString());
            Assert.AreEqual("yes", parsed["amphibious_armor"]["marines"].ToString());
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
            Assert.AreEqual("0.01", parsed.ToString());
        }

        [TestMethod]
        public void ParsedIsReal()
        {
            var input = "1.1";
            var parsed = ClausewitzParser.Real.End().Parse(input);
            Assert.AreEqual(1.1, parsed.Get());
        }
    }
}