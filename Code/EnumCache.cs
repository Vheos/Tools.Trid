using System.Collections.Generic;
using Vheos.Helpers.New;

namespace Vheos.Tools.Trid
{
    public static class EnumCache
    {
        internal const TriangleAxis TriangleAxisXYZ = (TriangleAxis)(Axis.X | Axis.Y | Axis.Z);
        internal const VertexAxis VertexAxisXYZ = (VertexAxis)(Axis.X | Axis.Y | Axis.Z);

        public static readonly IReadOnlyList<TriangleAxis> TriangleAxes = new[]
        {
            TriangleAxis.X,
            TriangleAxis.Y,
            TriangleAxis.Z,
        };
        public static readonly IReadOnlyBictionary<TriangleDirection, TridVectorInt> TriangleDirectionsAndVectors = new Bictionary<TriangleDirection, TridVectorInt>
        {
            { TriangleDirection.X, TridVectorInt.DirectionVector(TriangleDirection.X) },
            { TriangleDirection.Y, TridVectorInt.DirectionVector(TriangleDirection.Y) },
            { TriangleDirection.Z, TridVectorInt.DirectionVector(TriangleDirection.Z) },
            { TriangleDirection.NegX, TridVectorInt.DirectionVector(TriangleDirection.NegX) },
            { TriangleDirection.NegY, TridVectorInt.DirectionVector(TriangleDirection.NegY) },
            { TriangleDirection.NegZ, TridVectorInt.DirectionVector(TriangleDirection.NegZ) },
        };
        public static readonly IReadOnlyList<VertexAxis> VertexAxes = new[]
        {
            VertexAxis.XY,
            VertexAxis.YZ,
            VertexAxis.ZX,
        };
        public static readonly IReadOnlyBictionary<VertexDirection, TridVectorInt> VertexDirectionsAndVectors = new Bictionary<VertexDirection, TridVectorInt>
        {
            { VertexDirection.XY, TridVectorInt.DirectionVector(VertexDirection.XY) },
            { VertexDirection.YZ, TridVectorInt.DirectionVector(VertexDirection.YZ) },
            { VertexDirection.ZX, TridVectorInt.DirectionVector(VertexDirection.ZX) },
            { VertexDirection.NegXY, TridVectorInt.DirectionVector(VertexDirection.NegXY) },
            { VertexDirection.NegYZ, TridVectorInt.DirectionVector(VertexDirection.NegYZ) },
            { VertexDirection.NegZX, TridVectorInt.DirectionVector(VertexDirection.NegZX) },
        };
    }
}