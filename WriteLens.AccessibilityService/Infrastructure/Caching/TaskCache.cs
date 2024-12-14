using System.Text.Json;
using StackExchange.Redis;
using WriteLens.Shared.Exceptions.TaskExceptions;
using WriteLens.Shared.Interfaces.Caching;
using WriteLens.Accessibility.Models.DomainModels;

namespace WriteLens.Accessibility.Infrastructure.Caching;

public class TaskCache : ITaskCache
{
    private readonly IDatabase _taskCache;

    public TaskCache(IConnectionMultiplexer redis)
    {
        _taskCache = redis.GetDatabase();
    }

    public async Task RegisterTaskAsync(Guid taskId)
    {
        var task = new TaskModel{ TaskId = taskId, Status = "Pending" };

        await SetCacheValue(taskId, task);
    }

    public async Task UpdateTaskAsync<T>(Guid taskId, T task)
    {
        await SetCacheValue(taskId, task);
    }

    public async Task RemoveTaskAsync(Guid taskId)
    {
        await _taskCache.KeyDeleteAsync(taskId.ToString());
    }

    public async Task<T?> GetTaskAsync<T>(Guid taskId)
    {
        if ( ! await _taskCache.KeyExistsAsync(taskId.ToString()))
            return default; // Will return null for reference types

        var taskSerialized = await _taskCache.StringGetAsync(taskId.ToString());
        if (string.IsNullOrEmpty(taskSerialized))
            return default;

        try
        {
            return JsonSerializer.Deserialize<T>(taskSerialized) ?? default;
        }
        catch (JsonException exc)
        {
            return default;
        }
    }

    private async Task SetCacheValue<T>(Guid taskId, T task)
    {
        await _taskCache.StringSetAsync(
            taskId.ToString(),
            JsonSerializer.Serialize(task));
    }
}