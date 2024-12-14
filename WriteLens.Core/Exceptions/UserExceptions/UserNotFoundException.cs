namespace WriteLens.Core.Exceptions;

[System.Serializable]
public class UserNotFoundException : UserException
{
    public UserNotFoundException() { }
    public UserNotFoundException(string message) : base(message) { }
    public UserNotFoundException(string message, System.Exception inner)
        : base(message, inner) { }
    public UserNotFoundException(string keyName, object key)
        : base($"User with {keyName} '{key}' does not exist") {}
    protected UserNotFoundException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}