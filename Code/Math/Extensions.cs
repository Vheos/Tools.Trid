namespace Vheos.Tools.Trid;

using static Const;
using static Helpers.Math.Const;

public static class Extensions
{
    public static tvf EuclidToTrid(this f2 @this)
        => new((@this.X + @this.Y / FloatSqrt3, -@this.Y / FloatSqrt3 * 2f).Mul(UnitLength));
    public static tvf EuclidToTrid(this i2 @this)
        => @this.ToFloat2().EuclidToTrid();
}