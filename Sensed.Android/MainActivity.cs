using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.Core.App;
using AndroidX.Core.Graphics.Drawable;
using Avalonia;
using Avalonia.Android;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using Sensed.Views;
using System.IO;
using System.Text;

namespace Sensed.Android;

[Activity(
    Label = "Sensed.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    NoHistory = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    public MainView? MainView
    {
        get
        {
            if (Avalonia.Application.Current.ApplicationLifetime is ISingleViewApplicationLifetime lifetime)
            {
                return lifetime.MainView as MainView;
            }
            return null;
        }
    }

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }

    public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent? e)
    {
        return base.OnKeyUp(keyCode, e);
    }

    public override void OnBackPressed()
    {
        if (MainView?.OnBackButton() == true)
            return;

        base.OnBackPressed();
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    }

    protected override void OnRestart()
    {
        base.OnRestart();
    }

    protected override void OnResume()
    {
        base.OnResume();
    }

    protected override void OnPause()
    {
        base.OnPause();
    }

    //protected override void OnDestroy()
    //{
    //    //TODO: работает лишь при выходе через кнопку возврата
    //    //if (Avalonia.Application.Current.ApplicationLifetime is ISingleViewApplicationLifetime lifetime)
    //    //{
    //    //    if (lifetime.MainView is MainView mv)
    //    //        mv.Close();
    //    //}
    //    base.OnDestroy();
    //}

    protected override void OnDestroy()
    {
        //TODO: работает лишь при выходе через кнопку возврата???


        //работает при закрытии всего приложения и при выходе через кнопку возврата
        if (Avalonia.Application.Current.ApplicationLifetime is ISingleViewApplicationLifetime lifetime)
        {
            if (lifetime.MainView is MainView mv)
                mv.Close();
        }
        System.Console.WriteLine("Destroying app.");
        base.OnDestroy();
    }
}