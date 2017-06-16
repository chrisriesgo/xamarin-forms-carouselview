using System;
using System.Windows.Input;
using MvvmHelpers;
using Xamarin.Forms;

namespace CarouselLayout
{
	public class SwitcherPageViewModel : BaseViewModel
	{
		public SwitcherPageViewModel()
		{
			NoIndicatorCommand = new Command(async (obj) =>
			{
				await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new HomePage());
			});
			
			DotIndicatorCommand = new Command(async (obj) =>
			{
				await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new HomePageWithDots());
			});
			
			TabIndicatorCommand = new Command(async (obj) =>
			{
				await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new HomePageWithTabs());
			});
		}
		
		public ICommand NoIndicatorCommand { get; set; }
		public ICommand DotIndicatorCommand { get; set; }
		public ICommand TabIndicatorCommand { get; set; }
	}
}
