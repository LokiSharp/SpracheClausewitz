using System;
using Sprache;

namespace Clausewitz.Dsl
{
    public class Item { }
    
    public class Map : Item { }
    
    public class Pair : Item { }
    
    public class Array : Item { }
    
    public class Symbol : Item { }

    public static class ClausewitzParser
    {
        public static readonly Parser<int> INT =
            from op in Parse.Char('-').Optional()
            from num in Parse.Number
            select int.Parse(num) * (op.IsDefined ? -1 : 1);
        
        public static readonly Parser<double> PCT =
            from op in Parse.Char('-').Optional()
            from num in Parse.Number
            from pct  in Parse.Char('%')
            select double.Parse(num) / 100 * (op.IsDefined ? -1 : 1);
        
        public static readonly Parser<double> REAL =
            from op in Parse.Char('-').Optional()
            from n in Parse.Number
            from dot in Parse.Char('.')
            from f in Parse.Number
            select double.Parse($"{n}.{f}") * (op.IsDefined ? -1 : 1);
    }
}