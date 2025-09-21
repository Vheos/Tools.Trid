namespace Vheos.Tools.Trid;

using static Const;

public readonly struct Edge(i2 xy) : IEquatable<Edge>
{
    public readonly i2 XY = xy;

    #region Constructors

    public Edge(Vertex a, Vertex b)
        : this(a.XY, b.XY) { }
    public Edge(tvi a, tvi b)
        : this((a + b) / 2) { }
    public Edge(i2 a, i2 b)
        : this(new tvi(a), new tvi(b)) { }

    public Edge(tvi vector)
        : this(vector.XY) { }
    public Edge(i x, i y)
        : this(new tvi(x, y)) { }

    #endregion

    #region Shapes

    public static Edge Near(tvf position)
    {
        tvi vertexA = Vertex.Near(position).Position;
        tvi directionAB = vertexA.DirectionTo(position).RoundHex();
        if (directionAB == default)
            directionAB = DirectionX;

        return new(vertexA + directionAB * UnitLength / 2);
    }
    public static bool IsValid(Vertex a, Vertex b)
        => a.IsAdjacentTo(b);
    public static bool IsValidAndAligned(Vertex a, Vertex b)
        => a.IsAligned && b.IsAligned && IsValid(a, b);

    public Vertex VertexPos
        => new(Position + OffsetToVertexPos);
    public Vertex VertexNeg
        => new(Position - OffsetToVertexPos);

    public IEnumerable<Vertex> Vertices
    {
        get
        {
            tvi offset = OffsetToVertexPos;
            yield return new(Position + offset);
            yield return new(Position - offset);
        }
    }
    public IEnumerable<Edge> AdjacentEdges
    {
        get
        {
            tvi offset = OffsetToVertexPos;
            yield return new(Position + offset.Rotate60(1));
            yield return new(Position + offset.Rotate60(2));
            yield return new(Position + offset.Rotate60(4));
            yield return new(Position + offset.Rotate60(5));
        }
    }
    public IEnumerable<Triangle> AdjacentTriangles
    {
        get
        {
            tvi offset = OffsetToVertexPos * 2 / 3;
            yield return new(Position + offset.Rotate30(3).Round());
            yield return new(Position + offset.Rotate30(9).Round());
        }
    }
    
    public bool IsAligned
        => Position.Shape == TridShape.Edge;
    public Axis Axis
        => X % UnitLength == 0 ? Axis.Z
        : Y % UnitLength == 0 ? Axis.X
        : Axis.Y;
    public bool IsAdjacentTo(Vertex vertex)
        => vertex.IsAdjacentTo(this);
    public bool IsAdjacentTo(Edge edge)
        => Position.DistanceTo(edge.Position) == UnitLength / 2;
    public bool IsAdjacentTo(Triangle triangle)
        => Position.DistanceTo(triangle.Position) == UnitLength / 3;

    #endregion

    #region Math

    public tvi Position
        => new(XY);
    public tvi OffsetToVertexPos
    {
        get
        {
            i2 offset = XY.ModEuclid(UnitLength);
            i2 signs = offset.X == offset.Y ? (-1, +1) : (+1, -1);
            return new(offset.Mul(signs));
        }
    }

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

    public static Edge operator +(Edge a, tvi b)
        => new(a.Position + b);
    public static Edge operator -(Edge a, tvi b)
        => new(a.Position - b);

    #endregion

    #region IEquatable

    public static bool operator ==(Edge t, Edge a)
        => t.Equals(a);
    public static bool operator !=(Edge t, Edge a)
        => !t.Equals(a);
    public bool Equals(Edge a)
        => XY == a.XY;
    public override bool Equals(object a)
        => a is not null && a is Edge vertex && Equals(vertex);
    public override int GetHashCode()
        => XY.GetHashCode();

    #endregion
}