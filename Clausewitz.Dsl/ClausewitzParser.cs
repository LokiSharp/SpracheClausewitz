using System;
using System.Linq;
using Sprache;

namespace Clausewitz.Dsl
{
    public static class ClausewitzParser
    {
        private static readonly Parser<char> DoubleQuote = Parse.Char('"');
        private static readonly Parser<string> Space = Parse.WhiteSpace.Except(Parse.LineEnd).Many().Text();
        private static readonly Parser<char> BlockStart = Parse.Char('{');
        private static readonly Parser<char> BlockEnd = Parse.Char('}');

        private static readonly Parser<string> Symbol =
            Parse.Identifier(Parse.Letter, Parse.LetterOrDigit.Or(Parse.Char('_')));

        public static readonly Parser<string> Operator =
            Parse.String("<>")
                .Or(Parse.String("<="))
                .Or(Parse.String(">="))
                .Or(Parse.String("<"))
                .Or(Parse.String(">"))
                .Or(Parse.String("=")).Text();

        public static readonly Parser<string> Comment =
            from op in Parse.String("#")
            from space in Space.Optional()
            from comment in Parse.CharExcept('\n').Many().Text()
            select comment;

        public static readonly Parser<object> Integer =
            from op in Parse.Char('-').Optional()
            from num in Parse.Number
            select (object) (long.Parse(num) * (op.IsDefined ? -1 : 1));

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

        public static readonly Parser<object> Assignment =
            from name in Symbol
            from s1 in Space.Optional()
            from op in Operator
            from s2 in Space.Optional()
            from value in List.Or(Date).Or(Percent).Or(Real).Or(Integer).Or(Symbol)
            select new Tuple<object, object, object>(name, op, value);

        public static readonly Parser<object> List =
            from bs in BlockStart
            from leading in Space.Optional()
            from values in Parse.Ref(() => List.Or(Assignment).Or(Date).Or(Percent).Or(Real).Or(Integer).Or(Symbol))
                .DelimitedBy(Space)
            from trailing in Space.Optional()
            from be in BlockEnd
            select values.ToList();
    }
}