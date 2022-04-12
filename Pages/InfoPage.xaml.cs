using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Memo.Resources;

namespace Memo {

public partial class InfoPage : ContentPage {

public InfoPage() {
InitializeComponent();
}

private async void OnLicenseClicked(object sender, EventArgs e) {
await Launcher.OpenAsync("https://www.gnu.org/licenses/gpl-3.0.html");
}

private async void OnGithubClicked(object sender, EventArgs e) {
await Launcher.OpenAsync("https://github.com/dawidpieper/memo");
}

private async void OnClearDataClicked(object sender, EventArgs e) {
var r = await DisplayAlert(AppResources.WarningTitle, AppResources.ClearDataLabel, AppResources.YesButton, AppResources.NoButton);
if(r) Preferences.Clear();
}
}
}