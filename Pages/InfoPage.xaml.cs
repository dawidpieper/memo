using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;

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

}
}