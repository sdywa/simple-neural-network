using System.Text.RegularExpressions;

namespace NeuralNetwork;

public class TitanicData
{
    public string Title { get; set; }
    public bool IsFemale { get; set; }
    public double Age { get; set; }
    public bool IsAlone { get; set; }
    public double Fare { get; set; }
    public double PClass { get; set; }
    public string? Embarked { get; set; }
    public bool IsSurvived { get; set; }
    public bool IsValid
    {
        get => Age != -1 && Fare != -1 && Embarked is not null;
    }

    private static readonly Dictionary<string, int> _titles = new Dictionary<string, int>()
    {
        ["Mr"] = 1,
        ["Miss"] = 2,
        ["Mrs"] = 3,
        ["Master"] = 4,
        ["Other"] = 5,
    };

    private static readonly Dictionary<string, int> _embarked = new Dictionary<string, int>
    {
        ["Q"] = 0,
        ["S"] = 1,
        ["C"] = 2
    };

    public TitanicData(
        string name,
        string sex,
        string age,
        string sibSp,
        string parch,
        string fare,
        string pclass,
        string embarked,
        string isSurvived
    )
    {
        Title = Regex.Match(name, @"([A-Za-z]+)(?=\.)").Value;
        IsFemale = sex == "female";
        Age = !string.IsNullOrEmpty(age) ? double.Parse(age) : -1;
        IsAlone = double.Parse(sibSp) + double.Parse(parch) == 0;
        Fare = !string.IsNullOrEmpty(fare) ? double.Parse(fare) : -1;
        PClass = double.Parse(pclass);
        Embarked = embarked;
        IsSurvived = Convert.ToBoolean(int.Parse(isSurvived));
    }

    public void FixBrokenValues(int age, int fare, string embarked)
    {
        if (IsValid)
            return;

        if (Age == -1)
            Age = age;
        if (Fare == -1)
            Fare = fare;
        if (Embarked is null)
            Embarked = embarked;
    }

    public TrainingData ToTrainingData()
    {
        var values = new double[]
        {
            _titles.ContainsKey(Title) ? _titles[Title] : _titles["Other"],
            Convert.ToDouble(IsFemale),
            Age,
            Convert.ToDouble(IsAlone),
            Fare,
            PClass,
            Embarked is not null && _embarked.ContainsKey(Embarked) ? _embarked[Embarked] : 1
        };
        return new TrainingData(Convert.ToInt32(IsSurvived), values);
    }
}
