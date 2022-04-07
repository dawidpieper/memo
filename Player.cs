using System;
using System.IO;
using System.Collections.Generic;
#if ANDROID
using Android.Media;
using Android.Content.Res;
#elif IOS
using Foundation;
using AVFoundation;
#endif

namespace Memo {
public static class Player {
#if ANDROID
static List<MediaPlayer> Players = new List<MediaPlayer>();
#elif IOS
static List<AVAudioPlayer> Players = new List<AVAudioPlayer>();
#endif
public static void PlaySound(string soundname) {
#if ANDROID
var player = new MediaPlayer();
var fd = global::Android.App.Application.Context.Assets.OpenFd("Sounds/"+soundname+".mp3");
player.Prepared += (s, e) => player.Start();
Players.Add(player);
player.Completion += (s, e) => {
player.Release();
fd.Close();
Players.Remove(player);
};
player.SetDataSource(fd.FileDescriptor,fd.StartOffset,fd.Length);
player.Prepare();
#elif IOS
var url = NSBundle.MainBundle.GetUrlForResource("Sounds/"+soundname, "mp3");
var _player = AVAudioPlayer.FromUrl(url);
Players.Add(_player);
_player.FinishedPlaying += (sender, e) => {
Players.Remove(_player);
};
_player.Play();
#endif
}
}
}