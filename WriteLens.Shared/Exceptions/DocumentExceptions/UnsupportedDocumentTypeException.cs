namespace WriteLens.Shared.Exceptions.DocumentExceptions;

[System.Serializable]
public class UnsupportedDocumentTypeException : System.Exception
{
    public UnsupportedDocumentTypeException() { }
    public UnsupportedDocumentTypeException(string message) : base(message) { }
    public UnsupportedDocumentTypeException(string message, System.Exception inner) : base(message, inner) { }
    public UnsupportedDocumentTypeException(string keyName, object key)
        : base($"Document type with {keyName} '{key}' is not supported yet.") {}
    protected UnsupportedDocumentTypeException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}