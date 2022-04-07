using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using static Memo.Player;
using Memo.Resources;

namespace Memo {

public partial class TutorialPage : ContentPage {

int Frag=0;
int Ticks=0;
string SoundsBase;

public TutorialPage() {
InitializeComponent();
SoundsBase="birds";
Disappearing += (sender, e) => Board.Paused=true;
Board.Pick += (sender, e) => {
PlaySound("card_pick");
PlaySound(e.CardPicked.Sound);;
OnPick();
};
Board.Fail += (sender, e) => {
OnFail();
PlaySound("card_fail");
};
Board.Pair += (sender, e) => {
OnPair();
PlaySound("card_pair");
if(e.IsLastCard) {
PlaySound("success");
}
};
var ticker = Dispatcher.CreateTimer();
ticker.Interval = TimeSpan.FromSeconds(1);
ticker.IsRepeating=true;
ticker.Tick += (sender, e) => Tick();
ticker.Start();
Board.Paused=false;
SetHint(AppResources.TutorialStartHint);
StartGame(2,2,1);
}

public void Tick() {
if(!Board.Paused) {
++Ticks;
if(Ticks%2==0 && Board.IsPicked) PlaySound("card_picked");
}
}

public void SetHint(string h) {
HintLabel.Text=h;
SemanticScreenReader.Announce(HintLabel.Text);
}

public async void StartGame(int width, int height, int cards) {
Board.Paused=true;
Board.Clear();
Board.MemoWidth=width;
Board.MemoHeight=height;
for(int i=0; i<cards; ++i) {
PlaySound("card_place");
Board.AddRandomCard(SoundsBase+"/"+(i+1).ToString("00"));
await Task.Delay(TimeSpan.FromMilliseconds(250));
}
Board.Paused=false;
}

void OnPick() {
if(Frag==0) {
Frag=1;
SetHint(AppResources.TutorialFirstPickHint);
}
}

void OnPair() {
if(Frag==1) {
Frag=2;
SetHint(AppResources.TutorialFirstPairHint);
StartGame(3, 3, 2);
} else if(Board.RemainingCards==0 && Frag==2) {
Frag=3;
SetHint(AppResources.TutorialThirdPairHint);
StartGame(4, 4, 5);
} else if(Board.RemainingCards==0 && Frag==3) {
SetHint(AppResources.TutorialEndHint);
}
}

void OnFail() {

}
}

}