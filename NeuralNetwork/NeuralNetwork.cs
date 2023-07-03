using System.Linq;

namespace NeuralNetwork;

public class NeuralNetwork
{
    private Layer Results
    {
        get => _layers[_layers.Length - 1];
    }
    private Layer[] _layers;
    private Func<double, double> _activation;
    private Func<double, double> _derivative;

    public NeuralNetwork(
        Func<double, double> activation,
        Func<double, double> derivative,
        params int[] layers
    )
    {
        _activation = activation;
        _derivative = derivative;
        _layers = new Layer[layers.Length];
        var rnd = new Random();
        _layers[0] = new Layer(layers[0], 0);
        for (int i = 1; i < _layers.Length; i++)
        {
            int weightsCount = 0;
            if (i < _layers.Length)
                weightsCount = layers[i - 1];
            _layers[i] = new Layer(layers[i], weightsCount);
            for (int j = 0; j < layers[i]; j++)
            {
                _layers[i].Neurons[j].Bias = rnd.NextDouble() * 2 - 1;
                for (int k = 0; k < weightsCount; k++)
                    _layers[i].Neurons[j].Weights[k] = rnd.NextDouble() * 2 - 1;
            }
        }
    }

    public void Train(TrainingData[] trainingDatas, int epochs, int batchSize, double learningRate)
    {
        for (int epoch = 0; epoch < epochs; epoch++)
        {
            trainingDatas.Shuffle();
            var cost = 0.0;
            var rightAnswers = 0;
            var iterationsCount = 0;
            foreach (var batch in GetBatches(batchSize, trainingDatas))
            {
                (var count, var errorsSum) = UpdateBatch(batch, learningRate);
                rightAnswers += count;
                iterationsCount += batchSize;
                cost += errorsSum;
            }
            Console.WriteLine($"{rightAnswers}/{iterationsCount} Epoch: {epoch}; Cost: {cost}");
        }
    }

    public void Test(TrainingData[] trainingDatas)
    {
        var rightAnswers = 0;
        foreach (var trainingData in trainingDatas)
        {
            _layers[0].UpdateNeurons(trainingData.Value);
            for (int i = 1; i < _layers.Length; i++)
                _layers[i].FeedForward(_layers[i - 1], _activation);
            var values = Results.Neurons.Select(n => n.Value).ToArray();
            if (Array.IndexOf(values, values.Max()) == trainingData.Answer)
                rightAnswers++;
        }
        Console.WriteLine($"{rightAnswers}/{trainingDatas.Length}");
    }

    public bool Test(TrainingData trainingData)
    {
        _layers[0].UpdateNeurons(trainingData.Value);
        for (int i = 1; i < _layers.Length; i++)
            _layers[i].FeedForward(_layers[i - 1], _activation);
        var values = Results.Neurons.Select(n => n.Value).ToArray();
        return Array.IndexOf(values, values.Max()) == trainingData.Answer;
    }

    private (int, double) UpdateBatch(TrainingData[] batch, double learningRate)
    {
        var rightAnswers = 0;
        var errorsSum = 0.0;
        foreach (var data in batch)
        {
            _layers[0].UpdateNeurons(data.Value);
            for (int i = 1; i < _layers.Length; i++)
                _layers[i].FeedForward(_layers[i - 1], _activation);
            var values = Results.Neurons.Select(n => n.Value).ToArray();
            if (Array.IndexOf(values, values.Max()) == data.Answer)
                rightAnswers++;
            var answers = new double[Results.Size];
            answers[data.Answer] = 1;
            double[] errors = new double[Results.Size];
            for (int i = 0; i < errors.Length; i++)
            {
                errors[i] = values[i] - answers[i];
                errorsSum += errors[i] * errors[i];
            }
            for (int i = _layers.Length - 1; i > 0; i--)
                errors = _layers[i].BackPropagate(
                    errors,
                    _layers[i - 1],
                    _derivative,
                    learningRate
                );
        }
        return (rightAnswers, errorsSum);
    }

    private IEnumerable<TrainingData[]> GetBatches(int batchSize, TrainingData[] trainingDatas)
    {
        var values = new TrainingData[batchSize];
        for (int i = 0; i < trainingDatas.Length; i++)
        {
            var index = i % batchSize;
            values[index] = trainingDatas[i];
            if (i != 0 && index + 1 == batchSize)
                yield return values;
        }
    }

    private void DebugLayer(TrainingData trainingData)
    {
        Console.WriteLine("______");
        Debug(trainingData.Value);
        Console.WriteLine("@@@");
        Console.WriteLine(trainingData.Answer);
        for (var i = 0; i < Results.Size; i++)
            Console.WriteLine($"{i}. {Results.Neurons[i].Value}");
        Console.WriteLine("______");
    }

    private void Debug(double[] image)
    {
        for (int i = 0; i < image.Length; i++)
        {
            if (i % 28 == 0)
                Console.WriteLine();
            var number = (int)image[i];
            Console.Write(number);
            if (number < 10)
                Console.Write("  ");
            else if (number < 100)
                Console.Write(" ");
            Console.Write(" ");
        }
        Console.WriteLine();
        Console.WriteLine();
    }
}
