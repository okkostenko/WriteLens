namespace WriteLens.Shared.Exceptions.DocumentExceptions;

[System.Serializable]
public class DocumentNotFoundException : Exception
{
    public DocumentNotFoundException() { }
    public DocumentNotFoundException(string message) : base(message) { }
    public DocumentNotFoundException(string message, System.Exception inner)
        : base(message, inner) { }
    public DocumentNotFoundException(string keyName, object key)
        : base($"Document with {keyName} '{key}' does not exist") {}
    protected DocumentNotFoundException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}