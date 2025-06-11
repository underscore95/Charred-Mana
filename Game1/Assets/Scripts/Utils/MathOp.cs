
public enum MathOp
{
    Add,
    Multiply
}

public class MathOps
{
    public static float Eval(MathOp operation, float one, float two)
    {
        return operation switch
        {
            MathOp.Add => one + two,
            MathOp.Multiply => one * two,
            _ => throw new System.NotImplementedException($"Eval not implemented for {operation}"),
        };
    }
}
