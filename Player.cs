using System;
using System.IO;
#if ANDROID
using Android.Media;
using Android.Content.Res;
#elif IOS
using Foundation;
using AVFoundation;
#endif

namespace Memo {
public static class Player {
public static void PlaySound(string soundname) {
#if ANDROID
var player = new MediaPlayer();
var fd = global::Android.App.Application.Context.Assets.OpenFd("Sounds/"+soundname+".mp3");
player.Prepared += (s, e) => player.Start();
player.Completion += (s, e) => {
player.Release();
fd.Close();
};
player.SetDataSource(fd.FileDescriptor,fd.StartOffset,fd.Length);
player.Prepare();
#elif IOS
string sFilePath = NSBundle.MainBundle.PathForResource(Path.GetsoundnameWithoutExtension(soundname), Path.GetExtension("Sounds/"+soundname+".mp3"));
var url = NSUrl.FromString (sFilePath);
var _player = AVAudioPlayer.FromUrl(url);
_player.FinishedPlaying += (object sender, AVStatusEventArgs e) => {
_player = null;
};
_player.Play();
#endif
}
}
}