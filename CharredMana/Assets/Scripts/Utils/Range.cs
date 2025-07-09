
using System;

[Serializable]

public struct IntRange
{
    public int Begin;
    public int End;

    public IntRange(int begin, int end)
    {
        Begin = begin;
        End = end;
    }

    public void OffsetRangeBy(int v)
    {
        Begin += v;
        End += v;
    }

    public readonly bool Contains(int v)
    {
        return v >= Begin && v <= End;
    }
}

[Serializable]
public struct FloatRange
{
    public float Begin;
    public float End;

    public readonly bool Contains(float v)
    {
        return v >= Begin && v <= End;
    }
}