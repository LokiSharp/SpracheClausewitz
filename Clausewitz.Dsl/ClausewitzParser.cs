using System;
using System.Linq;
using Sprache;

namespace Clausewitz.Dsl
{
    public static class ClausewitzParser
    {
        private static readonly Parser<char> DoubleQuote = Parse.Char('"');
        private static readonly Parser<string> Space = Parse.WhiteSpace.Many().Text();
        private static readonly Parser<char> BlockStart = Parse.Char('{');
        private static readonly Parser<char> BlockEnd = Parse.Char('}');
        private static readonly Parser<string> LineBack = Parse.Char('\n').Or(Parse.Char('\t')).Many().Text();

        private static readonly Parser<string> Symbol =
            Parse.Identifier(Parse.Letter, Parse.LetterOrDigit.Or(Parse.Char('_')));

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

        public static readonly Parser<object> Integer =
            from op in Parse.Char('-').Optional()
            from num in Parse.Number
            select (object) (int.Parse(num) * (op.IsDefined ? -1 : 1));

        public static readonly Parser<object> Percent =
            from op in Parse.Char('-').Optional()
            from num in Parse.Number
            from pct in Parse.Char('%')
            select (object) (double.Parse(num) / 100 * (op.IsDefined ? -1 : 1));

        public static readonly Parser<object> Real =
            from op in Parse.Char('-').Optional()
            from n in Parse.Number
            from dot in Parse.Char('.')
            from f in Parse.Number
            select (object) (double.Parse($"{n}.{f}") * (op.IsDefined ? -1 : 1));

        public static readonly Parser<object> Date =
            from y in Parse.Number
            from yDot in Parse.Char('.')
            from m in Parse.Number
            from dDot in Parse.Char('.')
            from d in Parse.Number
            select (object) DateTime.Parse($"{y}-{m}-{d}");

        public static readonly Parser<Tuple<string, OperatorType, object>> Assignment =
            from name in Symbol
            from lb in LineBack.Optional()
            from leading in Space.Optional()
            from op in Operator
            from trailing in Space.Optional()
            from value in Map.Or(List).Or(Date).Or(Percent).Or(Real).Or(Integer).Or(Symbol)
            from lb2 in LineBack.Optional()
            select new Tuple<string, OperatorType, object>(name, op, value);

        public static readonly Parser<object> Map =
            from bs in BlockStart
            from lb1 in LineBack.Optional()
            from leading in Space.Optional()
            from values in Assignment.Many()
            from trailing in Space.Optional()
            from lb2 in LineBack.Optional()
            from be in BlockEnd
            select values.ToDictionary(x=> x.Item1, x => x.Item3);

        public static readonly Parser<object> List =
            from bs in BlockStart
            from lb1 in LineBack.Optional()
            from leading in Space.Optional()
            from values in Parse.Ref(() => List.Or(Map).Or(Date).Or(Percent).Or(Real).Or(Integer).Or(Symbol))
                .DelimitedBy(Space)
            from trailing in Space.Optional()
            from lb2 in LineBack.Optional()
            from be in BlockEnd
            select values.ToList();
    }
}