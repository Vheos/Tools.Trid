using System;
using System.Collections.Generic;

namespace Vheos.Tools.Trid
{
    public readonly struct Edge : IEquatable<Edge>
    {
        // Fields
        public readonly Vertex VertexA;
        public readonly Vertex VertexB;

        // Constructors
        internal Edge(Vertex vertexA, Vertex vertexB)
        {
            VertexA = vertexA;
            VertexB = vertexB;
        }
        public Edge(Vertex vertex, VertexDirection direction)
        {
            VertexA = vertex;
            VertexB = vertex.NearVertex(direction);
        }

        // Static
        public static Edge At(TridVector position)
        {
            Vertex vertexA = Vertex.At(position);
            Vertex vertexB = new(vertexA.Position + vertexA.Position.VertexVectorTo(position));
            return new(vertexA, vertexB);
        }

        // Methods
        public TridVectorInt ID
            => VertexA.Position + VertexB.Position;
        public TridVector Position
            => ID / 2f;
        public VertexAxis Axis
            => (VertexB.Position - VertexA.Position).ToVertexDirection().ToAxis();
        public IEnumerable<Vertex> NearVertices
        {
            get
            {
                yield return VertexA;
                yield return VertexB;
            }
        }
        public IEnumerable<Triangle> NearTriangles
        {
            get
            {
                TridVector halfVector = Axis.ComplementaryVertexAxis().ToDirection().ToVector() / 2f;
                yield return new(VertexA, VertexB, new((Position + halfVector).Int));
                yield return new(VertexB, VertexA, new((Position - halfVector).Int));
            }
        }

        // IEquatable
        public static bool operator ==(Edge t, Edge a)
            => t.Equals(a);
        public static bool operator !=(Edge t, Edge a)
            => !t.Equals(a);
        public bool Equals(Edge a)
            => ID == a.ID;
        public override bool Equals(object a)
            => a is not null && a is Edge vertex && Equals(vertex);
        public override int GetHashCode()
            => ID.GetHashCode();
    }
}