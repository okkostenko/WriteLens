using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using WriteLens.Shared.Constants;
using WriteLens.Shared.Entities;

public static class MongoDbMigrationRunner
{
    public static async Task RunMigrations(IMongoDatabase database)
    {
        await CreateCollections(database);
        await FillCollections(database);
    }

    private static async Task CreateCollections(IMongoDatabase database)
    {
        var currCollections = database.ListCollectionNames().ToList();
        var collections = typeof(MongoDbCollections)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly) // Filter for constants
            .Select(fi => fi.GetValue(null)?.ToString()) // Retrieve values
            .ToArray();


        foreach (string collectionName in collections)
        {
            if (!currCollections.Contains(collectionName))
            {
                await database.CreateCollectionAsync(collectionName);
            }
        }
    }

    private static async Task FillCollections(IMongoDatabase database)
    {
        string? initialDataPath = Environment.GetEnvironmentVariable("MONGO_DB_INITIAL_DATA_PATH");
        if (initialDataPath is null)
        {
            Console.WriteLine("No inital data file is found");
            return;
        }

        await FillDocumentTypes(database, initialDataPath);
    }

    private static async Task FillDocumentTypes(IMongoDatabase database, string dataPath)
    {
        var collection = database.GetCollection<DocumentTypeEntity>(MongoDbCollections.DocumentTypes);
        string filePath = $"{dataPath}/{MongoDbCollections.DocumentTypes}.json";
        string jsonContent = await File.ReadAllTextAsync(filePath);
        BsonDocument[] data = BsonSerializer.Deserialize<BsonDocument[]>(jsonContent);

        await FillCollectionWithData(collection, data);
    }

    private static async Task FillCollectionWithData<T>(IMongoCollection<T> collection, BsonDocument[] seedData)
    {
        var options = new ReplaceOptions { IsUpsert = true };
        foreach (BsonDocument data in seedData)
        {
            var filter = Builders<T>.Filter.Eq("_id", data["_id"]);
            await collection.ReplaceOneAsync(
                filter,
                BsonSerializer.Deserialize<T>(data),
                options);
        }
    }
}