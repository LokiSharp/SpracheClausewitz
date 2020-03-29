using System;

namespace Clausewitz.Dsl.SyntaxTree
{
    public class ClausewitzLiteral : IClausewitzValue
    {
        public ClausewitzLiteral(LiteralType type)
        {
            ValueType = type;
        }

        public ClausewitzLiteral(string value, LiteralType type)
        {
            Value = value;
            ValueType = type;
        }

        /// <summary>
        ///     A string representation of the value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///     The type the value is
        /// </summary>
        public LiteralType ValueType { get; set; }

        public IClausewitzValue this[string key]
        {
            get => throw new NotImplementedException("Cannot access ClausewitzLiteral by string.");
            set => throw new NotImplementedException("Cannot access ClausewitzLiteral by string.");
        }

        public IClausewitzValue this[int i]
        {
            get => throw new NotImplementedException("Cannot access ClausewitzLiteral by int.");
            set => throw new NotImplementedException("Cannot access ClausewitzLiteral by int.");
        }

        /// <summary>
        ///     Returns Value cast as the appropriate type.
        /// </summary>
        /// <returns></returns>
        public object Get()
        {
            switch (ValueType)
            {
                case LiteralType.Symbol:
                    return Value;

                case LiteralType.Integer:
                    return Convert.ToInt32(Value);

                case LiteralType.Percent:
                    return Convert.ToDouble(Value) / 100;

                case LiteralType.Real:
                    return Convert.ToDouble(Value);

                case LiteralType.Date:
                    return Convert.ToDateTime(Value);

                default:
                    return null;
            }
        }

        public override string ToString()
        {
            return Get().ToString();
        }
    }
}