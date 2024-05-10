using System.Text.RegularExpressions;

namespace WebCalc
{
    public class ExpressionParser : IExpressionParser
    {
        private static readonly Regex ExpressionPattern = new Regex(@"(\d+)([-+*/])?");

        public List<string> Parse(string expression)
        {
            if (!Regex.IsMatch(expression, @"^[0-9+\-*/\(\) ]+$"))
            {
                throw new ArgumentException("Invalid input expression.");
            }
            
            var matches = ExpressionPattern.Matches(expression);
            var expressionParts = new List<string>();

            foreach (Match match in matches)
            {
                if (int.TryParse(match.Groups[1].Value, out var operand))
                {
                    expressionParts.Add(operand.ToString());
                }

                if (match.Groups[2].Success)
                {
                    expressionParts.Add(match.Groups[2].Value);
                }
            }

            return expressionParts;
        }
    }
}