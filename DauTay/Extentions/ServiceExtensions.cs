using DauTay.Interfaces;
using DauTay.Repositories;
using DauTay.Service;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DauTay.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowMyOrigin",
            builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });
    }

    public static void ConfigureDbContext(this IConfiguration configuration)
    {
        //GetMongoDatabase config
        MongoSetting.Connection = configuration.GetSection("MongoSettings:Connection").Value;
        MongoSetting.DatabaseName = configuration.GetSection("MongoSettings:DatabaseName").Value;
    }

    public static void ConfigureRepositoryWrapper(this IServiceCollection services)
    {
        services.AddScoped<IMongoContext, MongoContext>();
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}
