using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CarouselLayout
{
	public partial class HomePage : ContentPage
	{
		public HomePageViewModel ViewModel { get; set; }
		
		public HomePage()
		{
			ViewModel = new HomePageViewModel();
			InitializeComponent();
			BindingContext = ViewModel;
		}

		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			//TODO: replace with event to command binding using behaviors
			if (e?.SelectedItem is CarouselItem item)
			{
				ViewModel.SelectedItem = item;
			}
		}
	}
}
