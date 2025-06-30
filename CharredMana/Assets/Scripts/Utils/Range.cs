
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
}

[Serializable]
public struct FloatRange
{
    public float Begin;
    public float End;
}