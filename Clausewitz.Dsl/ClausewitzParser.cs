using System;
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

        static readonly Parser<char> ClausewitzChar = Parse.AnyChar.Except(Parse.Char('"').Or(Parse.Char('#')));
        
        private static readonly Parser<ClausewitzLiteral> QuoteSymbol =
            from qs in DoubleQuote
            from symbol in ClausewitzChar.Many().Text()
            from qe in DoubleQuote
            select new ClausewitzLiteral(symbol, LiteralType.Symbol);

        private static readonly Parser<ClausewitzLiteral> NonSymbol =
            from symbol in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit.Or(Parse.Char('_')))
            select new ClausewitzLiteral(symbol, LiteralType.Symbol);

        private static readonly Parser<ClausewitzLiteral> Symbol = QuoteSymbol.Or(NonSymbol);

        public static readonly Parser<OperatorType> Operator =
            Parse.String("<>").Return(OperatorType.InEqual)
                .Or(Parse.String("<=").Return(OperatorType.LessThanOrEqual))
                .Or(Parse.String(">=").Return(OperatorType.GreaterThanOrEqual))
                .Or(Parse.String("<").Return(OperatorType.LessThan))
                .Or(Parse.String(">").Return(OperatorType.GreaterThan))
                .Or(Parse.String("=").Return(OperatorType.Equal));

        public static readonly Parser<string> Comment =
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

        // public static readonly Parser<ClausewitzLiteral> Date =
        //     from qs in DoubleQuote
        //     from year in Parse.Number
        //     from aDot in Parse.Char('.')
        //     from month in Parse.Number
        //     from bDot in Parse.Char('.')
        //     from day in Parse.Number
        //     from qe in DoubleQuote
        //     select new ClausewitzLiteral($"{year}-{month}-{day}", LiteralType.Date);

        public static readonly Parser<ClausewitzLiteral> ClausewitzLiteral =
            Percent
                // .Or(Date)
                .Or(Real)
                .Or(Integer)
                .Or(Symbol)
                .Token().CommitToken();

        public static readonly Parser<ClausewitzAssignment> ClausewitzAssignment =
            from name in Symbol
            from op in Operator.Token()
            from value in
                Parse.Ref(() => Map)
                    .Or(Parse.Ref(() => List))
                    // .Or(Date)
                    .Or(Percent)
                    .Or(Real)
                    .Or(Integer)
                    .Or(Symbol)
                    .Token().CommitToken()
            select new ClausewitzAssignment(name.Value, value);

        public static readonly Parser<IEnumerable<ClausewitzAssignment>> ClausewitzMembers =
            ClausewitzAssignment.DelimitedBy(Space.Token().CommitToken());

        public static readonly Parser<IClausewitzValue> Map =
            from bs in BlockStart.Token().CommitToken()
            from members in ClausewitzMembers.Optional()
            from be in BlockEnd.Token().CommitToken()
            select new ClausewitzMap(members.IsDefined ? members.Get() : null);

        private static readonly Parser<IClausewitzValue> ClausewitzValue =
            Parse.Ref(() => Map)
                .Or(Parse.Ref(() => List))
                .Or(ClausewitzLiteral);

        private static readonly Parser<IEnumerable<IClausewitzValue>> ClausewitzElements =
            ClausewitzValue.DelimitedBy(Space.Token().CommitToken());

        public static readonly Parser<IClausewitzValue> List =
            from bs in BlockStart.Token().CommitToken()
            from elements in ClausewitzElements.Optional()
            from be in BlockEnd.Token().CommitToken()
            select new ClausewitzList(elements.IsDefined ? elements.Get() : null);

        public static readonly Parser<IClausewitzValue> ClausewitzDoc =
            from values in ClausewitzMembers.Token().CommitToken()
            select new ClausewitzMap(values);

        public static Parser<T> CommitToken<T>(this Parser<T> parser)
        {
            if (parser == null)
                throw new ArgumentNullException(nameof(parser));
            return Comment.Many().SelectMany(leading => parser,
                (leading, item) => new
                {
                    leading, item
                }).SelectMany(_param1 => Comment.Many(), (_param1, trailing) => _param1.item);
        }

        public static ClausewitzMap ParseClausewitz(string toParse)
        {
            return (ClausewitzMap) ClausewitzDoc.Parse(toParse);
        }
    }
}