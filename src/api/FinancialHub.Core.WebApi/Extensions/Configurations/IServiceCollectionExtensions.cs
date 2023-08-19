using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using FinancialHub.Core.WebApi.Validators;

namespace FinancialHub.Core.WebApi.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApiConfigurations(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Financial Hub WebApi", Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidation(x =>
            {
                x.AutomaticValidationEnabled = true;
                x.DisableDataAnnotationsValidation = true;
            });
            services.AddScoped<IValidator<AccountModel>, AccountValidator>();
            services.AddScoped<IValidator<CategoryModel>, CategoryValidator>();
            services.AddScoped<IValidator<TransactionModel>, TransactionValidator>();
            return services;
        }
    }
}
