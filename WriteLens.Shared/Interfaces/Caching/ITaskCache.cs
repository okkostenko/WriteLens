namespace WriteLens.Shared.Interfaces.Caching;

public interface ITaskCache
{
    Task RegisterTaskAsync(Guid taskId);
    Task UpdateTaskAsync<T>(Guid taskId, T task);
    Task RemoveTaskAsync(Guid taskId);
    Task<T?> GetTaskAsync<T>(Guid taskId);
}