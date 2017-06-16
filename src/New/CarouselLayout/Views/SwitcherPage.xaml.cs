using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CarouselLayout
{
	public partial class SwitcherPage : ContentPage
	{
		public SwitcherPageViewModel ViewModel { get; set; }
		public SwitcherPage()
		{
			ViewModel = new SwitcherPageViewModel();
			InitializeComponent();
			BindingContext = ViewModel;
		}
	}
}
