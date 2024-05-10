namespace WebCalc;

public interface IExpressionParser
{
    List<string> Parse(string expression);
}