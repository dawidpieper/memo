using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Memo.Resources;

namespace Memo.Controls {
public class MemoBoard : ContentView {

public class CardPickEventArgs : EventArgs {
public Card CardPicked {get;set;}
public int X {get;set;}
public int Y {get;set;}
}

public class CardFailEventArgs : EventArgs {
public Card Card1 {get;set;}
public Card Card2 {get;set;}
}

public class CardPairEventArgs : EventArgs {
public Card CardPaired {get;set;}
public bool IsLastCard {get;set;}
}

public class Card {
public int X1, Y1, X2, Y2;
public string Sound;
public Card(int x1, int y1, int x2, int y2, string sound) {
X1=x1;
Y1=y1;
X2=x2;
Y2=y2;
Sound=sound;
}
public bool LocatedAt(int x, int y) {
return (X1==x&&Y1==y)||(X2==x&&Y2==y);
}

public override int GetHashCode() => X1*256*256*256+Y1*256*256+X2*256+Y2;
public override bool Equals(object o) {
if(!(o is Card)) return false;
Card c = (Card)o;
return (c.X1==X1 && c.Y1==Y1 && c.X2==X2 && c.Y2==Y2);
}

public static bool operator ==(Card c1, Card c2) => c1.Equals(c2);
public static bool operator !=(Card c1, Card c2) => !c1.Equals(c2);
}

List<Card> cards;
Random random;
(int,int) PickedCard;

public event EventHandler<CardPickEventArgs> Pick;
public event EventHandler<CardFailEventArgs> Fail;
public event EventHandler<CardPairEventArgs> Pair;

Grid grid;

public static readonly BindableProperty MemoWidthProperty = BindableProperty.Create("MemoWidth", typeof(int), typeof(MemoBoard), 3);
public static readonly BindableProperty MemoHeightProperty = BindableProperty.Create("MemoHeight", typeof(int), typeof(MemoBoard), 3);
public static readonly BindableProperty PausedProperty = BindableProperty.Create("MemoHeight", typeof(bool), typeof(MemoBoard), true);
public int MemoWidth {
get => (int)GetValue(MemoWidthProperty);
set => SetValue(MemoWidthProperty, value);
    }

public int MemoHeight {
get => (int)GetValue(MemoHeightProperty);
set => SetValue(MemoHeightProperty, value);
    }

public bool Paused {
get => (bool)GetValue(PausedProperty);
set => SetValue(PausedProperty, value);
    }

public MemoBoard() : base() {
cards = new List<Card>();
random = new Random();
PickedCard=(-1,-1);

grid = new Grid();
Content=grid;
}

public void Rebuild() {
if(grid==null) return;
grid.Children.Clear();
grid.RowDefinitions.Clear();
grid.ColumnDefinitions.Clear();
for(int i=0; i<MemoWidth; ++i) {
grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star)});
for(int j=0; j<MemoHeight; ++j) {
if(i==0) grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)});
var b = new Button();
SemanticProperties.SetDescription(b, GetFieldLabel(i,j));
b.ImageSource = GetFieldImage(i,j);
int x=i;
int y=j;
b.Clicked += (sender, e) => {
if(!Paused) PickCard(x,y);
};
grid.Children.Add(b);
grid.SetRow(b, i);
grid.SetColumn(b, j);
}
}
}

public void Refresh() {
if(grid.RowDefinitions.Count!=MemoWidth || grid.ColumnDefinitions.Count!=MemoHeight) {
Rebuild();
} else {
for(int i=0; i<MemoWidth; ++i)
for(int j=0; j<MemoHeight; ++j)
foreach(var c in grid.Children) {
if(grid.GetRow(c)==i && grid.GetColumn(c)==j) {
Button b = (Button)c;
SemanticProperties.SetDescription(b, GetFieldLabel(i,j));
b.ImageSource = GetFieldImage(i,j);
}
}
}
}

protected override void OnBindingContextChanged () {
base.OnBindingContextChanged ();
if (BindingContext != null) {
Rebuild();
}
}

protected override void OnPropertyChanged(string t) {
base.OnPropertyChanged(t);
Rebuild();
}

public Card GetCardAt(int x, int y) {
if(cards==null) return null;
foreach(var c in cards)
if(c.LocatedAt(x,y)) return c;
return null;
}

public bool IsCardAt(int x, int y) {
foreach(var c in cards)
if(c.LocatedAt(x,y)) return true;
return false;
}

public string GetFieldLabel(int x, int y) {
string letter = ((char)('A'+x)).ToString();
var sb = new StringBuilder();
if(IsCardAt(x,y) && PickedCard!=(x,y)) sb.Append(AppResources.CardField);
else if(PickedCard==(x,y)) sb.Append(AppResources.CardPickedField);
else sb.Append(AppResources.EmptyField);
sb.Append(": ");
sb.Append(letter);
sb.Append(y+1);
return sb.ToString();
}

public ImageSource GetFieldImage(int x, int y) {
if(IsCardAt(x,y) && PickedCard!=(x,y))
return new FileImageSource{File = "field_card.svg"};
else if(PickedCard==(x,y))
return new FileImageSource{File = "field_cardpicked.svg"};
else
return new FileImageSource{File = "field_empty.svg"};
}

public bool AddCard(int x1, int y1, int x2, int y2, string sound) {
if(IsCardAt(x1,y1) || IsCardAt(x2,y2)) return false;
cards.Add(new Card(x1, y1, x2, y2, sound));
Refresh();
return true;
}

public bool RemoveCard(int x, int y) {
foreach(var c in cards)
if(c.LocatedAt(x,y)) {
cards.Remove(c);
Refresh();
return true;
}
return false;
}

public bool AddRandomCard(string sound) {
if(cards.Count>=MemoWidth*MemoHeight/2) return false;
int x1, y1, x2, y2;
do {
x1=random.Next(MemoWidth);
y1=random.Next(MemoHeight);
x2=random.Next(MemoWidth);
y2=random.Next(MemoHeight);
} while(x1==x2 || y1==y2 || IsCardAt(x1,y1) || IsCardAt(x2,y2));
AddCard(x1, y1, x2, y2, sound);
return true;
}

public int RemainingCards {get => cards.Count;}

public bool IsPicked {get => PickedCard!=(-1,-1);}
public bool PickCard(int x, int y) {
if(PickedCard==(x,y)) return false;
if(!IsCardAt(x,y)) return false;
var e = new CardPickEventArgs();
e.CardPicked=GetCardAt(x,y);
e.X=x;
e.Y=y;
Pick?.Invoke(this, e);
if(PickedCard==(-1,-1)) {
PickedCard=(x,y);
Refresh();
} else {
var pc = GetCardAt(PickedCard.Item1, PickedCard.Item2);
var c = GetCardAt(x, y);
PickedCard=(-1,-1);
Refresh();
if(pc==null || c==null) return false;
if(pc==c) {
RemoveCard(x,y);
var ep = new CardPairEventArgs();
ep.CardPaired=c;
ep.IsLastCard=(RemainingCards==0);
Pair?.Invoke(this, ep);
} else {
var ef = new CardFailEventArgs();
ef.Card1=pc;
ef.Card2=c;
Fail?.Invoke(this, ef);
}
}
return true;
}

public void Clear() {
cards.Clear();
Refresh();
}
}
}