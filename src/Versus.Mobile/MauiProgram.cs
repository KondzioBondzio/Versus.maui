using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;

namespace Versus.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp(Action<IConfigurationBuilder> configureDelegate,
        Action<IConfigurationRoot, IServiceCollection> servicesDelegate)
    {
        var builder = MauiApp.CreateBuilder();
        builder.Configuration.AddVersusConfiguration(configureDelegate);

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services
            .AddVersusLogging(builder.Configuration)
            .AddVersusServices(builder.Configuration, servicesDelegate);

        return builder.Build();
    }
}
