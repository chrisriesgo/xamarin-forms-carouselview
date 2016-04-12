using System;
using Xamarin.Forms;
using CustomLayouts.Controls;
using Plugin.Settings;

namespace CustomLayouts
{
	public class SwitcherPage : ContentPage
	{
        public SwitchCell orientationSwitch;

        public SwitcherPage()
		{
			Title = "Pager Layout w/ Indicators";

            var none = new Button
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "No pager indicator",
                Command = new Command((obj) =>
                {
                    CrossSettings.Current.AddOrUpdateValue("IsVertical", !orientationSwitch.On);
                    Navigation.PushAsync(new HomePage(CarouselLayout.IndicatorStyleEnum.None));
                })
            };
            var dots = new Button
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Dots",
                Command = new Command((obj) =>
                {
                    CrossSettings.Current.AddOrUpdateValue("IsVertical", !orientationSwitch.On);
                    Navigation.PushAsync(new HomePage(CarouselLayout.IndicatorStyleEnum.Dots));
                })
            };
            var tabs = new Button
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Tabs",
                Command = new Command((obj) =>
                {
                    CrossSettings.Current.AddOrUpdateValue("IsVertical", !orientationSwitch.On);
                    Navigation.PushAsync(new HomePage(CarouselLayout.IndicatorStyleEnum.Tabs));
                })
            };
            orientationSwitch = new SwitchCell() { Text = "Vertical/Horizontal", On = true };
            var table = new TableView();
            table.Intent = TableIntent.Settings;
            table.Root = new TableRoot() {
                new TableSection("Scroll Orientation") {
                    orientationSwitch
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

