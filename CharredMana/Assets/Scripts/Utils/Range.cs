
using System;
using UnityEngine;
using UnityEngine.Assertions;

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

    public FloatRange(float begin, float end)
    {
        Begin = begin; 
        End = end;
    }

    public readonly bool Contains(float v)
    {
        return v >= Begin && v <= End;
    }

    public readonly void AssertValid(float beginMinimum = float.MinValue)
    {
        Assert.IsTrue(Begin >= beginMinimum);
        Assert.IsTrue(End >= Begin);
    }

    public readonly float Lerp(float t)
    {
        return Mathf.Lerp(Begin, End, t);
    }
}