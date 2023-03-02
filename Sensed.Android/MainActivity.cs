using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Avalonia.Android;
using Avalonia.Controls.ApplicationLifetimes;
using Sensed.Views;

namespace Sensed.Android;

[Activity(Label = "Sensed.Android", Theme = "@style/MyTheme.NoActionBar", Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
public class MainActivity : AvaloniaMainActivity
{
    public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent? e)
    {
        if (keyCode == Keycode.Back)
        {
            if (Avalonia.Application.Current.ApplicationLifetime is ISingleViewApplicationLifetime lifetime)
            {
                if (lifetime.MainView is MainView mv && mv.OnBackButton((int)keyCode, e)) return true;
            }
        }

        return base.OnKeyUp(keyCode, e);
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

    protected override void OnDestroy()
    {
        //TODO: работает лишь при выходе через кнопку возврата
        //if (Avalonia.Application.Current.ApplicationLifetime is ISingleViewApplicationLifetime lifetime)
        //{
        //    if (lifetime.MainView is MainView mv)
        //        mv.Close();
        //}
        base.OnDestroy();
    }
}
