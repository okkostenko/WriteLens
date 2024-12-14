using MassTransit;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using StackExchange.Redis;
using WriteLens.Accessibility.Application.Services;
using WriteLens.Accessibility.Helpers;
using WriteLens.Accessibility.Infrastructure.Caching;
using WriteLens.Accessibility.Infrastructure.Repositories;
using WriteLens.Accessibility.Interfaces.Repositories;
using WriteLens.Accessibility.Interfaces.Services;
using WriteLens.Accessibility.Settings;
using WriteLens.Accessibility.WebAPI.Consumers;
using WriteLens.Shared.Interfaces.Caching;
using WriteLens.Shared.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// * Serializers
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

// * Config
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection(nameof(ApplicationSettings)));
builder.Services.Configure<MLTASSettings>(builder.Configuration.GetSection(nameof(MLTASSettings)));

// * Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddOpenApi();

builder.Configuration.AddUserSecrets<Program>();

// * Redis
var redisSettings = builder.Configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>();
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(redisSettings.ConnectionString) // Use your Redis connection string
);

// * MongoDB
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));
builder.Services.AddScoped<IMongoClient>(
    ServiceProvider => new MongoClient(mongoDbSettings.ConnectionString)
);

// * Repositories
builder.Services.AddScoped<IDocumentContentRepository, MongoDbDocumentContentRepository>();
builder.Services.AddScoped<IDocumentScoreRepository, MongoDbDocumentScoreRepository>();
builder.Services.AddScoped<IDocumentFlagsRepository, MongoDbDocumentFlagsRepository>();
builder.Services.AddScoped<IDocumentTypeRepository, MongoDbDocumentTypeRepository>();

// * Services
builder.Services.AddScoped<IAccessibilityService, AccessibilityService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// * Caching
builder.Services.AddScoped<IDocumentTypeCache, DocumentTypeCache>();
builder.Services.AddScoped<ITaskCache, TaskCache>();
builder.Services.AddMemoryCache();

// * MassTransit
var rabbitMQSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
builder.Services.AddMassTransit(x => 
{
    x.AddConsumer<AccessibilityAnalysisConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMQSettings.Host, h =>
        {
            h.Username(rabbitMQSettings.Username);
            h.Password(rabbitMQSettings.Password);
        });
        
        cfg.ReceiveEndpoint("accessibility-analysis-queue", e =>
        {
            e.ConfigureConsumer<AccessibilityAnalysisConsumer>(context);
        });
    });
});

// * Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();

// * Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddHttpContextAccessor();

builder.WebHost.UseUrls("http://0.0.0.0:80");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var documentTypeCache = scope.ServiceProvider.GetRequiredService<IDocumentTypeCache>();
    await documentTypeCache.RefreshCacheAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.MapControllers();

app.Run();