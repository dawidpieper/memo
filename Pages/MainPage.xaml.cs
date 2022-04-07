using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;

namespace Memo {

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

private async void OnStartClicked(object sender, EventArgs e) {
var g = new SchemePage();
await Navigation.PushAsync(g);
}

private async void OnHelpClicked(object sender, EventArgs e) {
await Navigation.PushAsync(new TutorialPage());
}

private async void OnInfoClicked(object sender, EventArgs e) {
await Navigation.PushAsync(new InfoPage());
}
}


}