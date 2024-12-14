[System.Serializable]
public class InvalidSortingFiledException: System.Exception
{
    public InvalidSortingFiledException() { }
    public InvalidSortingFiledException(string message) : base(message) { }
    public InvalidSortingFiledException(string message, System.Exception inner) : base(message, inner) { }
    protected InvalidSortingFiledException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}