using System;
using LearningSystem.Data.EF;
using LearningSystem.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LearningSystem.Web.Configuration
{
    public static class DependenciesConfiguration
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            services
                .AddDbContext<LearningSystemDbContext>(opts => opts.UseSqlServer(connectionString))
                .AddEntityFrameworkSqlServer();
        }

        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<LearningSystemDbContext>();

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });
        }
    }
}