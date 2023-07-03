namespace NeuralNetwork;

public static class ArrayExtensions
{
    private static Random rnd = new Random();

    public static void Shuffle<T>(this T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int k = rnd.Next(i);
            (array[k], array[i]) = (array[i], array[k]);
        }
    }
}
