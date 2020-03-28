using System;
using System.Collections.Generic;
using System.Linq;
using Clausewitz.Dsl.SyntaxTree;
using Sprache;

namespace Clausewitz.Dsl
{
    public static class ClausewitzParser
    {
        private static readonly Parser<char> DoubleQuote = Parse.Char('"');
        private static readonly Parser<string> Space = Parse.WhiteSpace.Many().Text();
        private static readonly Parser<char> BlockStart = Parse.Char('{');
        private static readonly Parser<char> BlockEnd = Parse.Char('}');
        private static readonly Parser<string> Tabs = Parse.Char('\t').Many().Text();

        private static readonly Parser<string> LineEndAndTabs =
            from d in Space.Optional()
            from le in Parse.LineEnd.Optional()
            from tabs in Tabs.Optional()
            select "";

        private static readonly Parser<ClausewitzLiteral> Symbol =
            from symbol in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit.Or(Parse.Char('_')))
            select new ClausewitzLiteral(symbol, LiteralType.Symbol);

        public static readonly Parser<OperatorType> Operator =
            Parse.String("<>").Return(OperatorType.InEqual)
                .Or(Parse.String("<=").Return(OperatorType.LessThanOrEqual))
                .Or(Parse.String(">=").Return(OperatorType.GreaterThanOrEqual))
                .Or(Parse.String("<").Return(OperatorType.LessThan))
                .Or(Parse.String(">").Return(OperatorType.GreaterThan))
                .Or(Parse.String("=").Return(OperatorType.Equal));

        public static readonly Parser<object> Comment =
            from op in Parse.String("#")
            from space in Space.Optional()
            from comment in Parse.CharExcept('\n').Many().Text()
            select comment;

        public static readonly Parser<ClausewitzLiteral> Integer =
            from minus in Parse.String("-").Text().Optional()
            from digits in Parse.Digit.Many().Text()
            select new ClausewitzLiteral((minus.IsDefined ? minus.Get() : "") + digits, LiteralType.Integer);

        public static readonly Parser<ClausewitzLiteral> Percent =
            from minus in Parse.String("-").Text().Optional()
            from digits in Parse.Digit.Many().Text()
            from pct in Parse.Char('%')
            select new ClausewitzLiteral((minus.IsDefined ? minus.Get() : "") + digits, LiteralType.Percent);

        public static readonly Parser<ClausewitzLiteral> Real =
            from minus in Parse.String("-").Text().Optional()
            from integers in Parse.Digit.Many().Text()
            from dot in Parse.Char('.')
            from decimals in Parse.Digit.Many().Text()
            select new ClausewitzLiteral((minus.IsDefined ? minus.Get() : "") + $"{integers}.{decimals}",
                LiteralType.Real);

        public static readonly Parser<ClausewitzLiteral> Date =
            from year in Parse.Number
            from aDot in Parse.Char('.')
            from month in Parse.Number
            from bDot in Parse.Char('.')
            from day in Parse.Number
            select new ClausewitzLiteral($"{year}-{month}-{day}", LiteralType.Date);

        public static readonly Parser<ClausewitzLiteral> ClausewitzLiteral =
            Symbol.Or(Integer).Or(Percent).Or(Date).Or(Real);

        public static readonly Parser<Tuple<ClausewitzLiteral, OperatorType, IClausewitzValue>> Assignment =
            from lt1 in Parse.LineEnd.Optional()
            from tb1 in Tabs.Optional()
            from name in Symbol
            from leading in Space.Optional()
            from op in Operator
            from trailing in Space.Optional()
            from value in Map.Or(List).Or(Date).Or(Percent).Or(Real).Or(Integer).Or(Symbol)
            from lt2 in Parse.LineEnd.Optional()
            from tb2 in Tabs.Optional()
            select new Tuple<ClausewitzLiteral, OperatorType, IClausewitzValue>(name, op, value);


        public static readonly Parser<KeyValuePair<string, IClausewitzValue>> ClausewitzPair =
            from name in Symbol
            from leading in Space.Optional()
            from colon in Parse.Char('=').Token()
            from trailing in Space.Optional()
            from val in ClausewitzValue
            select new KeyValuePair<string, IClausewitzValue>(name.Value, val);

        public static readonly Parser<IEnumerable<KeyValuePair<string, IClausewitzValue>>> ClausewitzMembers =
            ClausewitzPair.DelimitedBy(LineEndAndTabs);

        public static readonly Parser<ClausewitzMap> Map =
            from bs in BlockStart
            from le1 in Parse.LineEnd.Optional()
            from leading in Space.Optional()
            from values in ClausewitzMembers
            from trailing in Space.Optional()
            from le2 in Parse.LineEnd.Optional()
            from be in BlockEnd
            select new ClausewitzMap(values);

        static readonly Parser<IClausewitzValue> ClausewitzValue =
            Parse.Ref(() => Map)
                .Or(Parse.Ref(() => List))
                .Or(ClausewitzLiteral);

        static readonly Parser<IEnumerable<IClausewitzValue>> JElements =
            ClausewitzValue.DelimitedBy(LineEndAndTabs);

        public static readonly Parser<IClausewitzValue> List =
            from bs in BlockStart
            from le1 in Parse.LineEnd.Optional()
            from leading in Space.Optional()
            from values in JElements
            from trailing in Space.Optional()
            from le2 in Parse.LineEnd.Optional()
            from be in BlockEnd
            select new ClausewitzList(values);
    }
}