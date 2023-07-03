namespace NeuralNetwork;

public class Layer
{
    public int Size { get; set; }
    public Neuron[] Neurons { get; set; }

    public Layer(int size, int weightsCount)
    {
        Size = size;
        Neurons = new Neuron[Size];
        for (int i = 0; i < Neurons.Length; i++)
            Neurons[i] = new Neuron(weightsCount);
    }

    public void FeedForward(Layer anotherLayer, Func<double, double> activation)
    {
        for (int i = 0; i < Size; i++)
        {
            double newValue = Neurons[i].Bias;
            for (int j = 0; j < anotherLayer.Size; j++)
            {
                newValue += anotherLayer.Neurons[j].Value * Neurons[i].Weights[j];
            }
            Neurons[i].Value = activation(newValue);
        }
    }

    public double[] BackPropagate(
        double[] errors,
        Layer anotherLayer,
        Func<double, double> derivative,
        double learningRate
    )
    {
        for (int i = 0; i < Size; i++)
        {
            var delta = errors[i] * derivative(Neurons[i].Value) * learningRate;
            for (int j = 0; j < anotherLayer.Size; j++)
                Neurons[i].Weights[j] -= delta * anotherLayer.Neurons[j].Value;
            Neurons[i].Bias -= delta;
        }

        var nextErrors = new double[anotherLayer.Size];
        for (int i = 0; i < anotherLayer.Size; i++)
            for (int j = 0; j < Size; j++)
                nextErrors[i] += Neurons[j].Weights[i] * errors[j];
        return nextErrors;
    }

    public void UpdateNeurons(double[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
            Neurons[i].Value = inputs[i];
    }
}
