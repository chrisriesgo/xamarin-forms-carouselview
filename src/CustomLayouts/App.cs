using System;

using Xamarin.Forms;

namespace CustomLayouts
{
	public class App : Application
	{
		public App()
		{
			// The root page of your application
			var navPage = new NavigationPage(new SwitcherPage());
			navPage.Icon = null;
			MainPage = navPage;
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

