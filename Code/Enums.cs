using Vheos.Helpers.Math;

namespace Vheos.Tools.Trid
{
    public enum TriangleAxis
    {
        X = Axis.X,
        Y = Axis.Y,
        Z = Axis.Z,
    }

    public enum VertexAxis
    {
        XY = Axis.X | Axis.Y,
        YZ = Axis.Y | Axis.Z,
        ZX = Axis.X | Axis.Z,
    }

    [System.Flags]
    public enum TriangleDirection
    {
        X = TriangleAxis.X,
        Y = TriangleAxis.Y,
        Z = TriangleAxis.Z,
        Neg = Z << 1,
        NegX = X | Neg,
        NegY = Y | Neg,
        NegZ = Z | Neg,
    }

    [System.Flags]
    public enum VertexDirection
    {
        XY = VertexAxis.XY,
        YZ = VertexAxis.YZ,
        ZX = VertexAxis.ZX,
        Neg = TriangleDirection.Neg,
        NegXY = XY | Neg,
        NegYZ = YZ | Neg,
        NegZX = ZX | Neg,
    }
}