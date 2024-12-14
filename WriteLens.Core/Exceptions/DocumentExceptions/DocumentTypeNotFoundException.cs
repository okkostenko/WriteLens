namespace WriteLens.Core.Exceptions;

[System.Serializable]
public class DocumentTypeNotFoundException : DocumentException
{
    public DocumentTypeNotFoundException() { }
    public DocumentTypeNotFoundException(string message) : base(message) { }
    public DocumentTypeNotFoundException(string message, System.Exception inner)
        : base(message, inner) { }
    public DocumentTypeNotFoundException(string keyName, object key)
        : base($"Document type with {keyName} '{key}' does not exist") {}
    protected DocumentTypeNotFoundException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}