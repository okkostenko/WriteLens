using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using WriteLens.Core.Infrastructure.Data.PostgresDb;
using WriteLens.Core.Helpers;
using WriteLens.Core.Settings;
using WriteLens.Shared.Settings;
using WriteLens.Core.Interfaces.Caching;
using WriteLens.Core.Infrastructure.Caching;
using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Core.Infrastructure.Repositories;
using WriteLens.Core.Interfaces.Services;
using WriteLens.Core.Application.Services;
using System.IdentityModel.Tokens.Jwt;

// * Serializers
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Configuration.AddUserSecrets<Program>();

// * Authentication
var jwtSettings = builder.Configuration
    .GetSection(nameof(JwtSettings))
    .Get<JwtSettings>() ?? throw new InvalidOperationException("'JwtSettings' not found.");
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));

builder.Services.AddAuthentication(options => 
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            NameClaimType = JwtRegisteredClaimNames.Sub
        };
    });

builder.Services.AddAuthorization();

// * MongoDB
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));
builder.Services.AddScoped<IMongoClient>(
    ServiceProvider => new MongoClient(mongoDbSettings.ConnectionString)
);

// * PostgresDB
var dbSettings = builder.Configuration
    .GetSection(nameof(PostgresDbSettings))
    .Get<PostgresDbSettings>() ?? throw new InvalidOperationException("'PostgresDbSettings' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(dbSettings.ConnectionString).LogTo(Console.WriteLine, LogLevel.Information)
);

// * Repositories
builder.Services.AddScoped<IDocumentTypeRepository, MongoDbDocumentTypeRepository>();

builder.Services.AddScoped<IUserRepository, PostgresDbUserRepository>();

builder.Services.AddScoped<IDocumentRepository, PostgresDbDocumentRepository>();

builder.Services.AddScoped<IDocumentContentRepository, MongoDbDocumentContentRepository>();
builder.Services.AddScoped<IDocumentContentRepositoryInserter, MongoDbDocumentContentRepositoryInserter>();
builder.Services.AddScoped<IDocumentContentRepositoryDeleter, MongoDbDocumentContentRepositoryDeleter>();
builder.Services.AddScoped<IDocumentContentRepositoryUpdater, MongoDbDocumentContentRepositoryUpdater>();
builder.Services.AddScoped<IDocumentContentRepositoryRetriever, MongoDbDocumentContentRepositoryRetriever>();

builder.Services.AddScoped<IDocumentScoreRepository, MongoDbDocumentScoreRepository>();
builder.Services.AddScoped<IDocumentScoreRepositoryInserter, MongoDbDocumentScoreRepositoryInserter>();
builder.Services.AddScoped<IDocumentScoreRepositoryDeleter, MongoDbDocumentScoreRepositoryDeleter>();
builder.Services.AddScoped<IDocumentScoreRepositoryUpdater, MongoDbDocumentScoreRepositoryUpdater>();

builder.Services.AddScoped<IDocumentFlagsRepository, MongoDbDocumentFlagsRepository>();
builder.Services.AddScoped<IDocumentFlagsRepositoryDeleter, MongoDbDocumentFlagsRepositoryDeleter>();

// * Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IDocumentServiceCreate, DocumentServiceCreate>();
builder.Services.AddScoped<IDocumentServiceRead, DocumentServiceRead>();
builder.Services.AddScoped<IDocumentServiceUpdate, DocumentServiceUpdate>();
builder.Services.AddScoped<IDocumentServiceDelete, DocumentServiceDelete>();
builder.Services.AddScoped<IDocumentScoreService, DocumentScoreService>();
builder.Services.AddScoped<IDocumentTypeService, DocumentTypeService>();

// * Caching
builder.Services.AddScoped<IDocumentTypeCache, DocumentTypeCache>();
builder.Services.AddMemoryCache();

// * Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
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
    var services = scope.ServiceProvider;

    // Run Postgres Migrations
    var context = services.GetRequiredService<ApplicationDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }

    var mongoDbClient = services.GetRequiredService<IMongoClient>();
    await MongoDbMigrationRunner.RunMigrations(
        mongoDbClient.GetDatabase(mongoDbSettings?.DatabaseName)
    );
    Console.WriteLine("MongoDb Migrations Run Successful");
    
    // Refresh Cache
    var documentTypeCache = services.GetRequiredService<IDocumentTypeCache>();
    await documentTypeCache.RefreshCacheAsync();
}


app.UseAuthentication();
app.UseAuthorization();
// app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
