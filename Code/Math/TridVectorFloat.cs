namespace Vheos.Tools.Trid;

using static Const;
using static Helpers.Math.Const;

public readonly struct TridVectorFloat(f2 xy) : IEquatable<tvf>
{
    public readonly f2 XY = xy;
    public readonly static tvf Zero = new(0f, 0f);

    #region Constructors

    public TridVectorFloat(f x, f y)
        : this((x, y)) { }

    #endregion

    #region Math

    public f Length
        => XYZ.Abs().AddComps() / 2f;
    public tvf Normalized
    {
        get
        {
            f length = Length;
            return length > 0f ? this / length : default;
        }
    }
    public tvi RoundHex()
    {
        i2 rounded = XY.Round();
        f2 remainder = XY.Sub(rounded);
        i2 hexRounded = remainder.X.Abs() >= remainder.Y.Abs()
            ? (rounded.X + (remainder.X + remainder.Y / 2f).Round(), rounded.Y)
            : (rounded.X, rounded.Y + (remainder.X / 2f + remainder.Y).Round());
        return new(hexRounded);
    }
    public tvi RoundHexToMultiple(i multiple)
        => (this / multiple).RoundHex() * multiple;
    public tvi Round()
        => new(XY.Round());
    public tvi RoundToMultiple(i multiple)
        => new(XY.RoundToMultiple(multiple));
    public bool IsNear(TridShape shape)
        => RoundHex().Shape == shape;

    public f X
        => XY.X;
    public f Y
        => XY.Y;
    public f Z
        => -XY.X - XY.Y;
    public f3 XYZ
        => XY.Append(Z);
    public f2 Euclid
        => (X + Y / 2f, -Y / 2f * FloatSqrt3).Div(UnitLength);

    public tvf OffsetTo(tvf a)
        => a - this;
    public tvf OffsetFrom(tvf a)
        => this - a;
    public f DistanceTo(tvf a)
        => OffsetTo(a).Length;
    public f DistanceFrom(tvf a)
        => OffsetFrom(a).Length;
    public tvf DirectionTo(tvf a)
        => OffsetTo(a).Normalized;
    public tvf DirectionFrom(tvf a)
        => OffsetFrom(a).Normalized;
    public f Dot(tvf a)
        => XYZ.Dot(a.XYZ);

    public tvf OffsetTo(tvi a)
        => a - this;
    public tvf OffsetFrom(tvi a)
        => this - a;
    public f DistanceTo(tvi a)
        => OffsetTo(a).Length;
    public f DistanceFrom(tvi a)
        => OffsetFrom(a).Length;
    public tvf DirectionTo(tvi a)
        => OffsetTo(a).Normalized;
    public tvf DirectionFrom(tvi a)
        => OffsetFrom(a).Normalized;
    public f Dot(tvi a)
        => XYZ.Dot(a.XYZ.ToFloat3());

    public f Angle
    {
        get
        {
            f length = Length;
            if (length <= 0f)
                return default;

            f dot = 2f * X + Y;
            f angle = 2f - dot / length;
            if (angle > 1f)
            {
                angle -= 1f;
                if (angle < 2f)
                    angle = angle / 2f + 1f;
            }

            return angle;
        }
    }
    public f SignedAngle
        => Y >= 0 ? Angle : -Angle;
    public f ClockwiseAngle
        => Y >= 0 ? Angle : 6f - Angle;
    public tvf Rotate60(i rotations = 1)
        => rotations.ModEuclid(6) switch
        {
            0 => new(+X, +Y),
            1 => new(-Y, -Z),
            2 => new(+Z, +X),
            3 => new(-X, -Y),
            4 => new(+Y, +Z),
            5 => new(-Z, -X),
            _ => default,
        };
    public tvf Rotate30(i rotations = 1)
    {
        tvf r = Rotate60(rotations / 2);
        return rotations.IsEven() ? r : new tvf(r.X - r.Y, r.X + r.Y * 2) / 2f;
    }

    public f AngleTo(tvf a)
        => SignedAngleTo(a).Abs();
    public f SignedAngleTo(tvf a)
        => a.ClockwiseAngle - ClockwiseAngle;
    public f ClockwiseAngleTo(tvf a)
        => SignedAngleTo(a).ModEuclid(6f);

    public f AngleTo(tvi a)
        => AngleTo(a.Float);
    public f SignedAngleTo(tvi a)
        => SignedAngleTo(a.Float);
    public f ClockwiseAngleTo(tvi a)
        => ClockwiseAngleTo(a.Float);

    #endregion

    #region Operators

    public static tvf operator -(tvf a)
        => new(a.XY.Neg());
    public static tvf operator +(tvf a)
        => new(a.XY);

    public static tvf operator +(tvf a, tvf b)
        => new(a.XY.Add(b.XY));
    public static tvf operator -(tvf a, tvf b)
        => new(a.XY.Sub(b.XY));
    public static tvf operator *(tvf a, tvf b)
        => new(a.XY.Mul(b.XY));
    public static tvf operator /(tvf a, tvf b)
        => new(a.XY.Div(b.XY));
    public static tvf operator %(tvf a, tvf b)
        => new(a.XY.Rem(b.XY));

    public static tvf operator +(tvf a, f b)
        => new(a.XY.Add(b));
    public static tvf operator -(tvf a, f b)
        => new(a.XY.Sub(b));
    public static tvf operator *(tvf a, f b)
        => new(a.XY.Mul(b));
    public static tvf operator /(tvf a, f b)
        => new(a.XY.Div(b));
    public static tvf operator %(tvf a, f b)
        => new(a.XY.Rem(b));

    public static tvf operator +(tvf a, tvi b)
        => new(a.XY.Add(b.XY));
    public static tvf operator -(tvf a, tvi b)
        => new(a.XY.Sub(b.XY));
    public static tvf operator *(tvf a, tvi b)
        => new(a.XY.Mul(b.XY));
    public static tvf operator /(tvf a, tvi b)
        => new(a.XY.Div(b.XY));
    public static tvf operator %(tvf a, tvi b)
        => new(a.XY.Rem(b.XY));

    public static tvf operator +(tvf a, i b)
        => new(a.XY.Add(b));
    public static tvf operator -(tvf a, i b)
        => new(a.XY.Sub(b));
    public static tvf operator *(tvf a, i b)
        => new(a.XY.Mul(b));
    public static tvf operator /(tvf a, i b)
        => new(a.XY.Div(b));
    public static tvf operator %(tvf a, i b)
        => new(a.XY.Rem(b));

    #endregion

    #region Conversions

    public override string ToString()
        => $"(X: {X:F2},  Y: {Y:F2},  Z: {Z:F2})";
    public static explicit operator tvi(tvf t)
        => new(t.XY.ToInt2());
    public tvf Int
        => (tvi)this;

    #endregion

    #region IEquatable

    public static bool operator ==(tvf t, tvf a)
        => t.Equals(a);
    public static bool operator !=(tvf t, tvf a)
        => !t.Equals(a);
    public bool Equals(tvf a)
        => XY == a.XY;
    public override bool Equals(object a)
        => a is not null && a is tvi vertex && Equals(vertex);
    public override i GetHashCode()
        => XY.GetHashCode();

    #endregion
}



/*
private i AngleRoundedDown
{
    get
    {
        (f x, f y, f z) = XYZ.Abs();
        return x >= y && x > z ? X < 0f ? 2 : 5
             : y >= z && y > x ? Y > 0f ? 1 : 4
             : z >= x && z > y ? Z < 0f ? 0 : 3
             : default;
    }
}
public b IsClockwiseOf(tvf a, tvf b)
    => (b.X - a.X) * (Y - a.Y) - (b.Y - a.Y) * (X - a.X) > 0f;
*/