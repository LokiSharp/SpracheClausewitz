using System.Collections.Generic;

namespace Clausewitz.Dsl.SyntaxTree
{
    public class ClausewitzList : IClausewitzValue
    {
        public ClausewitzList()
        {
            Elements = new List<IClausewitzValue>();
        }

        public ClausewitzList(IEnumerable<IClausewitzValue> elements)
        {
            Elements = new List<IClausewitzValue>();
            if (elements != null)
                foreach (var e in elements)
                    Elements.Add(e);
        }

        /// <summary>
        ///     All the Clausewitz values
        /// </summary>
        public List<IClausewitzValue> Elements { get; set; }
    }
}