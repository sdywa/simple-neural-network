namespace NeuralNetwork;

public class TrainingData
{
    public int Answer { get; set; }
    public double[] Value { get; set; }

    public TrainingData(int answer, double[] value)
    {
        Answer = answer;
        Value = value;
    }
}
