namespace Vheos.Tools.Trid;

public static class Extensions_Linq
{
    public static TridVectorInt Sum(this IEnumerable<TridVectorInt> t)
    {
        TridVectorInt sum = TridVectorInt.Zero;
        foreach (var vector in t)
            sum += vector;
        return sum;
    }
}