namespace Vheos.Tools.Trid;

using static Const;

public readonly struct Vertex(i2 xy) : IEquatable<Vertex>
{
    public readonly i2 XY = xy;

    #region Constructors

    public Vertex(tvi vector)
        : this(vector.XY) { }
    public Vertex(i x, i y)
        : this((x, y)) { }

    #endregion

    #region Shapes

    public static Vertex Near(tvf position)
        => new(position.RoundHexToMultiple(UnitLength));
    public IEnumerable<Vertex> AdjacentVertices
    {
        get
        {
            foreach (var direction in Directions)
                yield return new(Position + direction * UnitLength);
        }
    }
    public IEnumerable<Edge> AdjacentEdges
    {
        get
        {
            foreach (var direction in Directions)
                yield return new(Position + direction * UnitLength / 2);
        }
    }
    public IEnumerable<Triangle> AdjacentTriangles
    {
        get
        {
            foreach (var direction in Directions)
                yield return new(Position + (direction.Rotate30(3) * UnitLength * 2f / 3f).Round());
        }
    }

    public bool IsAdjacentTo(Vertex vertex)
        => Position.DistanceTo(vertex.Position) == UnitLength;
    public bool IsAdjacentTo(Edge edge)
        => Position.DistanceTo(edge.Position) == UnitLength / 2;
    public bool IsAdjacentTo(Triangle triangle)
        => Position.DistanceTo(triangle.Position) == UnitLength * 2 / 3;

    public bool IsAligned
        => Position.Shape == TridShape.Vertex;

    #endregion

    #region Math

    public tvi Position
        => new(XY);

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

    public static Vertex operator +(Vertex a, tvi b)
        => new(a.Position + b);
    public static Vertex operator -(Vertex a, tvi b)
        => new(a.Position - b);

    #endregion

    #region IEquatable

    public static bool operator ==(Vertex t, Vertex a)
        => t.Equals(a);
    public static bool operator !=(Vertex t, Vertex a)
        => !t.Equals(a);
    public bool Equals(Vertex a)
        => XY == a.XY;
    public override bool Equals(object a)
        => a is not null && a is tvi vertex && Equals(vertex);
    public override int GetHashCode()
        => XY.GetHashCode();

    #endregion
}

/*
 * 
 * Vertex
 * - vertex in direction X
 * - edge in direction X
 * - triangle in direction X
 * - adjacent vertices
 * - adjacent edges
 * - adjacent triangles
 * 
 * Edge
 * - triangle in direction X
 * - adjacent triangles
 * 
 * Triangle
 * - triangle in direction X
 * - adjacent triangles
 */