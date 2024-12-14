namespace WriteLens.Shared.Exceptions;

[System.Serializable]
public class AccessDeniedException : System.Exception
{
    public AccessDeniedException() { }
    public AccessDeniedException(string message) : base(message) { }
    public AccessDeniedException(string message, System.Exception inner) : base(message, inner) { }
    public AccessDeniedException(object userId, string objectName, object objectKey)
        : base($"User with id '{userId}' does not have access to the {objectName} with key {objectKey}") {}
    protected AccessDeniedException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}