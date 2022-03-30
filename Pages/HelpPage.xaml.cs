using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using static Memo.Player;

namespace Memo {

public partial class HelpPage : ContentPage {

int Round=0;
int Ticks=0;
bool LastSuccess=false;

string SoundsBase;

public HelpPage() {
InitializeComponent();
SoundsBase="birds";
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
PlaySound("card_pair");
if(LastSuccess) {
SemanticScreenReader.Announce("Dodatkowy czas!");
}
LastSuccess=true;
if(e.IsLastCard) {
LastSuccess=false;
PlaySound("success");
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
++Ticks;
if(Ticks%2==0 && Board.IsPicked) PlaySound("card_picked");
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
switch(i) {
case 0:
case 1:
StartGame(2, 2, 1, @"Witaj!
Gra Memo jest dźwiękowym odpowiednikiem znanej i lubianej gry Memory.
Celem gry jest zbieranie par kart.

Na ekranie wyświetlana jest teraz plansza składająca się z czterech pól. Dwa z nich są puste, na dwóch znajduje się karta.
Kiedy podniesiesz kartę, usłyszysz dźwięk symbolizujący jej zawartość. Celem jest podniesienie jednocześnie dwóch kart o tym samym dźwięku. Spróbuj!");
break;
case 2:
StartGame(2, 2, 2, @"Kiedy podniesiesz różne od siebie karty, powrócą one na swoje miejsca.
Zapamiętanie, gdzie znajduje się jaka karta, ma swoje zalety. Nie tylko pozwala przyspieszyć grę, a to ważne, bo czas nie jest nieograniczony, ale podniesienie kolejnej poprawnej pary pod rząd spowoduje przyznanie dodatkowego czasu.");
break;
case 3:
StartGame(3, 4, 4, @"Plansze mogą być różnych wymiarów, także prostokątnych.
Ta plansza ma 3 pola szerokości i 4 wysokości, a umieszczono na niej aż 4 pary kart. Dasz radę?");
break;
case 4:
StartGame(4, 4, 5, @"W trakcie gry zliczane są punkty.
Pod koniec każdej rundy pozostały czas jest przeliczany na punkty: każda sekunda to jeden punkt. Dodatkowo za każdą zebraną parę uzyskać 10 punktów.
To jak, ostatni, ekstremalny test?");
break;
case 5:
Board.Paused=true;
HintLabel.Text="To już koniec. Zapraszamy do gry!";
SemanticScreenReader.Announce(HintLabel.Text);
break;
}
}

public async void StartGame(int width, int height, int cards, string hint) {
Board.Paused=true;
Board.Clear();
Board.MemoWidth=width;
Board.MemoHeight=height;
for(int i=0; i<cards; ++i) {
PlaySound("card_place");
Board.AddRandomCard(SoundsBase+"/"+(i+1).ToString("00"));
await Task.Delay(TimeSpan.FromMilliseconds(250));
}
HintLabel.Text=hint;
SemanticScreenReader.Announce(hint);
Board.Paused=false;
}
}
}
