using System.Collections.Generic;

namespace Clausewitz.Dsl.SyntaxTree
{
    public class ClausewitzMap : IClausewitzValue
    {
        public ClausewitzMap()
        {
            Pairs = new Dictionary<string, IClausewitzValue>();
        }

        public ClausewitzMap(IEnumerable<KeyValuePair<string, IClausewitzValue>> pairs)
        {
            Pairs = new Dictionary<string, IClausewitzValue>();
            if (pairs != null)
                foreach (var p in pairs)
                    Pairs.Add(p.Key, p.Value);
        }

        /// <summary>
        ///     All the Clausewitz pair objects
        /// </summary>
        public Dictionary<string, IClausewitzValue> Pairs { get; set; }
    }
}