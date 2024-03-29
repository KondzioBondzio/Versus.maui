﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Versus.Core.Services;
using Versus.Core.Services.Session;
using Versus.Mobile.Services.Session;

namespace Versus.Mobile;

public static class DependencyInjection
{
    public static IConfigurationBuilder AddVersusConfiguration(this IConfigurationBuilder configuration,
        Action<IConfigurationBuilder> configureDelegate)
    {
        configuration
            .AddJsonFile(new EmbeddedFileProvider(typeof(MauiProgram).Assembly),
                "appsettings.json", false, false);

#if DEBUG
        configuration
            .AddJsonFile(new EmbeddedFileProvider(typeof(MauiProgram).Assembly),
                "appsettings.development.json", true, false);
#endif

        configureDelegate(configuration);
        return configuration;
    }

    public static IServiceCollection AddVersusServices(this IServiceCollection services,
        IConfigurationRoot configuration,
        Action<IConfigurationRoot, IServiceCollection> servicesDelegate)
    {
        services.AddLocalization();

        HttpClient httpClient = new() { BaseAddress = new Uri(configuration["ApiAddress"]!) };
        services.AddSingleton(new ApiClient(httpClient));
        services.AddSingleton<ISession, SecureStorageSession>();
        services.AddSingleton<SessionManager>();

        servicesDelegate(configuration, services);
        return services;
    }

    public static IServiceCollection AddVersusLogging(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddLogging(options => { options.AddSerilog(); });

        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            var exception = args.ExceptionObject as Exception;
            Log.Logger.Fatal(exception, "Unhandled exception: {ErrorMessage}", exception?.Message);
        };

        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            Log.Logger.Fatal(e.Exception, "Unhandled exception: {ErrorMessage}", e.Exception.Message);
            e.SetObserved();
        };

        return services;
    }
}
