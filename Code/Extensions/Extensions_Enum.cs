using Vheos.Helpers.Math;

namespace Vheos.Tools.Trid
{
    public static class Extensions_Enum
    {
        public static TridVectorInt ToVector(this TriangleDirection t)
            => EnumCache.TriangleDirectionsAndVectors.GetValueOr(t, default);
        public static TridVectorInt ToVector(this VertexDirection t)
            => EnumCache.VertexDirectionsAndVectors.GetValueOr(t, default);
        public static TriangleDirection ToTriangleDirection(this TridVectorInt t)
            => EnumCache.TriangleDirectionsAndVectors.GetKeyOr(t, default);
        public static VertexDirection ToVertexDirection(this TridVectorInt t)
            => EnumCache.VertexDirectionsAndVectors.GetKeyOr(t, default);

        public static TriangleDirection Rotate(this TriangleDirection t, int rotations = 1)
            => t.ToVector().Rotate(rotations).ToTriangleDirection();
        public static VertexDirection Rotate(this VertexDirection t, int rotations = 1)
            => t.ToVector().Rotate(rotations).ToVertexDirection();

        public static TriangleDirection ToDirection(this TriangleAxis t, bool isNegative = false)
            => isNegative
            ? (TriangleDirection)t | TriangleDirection.Neg
            : (TriangleDirection)t;
        public static TriangleAxis ToAxis(this TriangleDirection t)
            => (TriangleAxis)(t & ~TriangleDirection.Neg);
        public static VertexDirection ToDirection(this VertexAxis t, bool isNegative = false)
            => isNegative
            ? (VertexDirection)t
            : (VertexDirection)t | VertexDirection.Neg;
        public static VertexAxis ToAxis(this VertexDirection t)
            => (VertexAxis)(t & ~VertexDirection.Neg);
        public static int Sign(this TriangleDirection t)
            => (t & TriangleDirection.Neg) == 0 ? +1 : -1;
        public static int Sign(this VertexDirection t)
            => (t & VertexDirection.Neg) == 0 ? +1 : -1;

        internal static (TridVectorInt CCW, TridVectorInt CW) Split(this TridVectorInt t)
            => (t + t.Rotate(+1), t + t.Rotate(-1));
        public static (TridVectorInt CCW, TridVectorInt CW) NearTriangleVectors(this VertexDirection t)
            => t.ToVector().Split();
        public static (TridVectorInt CCW, TridVectorInt CW) NearVertexVectors(this TriangleDirection t)
        {
            var (CCW, CW) = t.ToVector().Split();
            return (CCW / 3, CW / 3);
        }
        public static TriangleAxis ComplementaryVertexAxis(this VertexAxis t)
            => EnumCache.TriangleAxisXYZ & (TriangleAxis)~t;
        public static VertexAxis ComplementaryTriangleAxis(this TriangleAxis t)
            => EnumCache.VertexAxisXYZ & (VertexAxis)~t;

        public static TriangleAxis Rotate(this TriangleAxis t, int rotations = 1)
        {
            rotations = rotations.PosMod(3);
            return t switch
            {
                TriangleAxis.X when rotations == 1 => TriangleAxis.Y,
                TriangleAxis.Y when rotations == 1 => TriangleAxis.Z,
                TriangleAxis.Z when rotations == 1 => TriangleAxis.X,
                TriangleAxis.X when rotations == 2 => TriangleAxis.Z,
                TriangleAxis.Y when rotations == 2 => TriangleAxis.X,
                TriangleAxis.Z when rotations == 2 => TriangleAxis.Y,
                _ => t,
            };
        }
        public static VertexAxis Rotate(this VertexAxis t, int rotations = 1)
        {
            rotations = rotations.PosMod(3);
            return t switch
            {
                VertexAxis.XY when rotations == 1 => VertexAxis.YZ,
                VertexAxis.YZ when rotations == 1 => VertexAxis.ZX,
                VertexAxis.ZX when rotations == 1 => VertexAxis.XY,
                VertexAxis.XY when rotations == 2 => VertexAxis.ZX,
                VertexAxis.YZ when rotations == 2 => VertexAxis.XY,
                VertexAxis.ZX when rotations == 2 => VertexAxis.YZ,
                _ => t,
            };
        }
    }
}