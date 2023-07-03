using NeuralNetwork;

Func<double, double> function = (x) => 1 / (1 + Math.Exp(-x));
Func<double, double> derivative = (x) => x * (1 - x);

if (args.Length == 0)
{
    throw new ArgumentNullException();
}
else if (args[0] == "numbers")
{
    var nn = new DigitRecognition(function, derivative, new[] { 784, 16, 16, 10 });

    nn.Train(10, 25, 0.001);
    nn.Test();
}
else if (args[0] == "titanic")
{
    var nn = new TitanicDetector(function, derivative, new[] { 8, 32, 2 });

    nn.Train(50, 3, 0.005);
    nn.Test();
}
