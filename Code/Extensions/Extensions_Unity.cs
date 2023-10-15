#if UNITY_5_3_OR_NEWER
using UnityEngine;

namespace Vheos.Tools.Trid
{
    public static class Extensions_Unity
    {
        public static Vector2 AsVector2(this TridVector t)
            => new(t.X, t.Y);
        public static Vector3 AsVector3(this TridVector t)
            => new(t.X, t.Y, t.Z);
        public static TridVector AsTridVector(this Vector2 t)
            => new(t.x, t.y);
        public static TridVector AsTridVector(this Vector3 t)
            => new(t.x, t.y);

        public static Vector2 ToEuclid(this TridVector t)
        {
            var (X, Y) = TridVector.ToEuclid(t.X, t.Y);
            return new(X, Y);
        }
        public static TridVector ToTrid(this Vector2 t)
        {
            var (X, Y) = TridVector.FromEuclid(t.x, t.y);
            return new(X, Y);
        }
        public static TridVector ToTrid(this Vector3 t)
        {
            var (X, Y) = TridVector.FromEuclid(t.x, t.y);
            return new(X, Y);
        }

        public static Vector2Int AsVector2Int(this TridVectorInt t)
            => new(t.X, t.Y);
        public static Vector3Int AsVector3Int(this TridVectorInt t)
            => new(t.X, t.Y, t.Z);
        public static TridVectorInt AsTridVectorInt(this Vector2Int t)
            => new(t.x, t.y);
        public static TridVectorInt AsTridVectorInt(this Vector3Int t)
            => new(t.x, t.y);

        public static Vector2 ToEuclid(this TridVectorInt t)
        {
            var (X, Y) = TridVector.ToEuclid(t.X, t.Y);
            return new(X, Y);
        }
        public static TridVector ToTrid(this Vector2Int t)
        {
            var (X, Y) = TridVector.FromEuclid(t.x, t.y);
            return new(X, Y);
        }
        public static TridVector ToTrid(this Vector3Int t)
        {
            var (X, Y) = TridVector.FromEuclid(t.x, t.y);
            return new(X, Y);
        }
    }
}
#endif