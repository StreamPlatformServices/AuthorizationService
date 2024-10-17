using AuthorizationService.Configuration;
using AuthorizationService.Data;
using AuthorizationService.Extensions;
using AuthorizationService.Models;
using AuthorizationService.Service;
using AuthorizationService.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configPath = Path.GetFullPath(builder.Configuration["ConfigPath"] ?? Directory.GetCurrentDirectory());
var databasePath = Path.GetFullPath(builder.Configuration["DatabasePath"] ?? Directory.GetCurrentDirectory());

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(Path.Combine(configPath, "appsettings.json"), optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

var jwtSettings = builder.Configuration.GetSection("ApiSettings:JwtOptions");
var kestrelSettings = builder.Configuration.GetSection("KestrelSettings").Get<KestrelSettings>() ?? throw new Exception("Fatal error: Please provide kestrel configuration");

builder.AddKestrelSettings(kestrelSettings);

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlite($"Data source={databasePath}/users.db"));
builder.Services.Configure<JwtOptions>(jwtSettings);
builder.Services
    .AddIdentityCore<AppUser>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<PasswordHasherOptions>(options =>
{
    options.IterationCount = 600_000;
});

builder.Services.AddAuthenticationServices(jwtSettings);
builder.Services.AddAuthorizationServices();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGeneratorService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>() ?? throw new Exception("Fatal error: Please provide CorsSettings configuration");
var corsPolicyName = "CustomCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName,
        policy =>
        {
            policy.WithOrigins(corsSettings.AllowedHosts)
                .WithHeaders(corsSettings.AllowedHeaders)
                .WithMethods(corsSettings.AllowedMethods);
        });
});

var useSwagger = builder.Configuration.GetSection("UseSwagger").Get<bool>();

if (useSwagger)
{
    builder.Services.AddSwaggerDocumentation();
}

builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

if (useSwagger)
{

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthorizationService API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors(corsPolicyName);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
