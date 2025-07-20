
using System;
using UnityEngine;
using UnityEngine.Assertions;

public enum MathOp
{
    Add,
    Multiply
}

public class MathOps
{
    public static void Test()
    {
        Assert.IsTrue(ApproxEqual(Merge(MathOp.Add, 1, 1), 2));
        Assert.IsTrue(ApproxEqual(Merge(MathOp.Add, 1, 2), 3));
        Assert.IsTrue(ApproxEqual(Merge(MathOp.Add, 1, -2), -1));
        Assert.IsTrue(ApproxEqual(Merge(MathOp.Add, -1, -2), -3));
        Assert.IsTrue(ApproxEqual(Merge(MathOp.Add, -1, 2), 1));
        Assert.IsTrue(ApproxEqual(Merge(MathOp.Multiply, 1, 1), 1));
        Assert.IsTrue(ApproxEqual(Merge(MathOp.Multiply, 1, 1.1f), 1.1f));
        Assert.IsTrue(ApproxEqual(Merge(MathOp.Multiply, 1.1f, 1.1f), 1.2f));
        Assert.IsTrue(ApproxEqual(Merge(MathOp.Multiply, 1.1f, -1.1f), 1));
        Assert.IsTrue(ApproxEqual(Merge(MathOp.Multiply, 1.1f, -1.5f), -1.4f));
        Assert.IsTrue(ApproxEqual(Merge(MathOp.Multiply, -1.1f, -1.1f), -1.2f));
        Assert.IsTrue(ApproxEqual(Merge(MathOp.Multiply, -1.1f, 1.0f), -1.1f));
        Assert.IsTrue(ApproxEqual(Merge(MathOp.Multiply, -1.1f, -1.0f), -1.1f));
        Assert.IsTrue(ApproxEqual(Eval(MathOp.Multiply, 10, -1.1f), 9));
    }

    // Return the value y for op such that Eval(op, x, y) approx equals x
    public static float GetDefaultValue(MathOp op)
    {
        switch (op)
        {
            case MathOp.Add:
                return 0;
            case MathOp.Multiply:
                return 1;
            default:
                Assert.IsTrue(false);
                return 0;
        }
    }

    private static bool ApproxEqual(float a, float b)
    {
        if (Mathf.Approximately(a, b)) return true;
        Debug.LogError($"Actual: {a} Expected: {b}");
        return false;
    }

    public static string ToString(MathOp op, float val, int numDecimals = 1, bool trimTrailingZeros = false)
    {
        string Format(float v)
        {
            string format = "F" + numDecimals;
            string result = v.ToString(format);
            if (trimTrailingZeros)
            {
                result = result.TrimEnd('0').TrimEnd('.');
            }
            return result;
        }

        switch (op)
        {
            case MathOp.Add:
                return (val < 0 ? "-" : "+") + Format(Math.Abs(val));
            case MathOp.Multiply:
                string percent = Format((Math.Abs(val) - 1) * 100) + "%";
                return (val > 0 ? "+" : "-") + percent;
            default:
                Assert.IsTrue(false);
                return "";
        }
    }

    public static float Eval(MathOp operation, float one, float two)
    {
        switch (operation)
        {
            case MathOp.Add:
                return one + two;
            case MathOp.Multiply:
                Assert.IsTrue(two <= -1 || two >= 1);
                return two >= 1 ? one * two : one - one * -(two + 1);
            default: throw new System.NotImplementedException($"Eval not implemented for {operation}");
        }
    }

    // Merge two of the same operation and return the value used so that the operation can be done once.
    // Examples:
    // Operation: +, one: 1, two: 1, result: 2
    // Operation: +, one: 3, two: 2, result: 5
    // Multiplications are additively merged
    // Operation: *, one: 1.1, two: 1.1, result: 1.2
    // Operation: *, one: 3, two: 2, result: 4
    // Operation: *, one: 3, two: 3, result: 5
    public static float Merge(MathOp operation, float one, float two)
    {
        switch (operation)
        {
            case MathOp.Add: return one + two;
            case MathOp.Multiply:
                Assert.IsTrue(one <= -1 || one >= 1);
                Assert.IsTrue(two <= -1 || two >= 1);
                float p = (one >= 0 ? one - 1 : one + 1) + (two > 0 ? two - 1 : two + 1);
                return p >= 0 ? p + 1 : p - 1;
            default: throw new System.NotImplementedException($"Eval not implemented for {operation}");
        }
    }

    // Lower numbers should be applied first
    public static float StatOpApplyOrder(MathOp operation)
    {
        return operation switch
        {
            MathOp.Add => 1,
            MathOp.Multiply => 2,
            _ => throw new System.NotImplementedException($"Eval not implemented for {operation}"),
        };
    }
}
