using Android.App;
using Android.OS;
using Android.Content.PM;
using Microsoft.Maui;
using Microsoft.Maui.Essentials;

namespace Memo
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : MauiAppCompatActivity
	{
protected override void OnCreate(Bundle savedInstanceState) {
base.OnCreate(savedInstanceState);
Microsoft.Maui.Essentials.Platform.Init(this, savedInstanceState);
}


	}
}