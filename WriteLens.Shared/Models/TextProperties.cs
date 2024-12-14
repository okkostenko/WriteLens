namespace WriteLens.Shared.Models;

public class TextProperties
{
    public string Text { get; set; }
    public int SentancesCount { get; set; }
    public int WordsCount { get; set; }
    public int SyllablesCount { get; set; }
    public int CharCount { get => Text.Length; }
}