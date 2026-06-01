using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PointsTableAndExams.Application.Common.Interfaces;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using PointsTableAndExams.Infrastructure.Data;
using PointsTableAndExams.Infrastructure.Data.Repositories;
using PointsTableAndExams.Infrastructure.Services;
using Serilog;

namespace PointsTableAndExams.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        // Unit of Work
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDailyLogRepository, DailyLogRepository>();
        services.AddScoped<IExamRequestRepository, ExamRequestRepository>();
        services.AddScoped<IFoodItemRepository, FoodItemRepository>();

        // Services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUserService>();

        // JWT Authentication
        var jwtSettings = configuration.GetSection("JwtSettings");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Secret"]!))
                };
            });

        services.AddAuthorization();

        return services;
    }
}
