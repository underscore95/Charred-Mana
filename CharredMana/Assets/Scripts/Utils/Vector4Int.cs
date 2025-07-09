
using UnityEngine;

public struct Vector4Int
{
    public int x;
    public int y;
    public int z;
    public int w;

    public Vector4Int(int x, int y, int z, int w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public Vector4Int(Vector2Int xy, Vector2Int zw)
    {
        this.x = xy.x;
        this.y = xy.y;
        this.z = zw.x;
        this.w = zw.y;
    }
}