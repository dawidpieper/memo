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

public partial class GamePage : ContentPage {

int RemainingTime=0;
int Round=0;
int Ticks=0;
int Points=0;
bool LastSuccess=false;
bool Completed=false;

string SoundsBase;

public GamePage(string soundsBase="birds") {
InitializeComponent();
SoundsBase=soundsBase;
Disappearing += (sender, e) => Board.Paused=true;
Board.Pick += (sender, e) => {
PlaySound("card_pick");
PlaySound(e.CardPicked.Sound);;
};
Board.Fail += (sender, e) => {
LastSuccess=false;
PlaySound("card_fail");
};
Board.Pair += (sender, e) => {
Points+=10;
PlaySound("card_pair");
if(LastSuccess && !e.IsLastCard) {
RemainingTime+=15;
SemanticScreenReader.Announce(AppResources.BonusTimeAlert);
}
LastSuccess=true;
if(e.IsLastCard) {
LastSuccess=false;
PlaySound("nextround");
Points+=RemainingTime;
NextRound(3);
}
};
var ticker = Dispatcher.CreateTimer();
ticker.Interval = TimeSpan.FromSeconds(1);
ticker.IsRepeating=true;
ticker.Tick += (sender, e) => Tick();
ticker.Start();
Board.Paused=false;
NextRound();
}

public void Tick() {
if(!Board.Paused) {
--RemainingTime;
if(RemainingTime>0 && RemainingTime%10==0)
SemanticScreenReader.Announce(RemainingTime.ToString()+" "+AppResources.SecondsAlert);
PointsLabel.Text=AppResources.PointsLabel+": "+Points.ToString();
if(Board.RemainingCards==0)
CardsLabel.Text="";
else
CardsLabel.Text=AppResources.RemainingCardsLabel+": "+Board.RemainingCards.ToString();
var sb = new StringBuilder();
sb.Append((RemainingTime/60).ToString("00"));
sb.Append(":");
sb.Append((RemainingTime%60).ToString("00"));
TimerLabel.Text=sb.ToString();
++Ticks;
if(Ticks%2==0 && Board.IsPicked) PlaySound("card_picked");
if(RemainingTime==0) {
PlaySound("over");
SemanticScreenReader.Announce(AppResources.TimeEndedAlert);
Board.Paused=true;
Completed=true;
Scores.AddScore(SoundsBase, Points, Round-1, false);
}
}
}

public async void NextRound(int t=0) {
if(t>0) {
Board.Paused=true;
await Task.Delay(t*1000);
Board.Paused=false;
}
++Round;
StartRound(Round);
}

public void StartRound(int i) {
RoundLabel.Text=AppResources.RoundLabel+" "+i.ToString();
switch(i) {
case 0:
case 1:
StartGame(3, 3, 2, 30);
break;
case 2:
StartGame(3, 3, 3, 45);
break;
case 3:
StartGame(3, 4, 4, 50);
break;
case 4:
StartGame(4, 4, 5, 60);
break;
case 5:
StartGame(4, 5, 6, 75);
break;
case 6:
StartGame(5, 5, 7, 90);
break;
case 7:
StartGame(5, 5, 8, 100);
break;
case 8:
Board.Paused=true;
PlaySound("success");
RoundLabel.Text=AppResources.GameEndedLabel;
CardsLabel.Text="";
Completed=true;
Scores.AddScore(SoundsBase, Points, Round-1, true);
break;
}
SemanticScreenReader.Announce(RoundLabel.Text);
}

public async void StartGame(int width, int height, int cards, int time) {
Board.Paused=true;
Board.Clear();
Board.MemoWidth=width;
Board.MemoHeight=height;
for(int i=0; i<cards; ++i) {
PlaySound("card_place");
Board.AddRandomCard(SoundsBase+"/"+(i+1).ToString("00"));
await Task.Delay(TimeSpan.FromMilliseconds(250));
}
RemainingTime=time;
SemanticScreenReader.Announce(RemainingTime.ToString()+" sek.");
Board.Paused=false;
}

protected override bool OnBackButtonPressed(){
if(!Completed) {
MainThread.BeginInvokeOnMainThread(async () => {
Board.Paused=true;
var r = await DisplayAlert(AppResources.DoAbortGameTitle, AppResources.GameWillBeLostLabel, AppResources.YesButton, AppResources.NoButton);
Board.Paused=false;
if(r) {
base.OnBackButtonPressed();
await Navigation.PopAsync();
}
});
return true;
} else
return false;
}
}
}
