namespace Vheos.Tools.Trid;

using static Const;

public readonly struct Triangle(i2 xy) : IEquatable<Triangle>
{
    public readonly i2 XY = xy;

    #region Constructors

    public Triangle(Vertex a, Vertex b, Vertex c)
        : this(a.XY, b.XY, c.XY) { }
    public Triangle(tvi a, tvi b, tvi c)
        : this(a.XY, b.XY, c.XY) { }
    public Triangle(i2 a, i2 b, i2 c)
        : this(a.Add(b).Add(c).Div(3)) { }

    public Triangle(tvi vector)
        : this(vector.XY) { }
    public Triangle(i x, i y)
    : this(new tvi(x, y)) { }

    #endregion

    #region Shapes

    public static Triangle Near(tvf position)
    {
        tvi rounded = (position - UnitLength / 2).RoundToMultiple(UnitLength) + UnitLength / 2;
        tvi offset = new(rounded.OffsetTo(position).Z.IsNegative().Choose(-1, +1).ToInt2());
        return new(rounded - offset);
    }
    public static bool IsValid(Vertex a, Vertex b, Vertex c)
        => a.IsAdjacentTo(b) && a.IsAdjacentTo(c) && b.IsAdjacentTo(c);
    public static bool IsValid(Edge a, Edge b, Edge c)
        => a.IsAdjacentTo(b) && a.IsAdjacentTo(c) && b.IsAdjacentTo(c);
    public static bool IsValidAndAligned(Vertex a, Vertex b, Vertex c)
        => a.IsAligned && b.IsAligned && c.IsAligned && IsValid(a, b, c);
    public static bool IsValidAndAligned(Edge a, Edge b, Edge c)
        => a.IsAligned && b.IsAligned && c.IsAligned && IsValid(a, b, c);

    public Vertex VertexX
        => new(Position + OffsetToVertexY.Rotate60(-2));
    public Vertex VertexY
        => new(Position + OffsetToVertexY);
    public Vertex VertexZ
        => new(Position + OffsetToVertexY.Rotate60(+2));
    public Vertex Vertex(Axis axis)
        => axis switch
        {
            Axis.X => VertexX,
            Axis.Y => VertexY,
            Axis.Z => VertexZ,
            _ => default,
        };

    public Edge EdgeX
        => new(Position + OffsetToEdgeY.Rotate60(-2));
    public Edge EdgeY
        => new(Position + OffsetToEdgeY);
    public Edge EdgeZ
        => new(Position + OffsetToEdgeY.Rotate60(+2));
    public Edge Edge(Axis axis)
    => axis switch
    {
        Axis.X => EdgeX,
        Axis.Y => EdgeY,
        Axis.Z => EdgeZ,
        _ => default,
    };

    public IEnumerable<Vertex> Vertices
    {
        get
        {
            var offset = OffsetToVertexY;
            yield return new(Position + offset.Rotate60(-2));
            yield return new(Position + offset);
            yield return new(Position + offset.Rotate60(+2));
        }
    }
    public IEnumerable<Edge> Edges
    {
        get
        {
            var offset = OffsetToEdgeY;
            yield return new(Position + offset.Rotate60(-2));
            yield return new(Position + offset);
            yield return new(Position + offset.Rotate60(+2));
        }
    }
    public IEnumerable<Triangle> AdjacentTriangles
    {
        get
        {
            var offset = OffsetToEdgeY * 2;
            yield return new(Position + offset.Rotate60(-2));
            yield return new(Position + offset);
            yield return new(Position + offset.Rotate60(+2));
        }
    }

    public bool IsAligned
        => Position.Shape == TridShape.Vertex;
    public bool IsFlatTop
        => X.ModEuclid(UnitLength) == 2;
    public bool IsPointyTop
        => X.ModEuclid(UnitLength) == 4;
    public bool IsAdjacentTo(Vertex vertex)
        => vertex.IsAdjacentTo(this);
    public bool IsAdjacentTo(Edge edge)
        => edge.IsAdjacentTo(this);
    public bool IsAdjacentTo(Triangle triangle)
        => Position.DistanceTo(triangle.Position) == UnitLength * 2 / 3;

    #endregion

    #region Math

    public tvi Position
        => new(XY);
    public tvi OffsetToVertexY
        => new(XY.ModRound(UnitLength).Neg());
    public tvi OffsetToEdgeY
        => new(XY.ModRound(UnitLength).Div(2));

    public i X
        => Position.X;
    public i Y
        => Position.Y;
    public i Z
        => Position.Z;
    public i3 XYZ
        => Position.XYZ;
    public f2 Euclid
        => Position.Euclid;

    public tvi OffsetTo(tvi a)
        => Position.OffsetTo(a);
    public tvi OffsetFrom(tvi a)
        => Position.OffsetFrom(a);
    public i DistanceTo(tvi a)
        => Position.DistanceTo(a);
    public i DistanceFrom(tvi a)
        => Position.DistanceFrom(a);
    public tvf DirectionTo(tvi a)
        => Position.DirectionTo(a);
    public tvf DirectionFrom(tvi a)
        => Position.DirectionFrom(a);

    public tvf OffsetTo(tvf a)
        => Position.OffsetTo(a);
    public tvf OffsetFrom(tvf a)
        => Position.OffsetFrom(a);
    public f DistanceTo(tvf a)
        => Position.DistanceTo(a);
    public f DistanceFrom(tvf a)
        => Position.DistanceFrom(a);
    public tvf DirectionTo(tvf a)
        => Position.DirectionTo(a);
    public tvf DirectionFrom(tvf a)
        => Position.DirectionFrom(a);

    #endregion

    #region Operators

    public static Triangle operator +(Triangle a, tvi b)
        => new(a.Position + b);
    public static Triangle operator -(Triangle a, tvi b)
        => new(a.Position - b);

    #endregion

    #region IEquatable

    public static bool operator ==(Triangle t, Triangle a)
        => t.Equals(a);
    public static bool operator !=(Triangle t, Triangle a)
        => !t.Equals(a);
    public bool Equals(Triangle a)
        => XY == a.XY;
    public override bool Equals(object a)
        => a is not null && a is Triangle vertex && Equals(vertex);
    public override int GetHashCode()
        => XY.GetHashCode();

    #endregion
}