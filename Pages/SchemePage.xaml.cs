using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Memo.Resources;

namespace Memo {

public class SoundScheme {
public string Dir {get;set;}
public string Name {get;set;}
public Score SchemeScore {get;set;}
public bool Available {get;set;}
public SoundScheme(string dir, string name, bool available) {
Dir=dir;
Name=name;
Available=available;
SchemeScore = Scores.GetScore(Dir);
}
}

public partial class SchemePage : ContentPage {

List<SoundScheme> SoundSchemes;

	public SchemePage() {
		InitializeComponent();
Appearing += (sender, e) => {
SoundSchemes = new List<SoundScheme>();
SoundSchemes.Add(new SoundScheme("birds", AppResources.BirdsSoundScheme, true));
SoundSchemes.Add(new SoundScheme("piano", AppResources.PianoSoundScheme, SoundSchemes[SoundSchemes.Count-1].SchemeScore.Wins>0));
SoundSchemes.Add(new SoundScheme("instruments", AppResources.InstrumentsSoundScheme, SoundSchemes[SoundSchemes.Count-1].SchemeScore.Wins>0));
SoundsView.ItemsSource=SoundSchemes;
};
	}

	private async void OnSchemeSelected(object sender, SelectionChangedEventArgs e) {
if(e.CurrentSelection!=null && e.CurrentSelection.Count==0) return;
SoundScheme scheme = (SoundScheme)e.CurrentSelection.First();
if(scheme.Available) {
var g = new GamePage(scheme.Dir);
await Navigation.PushAsync(g);
}
SoundsView.SelectedItem=null;
	}
}


}