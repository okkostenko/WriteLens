using System.Text.RegularExpressions;

namespace WriteLens.Shared.Utilities;

public static class TextUtitlity
{
    public const string VOWELS = "aeiouy";
    public static string[] GetSentances(string text)
        => Regex.Split(text, @"(?<=[.!?])\s+");
    
    public static int GetWordCount(string text)
        => text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

    public static int GetCharCount(string text)
        => text.Length;

    public static decimal CalculateAverageWordCountPerSentance(string text)
    {
        var sentences = GetSentances(text);
        var totalWords = GetWordCount(text);

        return sentences.Length == 0 ? 0 : totalWords / sentences.Length;
    }

    public static int GetSyllablesCount(string text)
    {
        text = text.ToLower();
        int count = 0;

        if (VOWELS.Contains(text[0]))
        count++;

        for (int i = 1; i < text.Length; i++)
        {
            if (VOWELS.Contains(text[i]) && ! VOWELS.Contains(text[i - 1]))
                count++;
        }

        if (text.EndsWith("e"))
            count--;

        return Math.Max(count, 1);
        }

    public static string Capitalize(string text)
    {
        return Regex.Replace(text, @"\b([a-z])", m => m.Value.ToUpper());
    }
}