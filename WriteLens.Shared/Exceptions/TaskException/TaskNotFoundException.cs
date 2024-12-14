namespace WriteLens.Shared.Exceptions.TaskExceptions;

[System.Serializable]
public class TaskNotFoundException : System.Exception
{
    public TaskNotFoundException() { }
    public TaskNotFoundException(string message) : base(message) { }
    public TaskNotFoundException(string message, System.Exception inner) : base(message, inner) { }
    public TaskNotFoundException(string keyName, object key)
        : base($"Task with {keyName} '{key}' does not exist or expiered.") {}
    protected TaskNotFoundException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}