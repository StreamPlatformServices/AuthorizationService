using AuthorizationService.CommonConfiguration.ConfigurationModels;
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

var settingsSection = builder.Configuration.GetSection("ApiSettings:JwtOptions");
var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>() ?? throw new Exception("Fatal error: Please provide CorsSettings configuration");
var corsPolicyName = "CustomCorsPolicy";

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlite($"Data source={databasePath}/users.db"));
builder.Services.Configure<JwtOptions>(settingsSection);
builder.Services
    .AddIdentityCore<AppUser>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthenticationServices(settingsSection);
builder.Services.AddAuthorizationServices();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

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
builder.Services.AddSwaggerDocumentation();
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
