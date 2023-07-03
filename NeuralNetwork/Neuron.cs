namespace NeuralNetwork;

public class Neuron
{
    public double Value { get; set; }
    public double Bias { get; set; }
    public double[] Weights { get; set; }

    public Neuron(int nextSize)
    {
        Weights = new double[nextSize];
    }
}
