using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Battleship.Maui;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Tint top status bar and bottom nav bar to dark navy (#020B14)
        Window?.SetStatusBarColor(Android.Graphics.Color.ParseColor("#020B14"));
        Window?.SetNavigationBarColor(Android.Graphics.Color.ParseColor("#020B14"));
    }
}