namespace WriteLens.Core.Exceptions;

[System.Serializable]
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, object key)
        : base($"{entityName} with key  '{key}' does not exist.") {}
}