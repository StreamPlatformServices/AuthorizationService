using Microsoft.EntityFrameworkCore;
using AuthorizationService.Data;
using AuthorizationService.Service.IService;
using AuthorizationService.Service;
using Microsoft.AspNetCore.Identity;
using AuthorizationService.Models;
using AuthorizationService.Entity;
using AuthorizationService.Extensions;

var builder = WebApplication.CreateBuilder(args);
var settingsSection = builder.Configuration.GetSection("ApiSettings:JwtOptions");
var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlite(defaultConnection));
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
/*builder.Services.AddEndpointsApiExplorer();*/

builder.Services.AddCorsServices();
builder.Services.AddSwaggerDocumentation();
/*builder.Services.AddControllers().AddNewtonsoftJson();*/

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
/*    app.UseCors("AllowAll");*/ // TODO: Check if it's needed
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
