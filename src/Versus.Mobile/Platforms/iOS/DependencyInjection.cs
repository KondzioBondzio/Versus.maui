using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Versus.Mobile.Platforms.iOS;

public static partial class DependencyInjection
{
    public static Action<IConfigurationBuilder> Configure => (builder) =>
    {
        builder.AddJsonFile(
            new EmbeddedFileProvider(typeof(DependencyInjection).Assembly, typeof(DependencyInjection).Namespace),
            "appsettings.json", false, false);

#if DEBUG
        builder.AddJsonFile(
            new EmbeddedFileProvider(typeof(DependencyInjection).Assembly, typeof(DependencyInjection).Namespace),
            "appsettings.development.json", true, false);
#endif
    };

    public static Action<IConfigurationRoot, IServiceCollection> ConfigureServices => (_, services) =>
    {
        // ...
    };
}
