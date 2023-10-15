namespace Vheos.Tools.Trid;

public readonly struct Triangle : IEquatable<Triangle>
{
    // Fields
    public readonly Vertex VertexA;
    public readonly Vertex VertexB;
    public readonly Vertex VertexC;

    // Constructors
    internal Triangle(Vertex vertexA, Vertex vertexB, Vertex vertexC)
    {
        VertexA = vertexA;
        VertexB = vertexB;
        VertexC = vertexC;
    }
    public Triangle(Vertex vertex, TriangleDirection direction)
    {
        VertexA = vertex;
        var (CCW, CW) = direction.NearVertexVectors();
        VertexB = new(vertex.Position + CCW);
        VertexC = new(vertex.Position + CW);
    }

    // Static
    public static Triangle At(TridVector position)
    {
        Vertex vertexA = Vertex.At(position);
        var (CCW, CW) = vertexA.Position.TriangleVectorTo(position).Split();
        Vertex vertexB = new(vertexA.Position + CCW / 3);
        Vertex vertexC = new(vertexA.Position + CW / 3);
        return new(vertexA, vertexB, vertexC);
    }

    // Methods
    public TridVectorInt ID
        => VertexA.Position + VertexB.Position + VertexC.Position;
    public TridVector Position
        => ID / 3f;
    public IEnumerable<Vertex> NearVertices
    {
        get
        {
            yield return VertexA;
            yield return VertexB;
            yield return VertexC;
        }
    }
    public IEnumerable<Edge> NearEdges
    {
        get
        {
            yield return new(VertexA, VertexB);
            yield return new(VertexB, VertexC);
            yield return new(VertexC, VertexA);
        }
    }
    public IEnumerable<Triangle> NearTriangles
    {
        get
        {
            yield return new(VertexA, VertexB, new(VertexA.Position + VertexB.Position - VertexC.Position));
            yield return new(VertexB, VertexC, new(VertexB.Position + VertexC.Position - VertexA.Position));
            yield return new(VertexC, VertexA, new(VertexC.Position + VertexA.Position - VertexB.Position));
        }
    }

    // IEquatable
    public static bool operator ==(Triangle t, Triangle a)
        => t.Equals(a);
    public static bool operator !=(Triangle t, Triangle a)
        => !t.Equals(a);
    public bool Equals(Triangle a)
        => ID == a.ID;
    public override bool Equals(object a)
        => a is not null && a is Triangle vertex && Equals(vertex);
    public override int GetHashCode()
        => ID.GetHashCode();
}