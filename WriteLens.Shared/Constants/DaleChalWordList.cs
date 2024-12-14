namespace WriteLens.Shared.Constants;

public class DaleChalWordList
{
    public DaleChalWordList()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "dale_chall_word_list.txt");
        CommonWords = new HashSet<string>(File.ReadAllLines(filePath));
    }

    public HashSet<string> CommonWords { get; set; }
}