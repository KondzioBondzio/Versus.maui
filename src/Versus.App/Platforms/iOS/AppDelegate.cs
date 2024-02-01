using Foundation;
using iOS = Versus.App.Platforms.iOS;

namespace Versus.App;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() =>
        MauiProgram.CreateMauiApp(iOS.DependencyInjection.Configure, iOS.DependencyInjection.ConfigureServices);
}
