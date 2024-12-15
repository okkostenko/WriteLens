using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WriteLens.Auth.Infrastructure.Data;
using WriteLens.Auth.Helpers;
using WriteLens.Auth.Settings;
using WriteLens.Auth.Interfaces.Repositories;
using WriteLens.Auth.Infrastructure.Repositories;
using WriteLens.Auth.Interfaces.Services;
using WriteLens.Auth.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using System.Reflection;


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
        options.Audience = jwtSettings.Audience;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            NameClaimType = JwtRegisteredClaimNames.Sub
        };
    }
);

builder.Services.AddAuthorization();

// * PostgresDB
var dbSettings = builder.Configuration
    .GetSection(nameof(PostgresDbSettings))
    .Get<PostgresDbSettings>() ?? throw new InvalidOperationException("'PostgresDbSettings' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(dbSettings.ConnectionString).LogTo(Console.WriteLine, LogLevel.Information)
);

// * Repositories
builder.Services.AddScoped<IUserRepository, PostgresDbUserRepository>();

// * Services
builder.Services.AddScoped<IAuthService, AuthService>();

// * Caching
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

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddHttpContextAccessor();

builder.WebHost.UseUrls("http://0.0.0.0:80");

var app = builder.Build();

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
