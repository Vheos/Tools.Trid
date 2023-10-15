using Vheos.Helpers.Math;
using Vheos.Helpers.New;

namespace Vheos.Tools.Trid
{
    public readonly struct TridVector
    {
        public readonly float X;
        public readonly float Y;
        public float Z
            => -X - Y;

        public TridVector(float x, float y)
        {
            X = x;
            Y = y;
        }

        #region Static
        private const float Sqrt3 = 1.73205080757f;

        public static TridVector Zero = new(0f, 0f);
        public static TridVector FromXY(float x, float y)
            => new(x, y);
        public static TridVector FromYZ(float y, float z)
            => new(-y - z, y);
        public static TridVector FromZX(float z, float x)
            => new(x, -x - z);
        public static TridVector From(TriangleAxis axis, float a)
             => axis switch
             {
                 TriangleAxis.X => FromXY(a, -a / 2),
                 TriangleAxis.Y => FromYZ(a, -a / 2),
                 TriangleAxis.Z => FromZX(a, -a / 2),
                 _ => default,
             };
        public static TridVector From(VertexAxis axis, float a, float b)
            => axis switch
            {
                VertexAxis.XY => FromXY(a, b),
                VertexAxis.YZ => FromYZ(a, b),
                VertexAxis.ZX => FromZX(a, b),
                _ => default,
            };

        public static (float X, float Y) ToEuclid(float tridX, float tridY)
        {
            float halfY = tridY / 2f;
            return (tridX + halfY, Sqrt3 * halfY);
        }
        public static (float X, float Y) FromEuclid(float euclidX, float euclidY)
        {
            float radiusY = euclidY / Sqrt3;
            return (euclidX - radiusY, 2f * radiusY);
        }
        #endregion

        #region Math
        public float Length
            => (X.Abs() + Y.Abs() + Z.Abs()) / 2f;
        public TridVector Normalized
            => this / Length;
        public TridVectorInt Rounded
        {
            get
            {
                int roundX = X.Round();
                int roundY = Y.Round();
                float diffX = X - roundX;
                float diffY = Y - roundY;

                return diffX.Abs() >= diffY.Abs()
                    ? (new(roundX + (diffX + diffY / 2f).Round(), roundY))
                    : (new(roundX, roundY + (diffX / 2f + diffY).Round()));
            }
        }
        public float Comp(Axis axis)
            => axis switch
            {
                Axis.X => X,
                Axis.Y => Y,
                Axis.Z => Z,
                _ => default,
            };

        // Spatial
        public TridVector OffsetTo(TridVectorInt a)
            => a - this;
        public TridVector OffsetTo(TridVector a)
            => a - this;
        public TridVector DirectionTo(TridVectorInt a)
            => OffsetTo(a).Normalized;
        public TridVector DirectionTo(TridVector a)
            => OffsetTo(a).Normalized;
        public TridVectorInt TriangleVectorTo(TridVectorInt a)
            => OffsetTo(a).NearTriangleVector;
        public TridVectorInt TriangleVectorTo(TridVector a)
            => OffsetTo(a).NearTriangleVector;
        public TridVectorInt VertexVectorTo(TridVectorInt a)
            => OffsetTo(a).NearVertexVector;
        public TridVectorInt VertexVectorTo(TridVector a)
            => OffsetTo(a).NearVertexVector;
        public float Distance(TridVectorInt a)
            => OffsetTo(a).Length;
        public float Distance(TridVector a)
            => OffsetTo(a).Length;
        public TridVector Rotate(int rotations = 1)
            => rotations.PosMod(6) switch
            {
                1 => new(-Y, -Z),
                2 => new(+Z, +X),
                3 => new(-X, -Y),
                4 => new(+Y, +Z),
                5 => new(-Z, -X),
                _ => new(+X, +Y),
            };
        public TridVector RotateAround(TridVector around, int rotations = 1)
            => (this - around).Rotate(rotations) + around;

        internal TridVectorInt NearVertexVector
        {
            get
            {
                Axis maxAxis = MaxAxis;
                VertexDirection direction = (VertexDirection)(maxAxis | MinAxis);
                TridVectorInt vector = TridVectorInt.DirectionVector(direction);
                return vector.MaxAxis == maxAxis ? vector : -vector;
            }
        }
        internal TridVectorInt NearTriangleVector
        {
            get
            {
                Axis axis = AbsMaxAxis;
                TriangleDirection direction = (TriangleDirection)axis;
                if (Comp(axis) < 0f)
                    direction |= TriangleDirection.Neg;
                return TridVectorInt.DirectionVector(direction);
            }
        }
        internal Axis MaxAxis
            => NewHelpers.MaxAxis(X, Y, Z);
        internal Axis MinAxis
            => NewHelpers.MinAxis(X, Y, Z);
        internal Axis AbsMaxAxis
            => NewHelpers.MaxAxis(X.Abs(), Y.Abs(), Z.Abs());
        #endregion

        #region Operators
        // -
        public static TridVector operator -(TridVector a)
            => new(-a.X, -a.Y);
        // VectorFloat <> VectorInt
        public static TridVector operator +(TridVector a, TridVectorInt b)
            => new(a.X + b.X, a.Y + b.Y);
        public static TridVector operator -(TridVector a, TridVectorInt b)
            => new(a.X - b.X, a.Y - b.Y);
        public static TridVector operator *(TridVector a, TridVectorInt b)
            => new(a.X * b.X, a.Y * b.Y);
        public static TridVector operator /(TridVector a, TridVectorInt b)
            => new(a.X / b.X, a.Y / b.Y);
        public static TridVector operator %(TridVector a, TridVectorInt b)
            => new(a.X % b.X, a.Y % b.Y);
        // VectorFloat <> Int
        public static TridVector operator +(TridVector a, int b)
            => new(a.X + b, a.Y + b);
        public static TridVector operator -(TridVector a, int b)
            => new(a.X - b, a.Y - b);
        public static TridVector operator *(TridVector a, int b)
            => new(a.X * b, a.Y * b);
        public static TridVector operator /(TridVector a, int b)
            => new(a.X / b, a.Y / b);
        public static TridVector operator %(TridVector a, int b)
            => new(a.X % b, a.Y % b);
        // Int <> VectorFloat
        public static TridVector operator +(int a, TridVector b)
            => new(b.X + a, b.Y + a);
        public static TridVector operator -(int a, TridVector b)
            => new(b.X - a, b.Y - a);
        public static TridVector operator *(int a, TridVector b)
            => new(b.X * a, b.Y * a);
        public static TridVector operator /(int a, TridVector b)
            => new(b.X / a, b.Y / a);
        public static TridVector operator %(int a, TridVector b)
            => new(b.X % a, b.Y % a);
        // VectorFloat <> VectorFloat
        public static TridVector operator +(TridVector a, TridVector b)
            => new(a.X + b.X, a.Y + b.Y);
        public static TridVector operator -(TridVector a, TridVector b)
            => new(a.X - b.X, a.Y - b.Y);
        public static TridVector operator *(TridVector a, TridVector b)
            => new(a.X * b.X, a.Y * b.Y);
        public static TridVector operator /(TridVector a, TridVector b)
            => new(a.X / b.X, a.Y / b.Y);
        public static TridVector operator %(TridVector a, TridVector b)
            => new(a.X % b.X, a.Y % b.Y);
        // VectorFloat <> Float
        public static TridVector operator +(TridVector a, float b)
            => new(a.X + b, a.Y + b);
        public static TridVector operator -(TridVector a, float b)
            => new(a.X - b, a.Y - b);
        public static TridVector operator *(TridVector a, float b)
            => new(a.X * b, a.Y * b);
        public static TridVector operator /(TridVector a, float b)
            => new(a.X / b, a.Y / b);
        public static TridVector operator %(TridVector a, float b)
            => new(a.X % b, a.Y % b);
        // Float <> VectorFloat
        public static TridVector operator +(float a, TridVector b)
            => new(b.X + a, b.Y + a);
        public static TridVector operator -(float a, TridVector b)
            => new(b.X - a, b.Y - a);
        public static TridVector operator *(float a, TridVector b)
            => new(b.X * a, b.Y * a);
        public static TridVector operator /(float a, TridVector b)
            => new(b.X / a, b.Y / a);
        public static TridVector operator %(float a, TridVector b)
            => new(b.X % a, b.Y % a);
        #endregion

        #region Casts
        public override string ToString()
            => $"(X: {X:F2},  Y: {Y:F2},  Z: {Z:F2})";
        public static explicit operator TridVectorInt(TridVector t)
            => new((int)t.X, (int)t.Y);
        public TridVectorInt Int
           => (TridVectorInt)this;
        #endregion
    }
}