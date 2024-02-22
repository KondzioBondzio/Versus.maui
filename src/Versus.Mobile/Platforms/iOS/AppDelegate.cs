using Foundation;
using iOS = Versus.Mobile.Platforms.iOS;

namespace Versus.Mobile;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() =>
        MauiProgram.CreateMauiApp(iOS.DependencyInjection.Configure, iOS.DependencyInjection.ConfigureServices);
}
