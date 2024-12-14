namespace WriteLens.Shared.Settings;

public class MongoDbSettings
{
    public string DatabaseName { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string User { get; set; }
    public string Password { get; set; }

    public string ConnectionString {
        
        get => $"mongodb://{User}:{Password}@{Host}:{Port}";
    }

    public string LocalConnectionString {
        get => $"mongodb://{Host}:{Port}";
    }
    }
