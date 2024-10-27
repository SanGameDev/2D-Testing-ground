using System.Collections.Generic;

public static class WordSeparation
{
    public static List<string> GetWordsFromText(string text)
    {
        string[] wordsArray = text.Split(' ');
        List<string> wordsList = new List<string>(wordsArray);
        return wordsList;
    }
}
