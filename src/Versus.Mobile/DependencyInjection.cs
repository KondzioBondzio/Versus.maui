using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Serilog;

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

        servicesDelegate(configuration, services);
        return services;
    }

    public static IServiceCollection AddVersusLogging(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddLogging(options => { options.AddSerilog(); });
        services.AddLocalization();
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
