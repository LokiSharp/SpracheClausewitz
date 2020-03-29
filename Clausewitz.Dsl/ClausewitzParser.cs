using System.Collections.Generic;
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
            from op in Parse.String("#").Token()
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
            Symbol.XOr(Integer).XOr(Percent).XOr(Date).XOr(Real).Token();

        public static readonly Parser<IClausewitzValue> Assignment =
            from name in Symbol
            from op in Operator.Token()
            from value in Parse.Ref(() => Map)
                .Or(Parse.Ref(() => List))
                .Or(Date)
                .Or(Percent)
                .Or(Real)
                .Or(Integer)
                .Or(Symbol).Token()
            select new ClausewitzAssignment(name.Value, value, op);

        public static readonly Parser<KeyValuePair<string, IClausewitzValue>> ClausewitzPair =
            from name in Symbol
            from colon in Parse.Char('=').Token()
            from val in ClausewitzValue
            select new KeyValuePair<string, IClausewitzValue>(name.Value, val);

        public static readonly Parser<IEnumerable<KeyValuePair<string, IClausewitzValue>>> ClausewitzMembers =
            ClausewitzPair.DelimitedBy(Space.Token());

        public static readonly Parser<IClausewitzValue> Map =
            from bs in BlockStart.Token()
            from values in ClausewitzMembers
            from be in BlockEnd.Token()
            select new ClausewitzMap(values);

        private static readonly Parser<IClausewitzValue> ClausewitzValue =
            Parse.Ref(() => Map)
                .Or(Parse.Ref(() => List))
                .Or(ClausewitzLiteral);

        private static readonly Parser<IEnumerable<IClausewitzValue>> ClausewitzElements =
            ClausewitzValue.DelimitedBy(Space.Token());

        public static readonly Parser<IClausewitzValue> List =
            from bs in BlockStart.Token()
            from values in ClausewitzElements
            from be in BlockEnd.Token()
            select new ClausewitzList(values);
    }
}