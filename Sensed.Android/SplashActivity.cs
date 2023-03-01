using Android.App;
using Android.Content;
using Android.OS;
using Application = Android.App.Application;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using Android.Runtime;
using Android.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Sensed.Views;

namespace Sensed.Android;

[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
public class SplashActivity : AvaloniaSplashActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .UseReactiveUI();
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    }

    protected override void OnDestroy()
    {
        //работает при закрытии всего приложения и при выходе через кнопку возврата
        if (Avalonia.Application.Current.ApplicationLifetime is ISingleViewApplicationLifetime lifetime)
        {
            if (lifetime.MainView is MainView mv)
                mv.Close();
        }
        System.Console.WriteLine("Destroying app.");
        base.OnDestroy();
    }

    protected override void OnRestart()
    {
        base.OnRestart();
    }

    protected override void OnResume()
    {
        base.OnResume();

        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
    }

    protected override void OnPause()
    {
        base.OnPause();
    }

    public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent? e)
    {
        if(keyCode == Keycode.Back)
        {
        
        }

        return base.OnKeyUp(keyCode, e);
    }
}
