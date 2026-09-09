using Dispatch.Api.Mapping;
using Dispatch.Application.Dtos;
using Dispatch.Application.Repository;
using Dispatch.Application.Services;
using Dispatch.Application.Validators;
using Dispatch.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Dispatch.Api.Extensions
{
    public static class ApplicationDependencies
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateWorkOrderRequestValidator>();
            services.AddScoped<IWorkOrderService, WorkOrderService>();
            services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
            services.AddAutoMapper(cfg => { }, typeof(WorkOrderMappingProfile).Assembly);
            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? Environment.GetEnvironmentVariable("SQLCONNSTR_DefaultConnection")
                ?? Environment.GetEnvironmentVariable("CUSTOMCONNSTR_DefaultConnection")
                ?? throw new InvalidOperationException(
                    "No database connection string configured. Set 'ConnectionStrings__DefaultConnection' " +
                    "as an environment variable / Azure App Setting, or add it to appsettings.json for local dev.");

            services.AddDbContext<DispatchDbContext>(options =>
                options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure()));


            return services;
        }
    }
}
