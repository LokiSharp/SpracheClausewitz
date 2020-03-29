using System;
using System.Collections.Generic;

namespace Clausewitz.Dsl.SyntaxTree
{
    public class ClausewitzAssignment : IClausewitzValue
    {
        public ClausewitzAssignment()
        {
            Pair = new KeyValuePair<string, IClausewitzValue>(null, null);
        }

        public ClausewitzAssignment(string name, IClausewitzValue value)
        {
            Pair = new KeyValuePair<string, IClausewitzValue>(name, value);
        }

        private KeyValuePair<string, IClausewitzValue> Pair { get; }
        public string Key => Pair.Key;
        public IClausewitzValue Value => Pair.Value;

        public IClausewitzValue this[string key]
        {
            get => throw new NotImplementedException("Cannot access ClausewitzAssignment by string.");
            set => throw new NotImplementedException("Cannot access ClausewitzAssignment by string.");
        }

        public IClausewitzValue this[int i]
        {
            get => throw new NotImplementedException("Cannot access ClausewitzAssignment by int.");
            set => throw new NotImplementedException("Cannot access ClausewitzAssignment by int.");
        }
    }
}