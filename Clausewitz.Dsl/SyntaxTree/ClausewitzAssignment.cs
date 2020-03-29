using System;

namespace Clausewitz.Dsl.SyntaxTree
{
    public class ClausewitzAssignment : IClausewitzValue
    {
        public ClausewitzAssignment()
        {
            Name = null;
            Value = null;
            OperatorType = new OperatorType();
        }

        public ClausewitzAssignment(string name, IClausewitzValue value, OperatorType operatorType)
        {
            Name = name;
            Value = value;
            OperatorType = operatorType;
        }

        public string Name { get; set; }
        public IClausewitzValue Value { get; set; }
        public OperatorType OperatorType { get; set; }

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