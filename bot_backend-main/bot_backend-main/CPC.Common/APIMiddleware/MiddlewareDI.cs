using CPC.Application.Services;
using CPC.Domain.Aggregations.UserActivities;
using CPC.Domain.Aggregations.Users;
using CPC.Domain.Models;
using CPC.Infraestructure.Crosscutting.DataObjects.Contracts;
using CPC.Infraestructure.Crosscutting.DataObjects.Core;
using CPC.Infrastructure.CrossCutting.Helpers;
using CPC.Infrastructure.CrossCutting.ICommon;
using CPC.Infrastructure.DataPersistent.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC.Common.APIMiddleware
{
    public static class MiddlewareDI
    {       
        public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CpcContext>(options =>
                        options.UseMySQL(connectionString).EnableSensitiveDataLogging(), ServiceLifetime.Scoped);

            return services;
        }

        public static IServiceCollection AddIntegrationsDI(this IServiceCollection services)
        {            
            services.AddScoped<IRepository, Repository<CpcContext>>();
            
            // Repositories
            services.AddScoped<IRepositoryUsers, RepositoryUsers>();
            services.AddScoped<IRepositoryUserActivities, RepositoryUserActivities>();
            
            // Services
            services.AddScoped<IServiceUsers, ServiceUsers>();
            services.AddScoped<IServiceUserActivities, ServiceUserActivities>();
            services.AddScoped<ISecurity, Security>();


            return services;
        }
    }
}
