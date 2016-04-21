using System;
using Xamarin.Forms;
using CustomLayouts.Controls;

namespace CustomLayouts
{
	public class SwitcherPage : ContentPage
	{
		public SwitchCell scrollOrientationSwitch;

		public SwitcherPage()
		{
			Title = "Pager Layout w/ Indicators";

			var none = new Button {
				HorizontalOptions = LayoutOptions.Center,
				Text = "No pager indicator",
				Command = new Command((obj) => Navigation.PushAsync(new HomePage(CarouselLayout.IndicatorStyleEnum.None, scrollOrientationSwitch.On ? ScrollOrientation.Horizontal : ScrollOrientation.Vertical)))
			};
			var dots = new Button {
				HorizontalOptions = LayoutOptions.Center,
				Text = "Dots",
				Command = new Command((obj) => Navigation.PushAsync(new HomePage(CarouselLayout.IndicatorStyleEnum.Dots, scrollOrientationSwitch.On ? ScrollOrientation.Horizontal : ScrollOrientation.Vertical)))
			};
			var tabs = new Button {
				HorizontalOptions = LayoutOptions.Center,
				Text = "Tabs",
				Command = new Command((obj) => Navigation.PushAsync(new HomePage(CarouselLayout.IndicatorStyleEnum.Tabs, scrollOrientationSwitch.On ? ScrollOrientation.Horizontal : ScrollOrientation.Vertical)))
			};
			scrollOrientationSwitch = new SwitchCell() { Text = "Vertical/Horizontal", On = true };
			var table = new TableView();
			table.Intent = TableIntent.Settings;
			table.Root = new TableRoot() {
				new TableSection("Orientation") {
					scrollOrientationSwitch
				}
			};
			Content = new StackLayout {
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.Center,
				Spacing = 20,
				Children = {
					none,
					dots,
					tabs,
					table
				}
			};
		}
	}
}

