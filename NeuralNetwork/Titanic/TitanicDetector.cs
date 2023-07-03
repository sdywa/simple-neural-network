using System.Text.RegularExpressions;

namespace NeuralNetwork;

public class TitanicDetector : NeuralNetwork
{
    private readonly HashSet<string> IgnoredValues = new HashSet<string>()
    {
        "PassengerId",
        "Ticket",
        "Cabin"
    };
    private readonly string AnswerColumn = "Survived";
    private readonly char CSVSeparator = ',';

    public TitanicDetector(
        Func<double, double> activation,
        Func<double, double> derivative,
        params int[] sizes
    )
        : base(activation, derivative, sizes) { }

    public void Train(int epochs, int batchSize, double learningRate)
    {
        var data = GetData(@"NeuralNetwork/Titanic/train", "train.csv");
        if (data is null)
            return;

        base.Train(data, epochs, batchSize, learningRate);
    }

    public void Test()
    {
        var data = GetTestData(@"NeuralNetwork/Titanic/test", "test.csv", "gender_submission.csv");
        if (data is null)
            return;

        base.Test(data);
    }

    public void Test(TitanicData data)
    {
        Console.WriteLine(base.Test(data.ToTrainingData()));
    }

    private TrainingData[]? GetData(string path, string file)
    {
        var datas = new List<TitanicData>();
        using (var reader = new StreamReader($@"{path}{Path.DirectorySeparatorChar}{file}"))
        {
            var columns = reader.ReadLine()?.Split(CSVSeparator);
            if (columns is null)
                return null;

            for (int i = 0; ; i++)
            {
                if (reader.EndOfStream)
                    break;

                var line = reader.ReadLine();
                if (line is null)
                    continue;

                var values = Regex.Split(line, @",(?!\s)");
                if (values is null)
                    continue;

                datas.Add(ParseData(columns, values));
            }
        }

        FixData(datas);
        return ConvertToTrainingData(datas);
    }

    private TrainingData[]? GetTestData(string path, string testFile, string answersFile)
    {
        var datas = GetData(path, testFile);
        if (datas is null)
            return null;

        using (var reader = new StreamReader($@"{path}{Path.DirectorySeparatorChar}{answersFile}"))
        {
            var columns = reader.ReadLine()?.Split(CSVSeparator);
            if (columns is null)
                return null;

            var answerIndex = columns.TakeWhile(c => c != AnswerColumn).Count();
            for (int i = 0; ; i++)
            {
                if (reader.EndOfStream)
                    break;

                var line = reader.ReadLine();
                if (line is null)
                    continue;

                var values = Regex.Split(line, @",(?!\s)");
                if (values is not null)
                {
                    var parsed = values.Select(v => int.Parse(v)).ToList();
                    datas[i].Answer = parsed[answerIndex];
                }
            }
        }

        return datas;
    }

    private TitanicData ParseData(string[] columns, string[] values)
    {
        var valuesDict = new Dictionary<string, string>();
        for (int i = 0; i < columns.Length; i++)
            valuesDict[columns[i]] = values[i];

        return new TitanicData(
            valuesDict["Name"],
            valuesDict["Sex"],
            valuesDict["Age"],
            valuesDict["SibSp"],
            valuesDict["Parch"],
            valuesDict["Fare"],
            valuesDict["Pclass"],
            valuesDict["Embarked"],
            valuesDict.ContainsKey(AnswerColumn) ? valuesDict[AnswerColumn] : "0"
        );
    }

    private void FixData(List<TitanicData> datas)
    {
        var age = (int)datas.Average(d => d.Age);
        var fare = (int)datas.Average(d => d.Fare);
        var embarked = "S";

        foreach (var data in datas)
            data.FixBrokenValues(age, fare, embarked);
    }

    private TrainingData[] ConvertToTrainingData(List<TitanicData> datas)
    {
        var converted = new TrainingData[datas.Count];
        for (int i = 0; i < converted.Length; i++)
            converted[i] = datas[i].ToTrainingData();
        return converted;
    }
}
