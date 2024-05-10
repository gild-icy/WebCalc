namespace WebCalc.Operations;

public class Multiplication : IOperation
{
    public double Calculate(double operand1, double operand2)
    {
        return operand1 * operand2;
    }
}