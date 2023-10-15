namespace Vheos.Tools.Trid;

public readonly struct TridVectorInt : IEquatable<TridVectorInt>
{
    public readonly int X;
    public readonly int Y;
    public int Z
        => -X - Y;

    public TridVectorInt(int x, int y)
    {
        X = x;
        Y = y;
    }

    #region Static
    public static TridVectorInt Zero = new(0, 0);
    public static TridVectorInt FromXY(int x, int y)
        => new(x, y);
    public static TridVectorInt FromYZ(int y, int z)
        => new(-y - z, y);
    public static TridVectorInt FromZX(int z, int x)
        => new(x, -x - z);
    public static TridVectorInt From(TriangleAxis triangleAxis, int a)
         => triangleAxis switch
         {
             TriangleAxis.X => FromXY(a, -a / 2),
             TriangleAxis.Y => FromYZ(a, -a / 2),
             TriangleAxis.Z => FromZX(a, -a / 2),
             _ => default,
         };
    public static TridVectorInt From(VertexAxis vertexAxis, int a, int b)
        => vertexAxis switch
        {
            VertexAxis.XY => FromXY(a, b),
            VertexAxis.YZ => FromYZ(a, b),
            VertexAxis.ZX => FromZX(a, b),
            _ => default,
        };
    internal static TridVectorInt DirectionVector(TriangleDirection direction)
        => direction.Sign() * From(direction.ToAxis(), +2);
    internal static TridVectorInt DirectionVector(VertexDirection direction)
        => direction.Sign() * From(direction.ToAxis(), +1, -1);
    #endregion

    #region Math
    public int Length
        => (X.Abs() + Y.Abs() + Z.Abs()) / 2;
    public TridVector Normalized
        => this / Length;
    public int Comp(Axis axis)
        => axis switch
        {
            Axis.X => X,
            Axis.Y => Y,
            Axis.Z => Z,
            _ => default,
        };

    // Spatial
    public TridVectorInt OffsetTo(TridVectorInt a)
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
    public int Distance(TridVectorInt a)
        => OffsetTo(a).Length;
    public float Distance(TridVector a)
        => OffsetTo(a).Length;
    public TridVectorInt Rotate(int rotations = 1)
        => rotations.PosMod(6) switch
        {
            1 => new(-Y, -Z),
            2 => new(+Z, +X),
            3 => new(-X, -Y),
            4 => new(+Y, +Z),
            5 => new(-Z, -X),
            _ => new(+X, +Y),
        };
    public TridVectorInt RotateAround(TridVectorInt around, int rotations = 1)
        => (this - around).Rotate(rotations) + around;

    internal TridVectorInt NearVertexVector
    {
        get
        {
            Axis maxAxis = MaxAxis;
            VertexDirection direction = (VertexDirection)(maxAxis | MinAxis);
            TridVectorInt vector = DirectionVector(direction);
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
            return DirectionVector(direction);
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
    public static TridVectorInt operator -(TridVectorInt a)
        => new(-a.X, -a.Y);
    // VectorInt <> VectorInt
    public static TridVectorInt operator +(TridVectorInt a, TridVectorInt b)
        => new(a.X + b.X, a.Y + b.Y);
    public static TridVectorInt operator -(TridVectorInt a, TridVectorInt b)
        => new(a.X - b.X, a.Y - b.Y);
    public static TridVectorInt operator *(TridVectorInt a, TridVectorInt b)
        => new(a.X * b.X, a.Y * b.Y);
    public static TridVectorInt operator /(TridVectorInt a, TridVectorInt b)
        => new(a.X / b.X, a.Y / b.Y);
    public static TridVectorInt operator %(TridVectorInt a, TridVectorInt b)
        => new(a.X % b.X, a.Y % b.Y);
    // VectorInt <> Int
    public static TridVectorInt operator +(TridVectorInt a, int b)
        => new(a.X + b, a.Y + b);
    public static TridVectorInt operator -(TridVectorInt a, int b)
        => new(a.X - b, a.Y - b);
    public static TridVectorInt operator *(TridVectorInt a, int b)
        => new(a.X * b, a.Y * b);
    public static TridVectorInt operator /(TridVectorInt a, int b)
        => new(a.X / b, a.Y / b);
    public static TridVectorInt operator %(TridVectorInt a, int b)
        => new(a.X % b, a.Y % b);
    // Int <> VectorInt
    public static TridVectorInt operator +(int a, TridVectorInt b)
        => new(b.X + a, b.Y + a);
    public static TridVectorInt operator -(int a, TridVectorInt b)
        => new(b.X - a, b.Y - a);
    public static TridVectorInt operator *(int a, TridVectorInt b)
        => new(b.X * a, b.Y * a);
    public static TridVectorInt operator /(int a, TridVectorInt b)
        => new(b.X / a, b.Y / a);
    public static TridVectorInt operator %(int a, TridVectorInt b)
        => new(b.X % a, b.Y % a);
    // VectorInt <> VectorFloat
    public static TridVector operator +(TridVectorInt a, TridVector b)
        => new(a.X + b.X, a.Y + b.Y);
    public static TridVector operator -(TridVectorInt a, TridVector b)
        => new(a.X - b.X, a.Y - b.Y);
    public static TridVector operator *(TridVectorInt a, TridVector b)
        => new(a.X * b.X, a.Y * b.Y);
    public static TridVector operator /(TridVectorInt a, TridVector b)
        => new(a.X / b.X, a.Y / b.Y);
    public static TridVector operator %(TridVectorInt a, TridVector b)
        => new(a.X % b.X, a.Y % b.Y);
    // VectorInt <> Float
    public static TridVector operator +(TridVectorInt a, float b)
        => new(a.X + b, a.Y + b);
    public static TridVector operator -(TridVectorInt a, float b)
        => new(a.X - b, a.Y - b);
    public static TridVector operator *(TridVectorInt a, float b)
        => new(a.X * b, a.Y * b);
    public static TridVector operator /(TridVectorInt a, float b)
        => new(a.X / b, a.Y / b);
    public static TridVector operator %(TridVectorInt a, float b)
        => new(a.X % b, a.Y % b);
    // Float <> VectorInt
    public static TridVector operator +(float a, TridVectorInt b)
        => new(b.X + a, b.Y + a);
    public static TridVector operator -(float a, TridVectorInt b)
        => new(b.X - a, b.Y - a);
    public static TridVector operator *(float a, TridVectorInt b)
        => new(b.X * a, b.Y * a);
    public static TridVector operator /(float a, TridVectorInt b)
        => new(b.X / a, b.Y / a);
    public static TridVector operator %(float a, TridVectorInt b)
        => new(b.X % a, b.Y % a);
    #endregion

    #region Casts
    public override string ToString()
        => $"(X: {X},  Y: {Y},  Z: {Z})";
    public static implicit operator TridVector(TridVectorInt t)
        => new(t.X, t.Y);
    public TridVector Float
        => (TridVector)this;
    #endregion

    #region IEquatable
    public static bool operator ==(TridVectorInt t, TridVectorInt a)
        => t.Equals(a);
    public static bool operator !=(TridVectorInt t, TridVectorInt a)
        => !t.Equals(a);
    public bool Equals(TridVectorInt a)
        => X == a.X && Y == a.Y;
    public override bool Equals(object a)
        => a is not null && a is TridVectorInt vertex && Equals(vertex);
    public override int GetHashCode()
        => X.GetHashCode() ^ (Y.GetHashCode() << 2);
    #endregion
}