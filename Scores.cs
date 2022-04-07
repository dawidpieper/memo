using System;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Essentials;

namespace Memo {

public class Score {
public int Max {get;set;}
public double Average {get;set;}
public int MaxRounds {get;set;}
public double AverageRounds {get;set;}
public int Count {get;set;}
public int Wins {get;set;}
}

public static class Scores {

public static int GetMaxScore(string scheme) {
var sb = new StringBuilder();
sb.Append("Score.");
sb.Append(scheme);
sb.Append(".Max");
return (int)Preferences.Get(sb.ToString(), 0);
}

public static double GetAverageScore(string scheme) {
var sb = new StringBuilder();
sb.Append("Score.");
sb.Append(scheme);
sb.Append(".Average");
return (double)Preferences.Get(sb.ToString(), 0);
}

public static int GetMaxRounds(string scheme) {
var sb = new StringBuilder();
sb.Append("Score.");
sb.Append(scheme);
sb.Append(".MaxRounds");
return (int)Preferences.Get(sb.ToString(), 0);
}

public static double GetAverageRounds(string scheme) {
var sb = new StringBuilder();
sb.Append("Score.");
sb.Append(scheme);
sb.Append(".AverageRounds");
return (double)Preferences.Get(sb.ToString(), 0);
}

public static int GetCount(string scheme) {
var sb = new StringBuilder();
sb.Append("Score.");
sb.Append(scheme);
sb.Append(".Count");
return (int)Preferences.Get(sb.ToString(), 0);
}

public static int GetWins(string scheme) {
var sb = new StringBuilder();
sb.Append("Score.");
sb.Append(scheme);
sb.Append(".Wins");
return (int)Preferences.Get(sb.ToString(), 0);
}

public static Score GetScore(string scheme) {
var score = new Score();
score.Max = GetMaxScore(scheme);
score.Average = GetAverageScore(scheme);
score.MaxRounds = GetMaxRounds(scheme);
score.AverageRounds = GetAverageRounds(scheme);
score.Count = GetCount(scheme);
score.Wins = GetWins(scheme);
return score;
}

public static void AddScore(string scheme, int points, int round, bool win) {
var score = GetScore(scheme);
StringBuilder sb;
if(points>score.Max) {
sb = new StringBuilder();
sb.Append("Score.");
sb.Append(scheme);
sb.Append(".Max");
Preferences.Set(sb.ToString(), points);
}
double average = (score.Average*score.Count+points)/(double)(score.Count+1);
sb = new StringBuilder();
sb.Append("Score.");
sb.Append(scheme);
sb.Append(".Average");
Preferences.Set(sb.ToString(), average);
if(round>score.MaxRounds) {
sb = new StringBuilder();
sb.Append("Score.");
sb.Append(scheme);
sb.Append(".MaxRounds");
Preferences.Set(sb.ToString(), round);
}
double averagerounds = (score.AverageRounds*score.Count+round)/(double)(score.Count+1);
sb = new StringBuilder();
sb.Append("Score.");
sb.Append(scheme);
sb.Append(".AverageRounds");
Preferences.Set(sb.ToString(), averagerounds);
sb = new StringBuilder();
sb.Append("Score.");
sb.Append(scheme);
sb.Append(".Count");
Preferences.Set(sb.ToString(), score.Count+1);
if(win) {
sb = new StringBuilder();
sb.Append("Score.");
sb.Append(scheme);
sb.Append(".Wins");
Preferences.Set(sb.ToString(), score.Wins+1);
}
}
}
}