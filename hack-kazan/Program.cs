using System.Runtime.CompilerServices;
using Hack.DAL;
using Hack.DAL.Interfaces;
//using Hack.DAL.Repositories;
using Hack.Domain.Entities;
using Hack.Services;
using Hack.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hack.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// эстетично
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add services to the container

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMarkService, MarkService>();
//builder.Services.AddScoped<IMarkChainService, MarkChainService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext using SQL Server Provider
builder.Services.AddDbContext<ApplicationDbContext>();
//builder.Services.AddSingleton<IMarkChainRepository, MarkChainRepository>();

// Authentication, tokens
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateActor = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;

    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

var app = builder.Build();

//mirgration up как надо 
using (var context = (ApplicationDbContext)app.Services.GetService(typeof(ApplicationDbContext))!)
{
    context.Database.EnsureCreated();
}

app.UseCors(cors =>
{
    cors.AllowAnyHeader();
    cors.AllowAnyOrigin();
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", async (context) =>
{
    await context.Response.WriteAsync("Жизнь удалась");
});

app.Run();

// Man, I love Identity Framework
public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}