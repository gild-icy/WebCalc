using WebCalc.Operations;

namespace WebCalc
{
    public class Calculator : ICalculator
    {
        private readonly Dictionary<string, IOperation> _operations;
        private readonly IExpressionParser _parser;

        public Calculator(IExpressionParser parser)
        {
            _operations = new Dictionary<string, IOperation>
            {
                { "+", new Addition() },
                { "-", new Subtraction() },
                { "*", new Multiplication() },
                { "/", new Division() }
            };

            _parser = parser;
        }

        private double PerformOperation(string operation, double operand1, double operand2)
        {
            return _operations[operation].Calculate(operand1, operand2);
        }

        private void ProcessOperator(string operation, Stack<string> operatorStack, Queue<string> outputQueue)
        {
            while (operatorStack.Count > 0 && _operations.ContainsKey(operatorStack.Peek()) &&
                   (_operations[operation] is Addition || _operations[operation] is Subtraction) &&
                   (_operations[operatorStack.Peek()] is Multiplication || _operations[operatorStack.Peek()] is Division) ||
                   ((_operations[operation] is Multiplication || _operations[operation] is Division) &&
                    (operatorStack.Count > 0 && (_operations[operatorStack.Peek()] is Multiplication || _operations[operatorStack.Peek()] is Division))))
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }

            operatorStack.Push(operation);
        }

        private void FinalizeOutputQueue(Stack<string> operatorStack, Queue<string> outputQueue)
        {
            while (operatorStack.Count > 0)
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }
        }

        private double EvaluateRPN(Queue<string> outputQueue)
        {
            var calculationStack = new Stack<double>();

            while (outputQueue.Count > 0)
            {
                var token = outputQueue.Dequeue();

                if (_operations.ContainsKey(token))
                {
                    var operand2 = calculationStack.Pop();
                    var operand1 = calculationStack.Pop();
                    var result = PerformOperation(token, operand1, operand2);
                    calculationStack.Push(result);
                }
                else
                {
                    calculationStack.Push(int.Parse(token));
                }
            }

            return calculationStack.Pop();
        }

        // Shunting-yard algorithm
        public double Calculate(string expression)
        {
            var parts = _parser.Parse(expression);
            var outputQueue = new Queue<string>();
            var operatorStack = new Stack<string>();

            foreach (var part in parts)
            {
                if (_operations.ContainsKey(part))
                {
                    ProcessOperator(part, operatorStack, outputQueue);
                }
                else
                {
                    outputQueue.Enqueue(part);
                }
            }

            FinalizeOutputQueue(operatorStack, outputQueue);

            return EvaluateRPN(outputQueue);
        }
    }
}