namespace WriteLens.Shared.Exceptions.AnalysisExceptions;

[System.Serializable]
public class NothigToAnalyzeException : System.Exception
{
    public NothigToAnalyzeException() 
        : base($"Document is already analyzed") { }
    public NothigToAnalyzeException(string message) : base(message) { }
    public NothigToAnalyzeException(string message, System.Exception inner) : base(message, inner) { }
    public NothigToAnalyzeException(string keyName, object key)
        : base($"Document with {keyName} '{key}' has no content to analyze") {}
    protected NothigToAnalyzeException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}