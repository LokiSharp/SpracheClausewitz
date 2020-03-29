using System;
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

        public IClausewitzValue this[string key]
        {
            get => throw new NotImplementedException("Cannot access ClausewitzList by string.");
            set => throw new NotImplementedException("Cannot access ClausewitzList by string.");
        }

        /// <summary>
        ///     Makes Elements directly accessable
        /// </summary>
        /// <param name="i">The index of the IClausewitzValue</param>
        /// <returns>The IClausewitzValue at index i</returns>
        public IClausewitzValue this[int i]
        {
            get
            {
                if (0 <= i && i < Elements.Count) return Elements[i];
                throw new IndexOutOfRangeException(i + " is out of range.");
            }
            set
            {
                if (0 <= i && i < Elements.Count) Elements[i] = value;
                else throw new IndexOutOfRangeException(i + " is out of range.");
            }
        }
    }
}