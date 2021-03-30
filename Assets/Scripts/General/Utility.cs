public static class Utility
{
    public static T[] Shuffle<T>(T[] array, int seed)
    {
        System.Random rnd = new System.Random();
        for (int i = 0; i < array.Length - 1; i++)
        {
            int randomIndex = rnd.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }
        return array;
    }
}
