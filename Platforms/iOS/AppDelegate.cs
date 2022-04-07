using System;
using System.Globalization;
using Foundation;
using AVFoundation;
using UIKit;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Memo;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
{
CultureInfo.CurrentUICulture = new CultureInfo(NSBundle.MainBundle.PreferredLocalizations[0], false);

var notification = NSLocale.Notifications.ObserveCurrentLocaleDidChange ((sender, args) => {
CultureInfo.CurrentUICulture = new CultureInfo(NSBundle.MainBundle.PreferredLocalizations[0], false);

});

    AVAudioSession session = AVAudioSession.SharedInstance();
    session.SetCategory(AVAudioSessionCategory.Playback, AVAudioSessionCategoryOptions.MixWithOthers );
    session.SetActive(true);

    return base.FinishedLaunching(application, launchOptions);
}


}
