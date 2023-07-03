using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace NeuralNetwork;

public class DigitRecognition : NeuralNetwork
{
    public DigitRecognition(
        Func<double, double> activation,
        Func<double, double> derivative,
        params int[] sizes
    )
        : base(activation, derivative, sizes) { }

    public void Train(int epochs, int batchSize, double learningRate)
    {
        base.Train(GetData("NeuralNetwork/Numbers/train"), epochs, batchSize, learningRate);
    }

    public void Test()
    {
        base.Test(GetData("NeuralNetwork/Numbers/test"));
    }

    private TrainingData[] GetData(string path)
    {
        var files = new DirectoryInfo(path).GetFiles();
        var images = new Image<Rgba32>[files.Length];
        var answers = new int[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            images[i] = Image.Load<Rgba32>(files[i].FullName);
            var file = Path.GetFileNameWithoutExtension(files[i].Name);
            answers[i] = file[file.Length - 1] - '0';
        }

        var inputs = new TrainingData[images.Length];
        for (int i = 0; i < inputs.Length; i++)
        {
            var image = new double[784];
            for (int x = 0; x < 28; x++)
            {
                for (int y = 0; y < 28; y++)
                {
                    Rgba32 pixelColor = images[i][y, x];
                    double color =
                        0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B;
                    image[y + x * 28] = color;
                }
            }
            inputs[i] = new TrainingData(answers[i], image);
        }
        return inputs;
    }
}
