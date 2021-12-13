using System;

public delegate System.Numerics.Vector2 Fv2Vector2(System.Numerics.Vector2 v2);

public static class Fv2
{
    public static System.Numerics.Vector2 TestVectorF(System.Numerics.Vector2 v2)
    {
        var r = new Random();
        return new System.Numerics.Vector2((float)r.NextDouble(), (float)r.NextDouble());
    }
}

