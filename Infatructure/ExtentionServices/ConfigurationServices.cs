using System.Text;
using API.web_h13p.Application.Interface;
using API.web_h13p.Application.Services;
using API.web_h13p.Domain.Entities;
using API.web_h13p.Infatructure.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.web_h13p.Infatructure.ExtentionServices;

public static class ConfigurationServices
{
    public static void RegisterDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ConnectSQL");
        services.AddDbContext<WebApiDbContext>(option => option.UseSqlServer(connectionString));
        services.AddIdentity<User, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 6;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<WebApiDbContext>();
    }
    
    public static void JwtToken(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        });
    }
    
    public static void RegisterDI(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }
    public static void AutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }

    public static void Cors(this IServiceCollection service)
    {
        service.AddCors(option =>
        {
        option.AddPolicy(name: "PolicyCors",
            policy =>
            {
                policy.WithOrigins("http://localhost:3000");
                policy.WithMethods("POST", "GET");
                policy.AllowAnyHeader();
            });
        });
    }
}