using System;
using Xamarin.Forms;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace CustomLayouts
{
	public class PagerIndicatorTabs : Grid
	{
		int dotCount = 1;
		int _selectedIndex;

		public Color DotColor { get; set; }
		public double DotSize { get; set; }

		public PagerIndicatorTabs()
		{
			HorizontalOptions = LayoutOptions.CenterAndExpand;
			VerticalOptions = LayoutOptions.Center;
			DotColor = Color.Black;
			Device.OnPlatform(iOS: () => BackgroundColor = Color.Gray);

			var assembly = typeof(PagerIndicatorTabs).GetTypeInfo().Assembly;
			foreach (var res in assembly.GetManifestResourceNames())
				System.Diagnostics.Debug.WriteLine("found resource: " + res);
		}

		void CreateTabs()
		{

			if(Children != null && Children.Count > 0) Children.Clear();

			foreach(var item in ItemsSource)
			{

				var index = Children.Count;
				var tab = new StackLayout {
					Orientation = StackOrientation.Vertical,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					Padding = new Thickness(7),
				};
				Device.OnPlatform(
					iOS: () =>
					{
						tab.Children.Add(new Image { Source = "pin.png", HeightRequest = 20 });
						tab.Children.Add(new Label { Text = "Tab " + (index + 1), FontSize = 11 });
					},					
					Android: () =>
					{
						tab.Children.Add(new Image { Source = "pin.png", HeightRequest = 25 });
					}
				);
				var tgr = new TapGestureRecognizer();
				tgr.Command = new Command(() =>
				{
					SelectedItem = ItemsSource[index];
				});
				tab.GestureRecognizers.Add(tgr);
				Children.Add(tab, index, 0);
			}
		}

		public static BindableProperty ItemsSourceProperty =
			BindableProperty.Create<PagerIndicatorTabs, IList> (
				pi => pi.ItemsSource,
				null,
				BindingMode.OneWay,
				propertyChanging: (bindable, oldValue, newValue) => {
				((PagerIndicatorTabs)bindable).ItemsSourceChanging ();
			},
				propertyChanged: (bindable, oldValue, newValue) => {
				((PagerIndicatorTabs)bindable).ItemsSourceChanged ();
			}
		);

		public IList ItemsSource {
			get {
				return (IList)GetValue(ItemsSourceProperty);
			}
			set {
				SetValue (ItemsSourceProperty, value);
			}
		}

		public static BindableProperty SelectedItemProperty =
			BindableProperty.Create<PagerIndicatorTabs, object> (
				pi => pi.SelectedItem,
				null,
				BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) => {
				((PagerIndicatorTabs)bindable).SelectedItemChanged ();
			});

		public object SelectedItem {
			get {
				return GetValue (SelectedItemProperty);
			}
			set {
				SetValue (SelectedItemProperty, value);
			}
		}

		void ItemsSourceChanging ()
		{
			if (ItemsSource != null)
				_selectedIndex = ItemsSource.IndexOf (SelectedItem);
		}

		void ItemsSourceChanged ()
		{
			if (ItemsSource == null) return;

			CreateTabs();
		}

		void SelectedItemChanged () {

			var selectedIndex = ItemsSource.IndexOf (SelectedItem);
			var pagerIndicators = Children.Cast<StackLayout> ().ToList ();

			foreach(var pi in pagerIndicators)
			{
				UnselectTab(pi);
			}

			if(selectedIndex > -1)
			{
				SelectTab(pagerIndicators[selectedIndex]);
			}
		}

		static void UnselectTab (StackLayout tab)
		{
			tab.Opacity = 0.5;
		}

		static void SelectTab (StackLayout tab)
		{
			tab.Opacity = 1.0;
		}
	}
}

