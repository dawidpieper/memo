using System;
using System.Collections.Generic;
using System.Resources;
using System.Globalization;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Essentials;
using Application = Microsoft.Maui.Controls.Application;

namespace Memo {

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		MainPage = new NavigationPage(new MainPage());
	}
}
}