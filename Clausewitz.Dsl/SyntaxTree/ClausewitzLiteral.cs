namespace Clausewitz.Dsl.SyntaxTree
{
    public class ClausewitzLiteral: IClausewitzValue
    {
        /// <summary>
        /// A string representation of the value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The type the value is
        /// </summary>
        public LiteralType ValueType { get; set; }
        
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
        /// Returns Value cast as the appropriate type.
        /// </summary>
        /// <returns></returns>
        public object Get()
        {
            switch (ValueType)
            {
                case LiteralType.Symbol:
                    return Value;
                
                case LiteralType.Integer:
                    return Value;

                case LiteralType.Percent:
                    return Value;
                
                case LiteralType.Real:
                    return Value;

                case LiteralType.Date:
                    return Value;

                default:
                    return null;
            }
        }
    }
}