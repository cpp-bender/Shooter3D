public static class Utility
{
	public static T[] Shuffle<T>(T[] array, int seed)
    {
        //The Fisher-Yates Shuffle Algorithm
        System.Random rnd = new System.Random(seed);
        for (int i = 0; i < array.Length - 1; i++)
        {
            int randomIndex = rnd.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }
        return array;
		string a="ahmet";
    }
}