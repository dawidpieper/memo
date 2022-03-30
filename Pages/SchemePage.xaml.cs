using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;

namespace Memo {

public partial class SchemePage : ContentPage {

string[] SoundSchemes = {"birds", "piano"};

	public SchemePage() {
		InitializeComponent();
SoundsView.ItemsSource=SoundSchemes;
	}

	private async void OnSchemeSelected(object sender, SelectionChangedEventArgs e) {
if(e.CurrentSelection!=null && e.CurrentSelection.Count==0) return;
var g = new GamePage((string)(e.CurrentSelection.First()));
await Navigation.PushAsync(g);
SoundsView.SelectedItem=null;
	}
}


}