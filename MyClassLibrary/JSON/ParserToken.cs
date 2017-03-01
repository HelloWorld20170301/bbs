namespace MyClassLibrary.JSON
{
    internal enum ParserToken
    {
        None = System.Char.MaxValue + 1,
        Number,
        True,
        False,
        Null,
        CharSeq,
        Char,
        Text,
        Object,
        ObjectPrime,
        Pair,
        PairRest,
        Array,
        ArrayPrime,
        Value,
        ValueRest,
        String,
        End,
        Epsilon
    }
}
