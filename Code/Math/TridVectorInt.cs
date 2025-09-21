namespace Vheos.Tools.Trid;

using Vheos.Helpers.Math;
using static Const;
using static Helpers.Math.Const;

public readonly struct TridVectorInt(i2 xy) : IEquatable<tvi>
{
    public readonly i2 XY = xy;
    public readonly static tvi Zero = new(0, 0);

    #region Constructors

    public TridVectorInt(i x, i y)
        : this((x, y)) { }

    #endregion

    #region Math

    public i Length
    => XYZ.Abs().AddComps() / 2;
    public tvf Normalized
    {
        get
        {
            var length = Length;
            return length > 0 ? this / length : default;
        }
    }
    public TridShape Shape
        => XY.ModEuclid(UnitLength) switch
        {
            (0, 0) => TridShape.Vertex,
            (3, 0) or (3, 3) or (0, 3) => TridShape.Edge,
            (2, 2) or (4, 4) => TridShape.Triangle,
            _ => TridShape.None
        };
    public i X
        => XY.X;
    public i Y
        => XY.Y;
    public i Z
        => -XY.X - XY.Y;
    public i3 XYZ
        => XY.Append(Z);
    public f2 Euclid
       => Float.Euclid;

    public tvi OffsetTo(tvi a)
        => a - this;
    public tvi OffsetFrom(tvi a)
        => this - a;
    public i DistanceTo(tvi a)
        => OffsetTo(a).Length;
    public i DistanceFrom(tvi a)
        => OffsetFrom(a).Length;
    public tvf DirectionTo(tvi a)
        => OffsetTo(a).Normalized;
    public tvf DirectionFrom(tvi a)
        => OffsetFrom(a).Normalized;
    public i Dot(tvi a)
        => XYZ.Dot(a.XYZ);

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
        => XYZ.ToFloat3().Dot(a.XYZ);

    public f Angle
        => Float.Angle;
    public f SignedAngle
        => Float.SignedAngle;
    public f ClockwiseAngle
        => Float.ClockwiseAngle;
    public tvi Rotate60(i rotations = 1)
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
        => Float.Rotate30(rotations);

    public f AngleTo(tvf a)
        => Float.AngleTo(a);
    public f SignedAngleTo(tvf a)
        => Float.SignedAngleTo(a);
    public f ClockwiseAngleTo(tvf a)
        => Float.ClockwiseAngleTo(a);

    public f AngleTo(tvi a)
        => Float.AngleTo(a.Float);
    public f SignedAngleTo(tvi a)
        => Float.SignedAngleTo(a.Float);
    public f ClockwiseAngleTo(tvi a)
        => Float.ClockwiseAngleTo(a.Float);

    #endregion

    #region Operators

    public static tvi operator -(tvi a)
        => new(a.XY.Neg());
    public static tvi operator +(tvi a)
        => new(a.XY);

    public static tvi operator +(tvi a, tvi b)
        => new(a.XY.Add(b.XY));
    public static tvi operator -(tvi a, tvi b)
        => new(a.XY.Sub(b.XY));
    public static tvi operator *(tvi a, tvi b)
        => new(a.XY.Mul(b.XY));
    public static tvi operator /(tvi a, tvi b)
        => new(a.XY.Div(b.XY));
    public static tvi operator %(tvi a, tvi b)
        => new(a.XY.Rem(b.XY));

    public static tvi operator +(tvi a, i b)
        => new(a.XY.Add(b));
    public static tvi operator -(tvi a, i b)
        => new(a.XY.Sub(b));
    public static tvi operator *(tvi a, i b)
        => new(a.XY.Mul(b));
    public static tvi operator /(tvi a, i b)
        => new(a.XY.Div(b));
    public static tvi operator %(tvi a, i b)
        => new(a.XY.Rem(b));

    public static tvf operator +(tvi a, tvf b)
        => new(a.XY.ToFloat2().Add(b.XY));
    public static tvf operator -(tvi a, tvf b)
        => new(a.XY.ToFloat2().Sub(b.XY));
    public static tvf operator *(tvi a, tvf b)
        => new(a.XY.ToFloat2().Mul(b.XY));
    public static tvf operator /(tvi a, tvf b)
        => new(a.XY.ToFloat2().Div(b.XY));
    public static tvf operator %(tvi a, tvf b)
        => new(a.XY.ToFloat2().Rem(b.XY));

    public static tvf operator +(tvi a, f b)
        => new(a.XY.ToFloat2().Add(b));
    public static tvf operator -(tvi a, f b)
        => new(a.XY.ToFloat2().Sub(b));
    public static tvf operator *(tvi a, f b)
        => new(a.XY.ToFloat2().Mul(b));
    public static tvf operator /(tvi a, f b)
        => new(a.XY.ToFloat2().Div(b));
    public static tvf operator %(tvi a, f b)
        => new(a.XY.ToFloat2().Rem(b));

    #endregion

    #region Conversions

    public override string ToString()
        => $"(X: {X},  Y: {Y},  Z: {Z})";
    public static implicit operator tvf(tvi t)
        => new(t.XY.ToFloat2());
    public tvf Float
        => (tvf)this;

    #endregion

    #region IEquatable

    public static bool operator ==(tvi t, tvi a)
        => t.Equals(a);
    public static bool operator !=(tvi t, tvi a)
        => !t.Equals(a);
    public bool Equals(tvi a)
        => XY == a.XY;
    public override bool Equals(object a)
        => a is not null && a is tvi vertex && Equals(vertex);
    public override i GetHashCode()
        => XY.GetHashCode();

    #endregion
}