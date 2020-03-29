using System;

namespace Clausewitz.Dsl.SyntaxTree
{
    public class ClausewitzAssignment : IClausewitzValue
    {
        public ClausewitzAssignment()
        {
            AssignmentPair = new Tuple<string, IClausewitzValue, OperatorType>(null, null, new OperatorType());
        }

        public ClausewitzAssignment(string name, IClausewitzValue value, OperatorType operatorType)
        {
            AssignmentPair = new Tuple<string, IClausewitzValue, OperatorType>(name, value, operatorType);
        }

        private Tuple<string, IClausewitzValue, OperatorType> AssignmentPair { get; }
        public string Key => AssignmentPair.Item1;
        public IClausewitzValue Value => AssignmentPair.Item2;
        public OperatorType OperatorType => AssignmentPair.Item3;

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