namespace Clausewitz.Dsl.SyntaxTree
{
    public interface IClausewitzValue
    {
        IClausewitzValue this[string key] { get; set; }

        IClausewitzValue this[int i] { get; set; }
    }
}