using Android.App;
using Android.Runtime;
using Droid = Versus.Mobile.Platforms.Android;

namespace Versus.Mobile;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() =>
        MauiProgram.CreateMauiApp(Droid.DependencyInjection.Configure, Droid.DependencyInjection.ConfigureServices);
}
