namespace Vheos.Tools.Trid;

public static class Const
{
    internal const MethodImplOptions Inline = MethodImplOptions.AggressiveInlining;

    public  const i UnitLength = 6;

    public static readonly tvi DirectionX = new(+1, 0);
    public static readonly tvi DirectionY = DirectionX.Rotate60(2);
    public static readonly tvi DirectionZ = DirectionY.Rotate60(2);

    public static IEnumerable<tvi> Directions
    {
        get
        {
            yield return DirectionX;
            yield return -DirectionZ;
            yield return DirectionY;
            yield return -DirectionX;
            yield return DirectionZ;
            yield return -DirectionY;
        }
    }
}