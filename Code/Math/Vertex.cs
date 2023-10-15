namespace Vheos.Tools.Trid;

public readonly struct Vertex : IEquatable<Vertex>
{
    // Fields
    public readonly TridVectorInt Position;

    // Constructors
    public Vertex(TridVectorInt position)
        => Position = position;

    // Static
    public static Vertex Zero = new(TridVectorInt.Zero);
    public static Vertex At(TridVector position)
        => new(position.Rounded);

    // Methods
    public Vertex NearVertex(VertexDirection direction)
        => new(Position + direction.ToVector());
    public Edge NearEdge(VertexDirection direction)
        => new(this, direction);
    public Triangle NearTriangle(TriangleDirection direction)
        => new(this, direction);
    public IEnumerable<Vertex> NearVertices
    {
        get
        {
            foreach (var direction in EnumCache.VertexDirectionsAndVectors.Keys)
                yield return NearVertex(direction);
        }
    }
    public IEnumerable<Edge> NearEdges
    {
        get
        {
            foreach (var direction in EnumCache.VertexDirectionsAndVectors.Keys)
                yield return NearEdge(direction);
        }
    }
    public IEnumerable<Triangle> NearTriangles
    {
        get
        {
            foreach (var direction in EnumCache.TriangleDirectionsAndVectors.Keys)
                yield return NearTriangle(direction);
        }
    }

    // IEquatable
    public static bool operator ==(Vertex t, Vertex a)
        => t.Equals(a);
    public static bool operator !=(Vertex t, Vertex a)
        => !t.Equals(a);
    public bool Equals(Vertex a)
        => Position == a.Position;
    public override bool Equals(object a)
        => a is not null && a is TridVectorInt vertex && Equals(vertex);
    public override int GetHashCode()
        => Position.GetHashCode();
}