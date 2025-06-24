
using System;
using UnityEngine.Assertions;

public enum MathOp
{
    Add,
    Multiply
}

public class MathOps
{
    public static string ToString(MathOp op, float val, int numDecimals = 1)
    {
        string format = "F" + numDecimals;
        switch (op)
        {
            case MathOp.Add:
                return (val < 0 ? "-" : "+") + Math.Abs(val).ToString(format);
            case MathOp.Multiply:
                return "+" + ((val-1) * 100).ToString(format) + "%";
            default:
                Assert.IsTrue(false);
                return "";
        }
    }

    public static float Eval(MathOp operation, float one, float two)
    {
        return operation switch
        {
            MathOp.Add => one + two,
            MathOp.Multiply => one * two,
            _ => throw new System.NotImplementedException($"Eval not implemented for {operation}"),
        };
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
        if (operation == MathOp.Multiply) Assert.IsTrue(one >= 1 && two >= 1, "Merging * ops with value <1 untested.");
        return operation switch
        {
            MathOp.Add => one + two,
            MathOp.Multiply => (one - 1) + (two - 1) + 1,
            _ => throw new System.NotImplementedException($"Eval not implemented for {operation}"),
        };
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
