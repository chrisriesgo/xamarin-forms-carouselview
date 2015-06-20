using System;
using Xamarin.Forms;
using CustomLayouts.Controls;
using CustomLayouts.ViewModels;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace CustomLayouts
{
	public class HomePage : ContentPage
	{
		View _tabs;

		RelativeLayout relativeLayout;

		CarouselLayout.IndicatorStyleEnum _indicatorStyle;

		SwitcherPageViewModel viewModel;

		public HomePage(CarouselLayout.IndicatorStyleEnum indicatorStyle)
		{
			_indicatorStyle = indicatorStyle;

			viewModel = new SwitcherPageViewModel();
			BindingContext = viewModel;

			Title = _indicatorStyle.ToString();

			relativeLayout = new RelativeLayout 
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			var currentSessionsCarousel = CreateCurrentSessionsCarousel();
			var dots = CreatePagerIndicatorContainer();
//			_tabs = CreateTabsContainer();
			_tabs = CreateTabs();

			switch(currentSessionsCarousel.IndicatorStyle)
			{
				case CarouselLayout.IndicatorStyleEnum.Dots:
					relativeLayout.Children.Add (currentSessionsCarousel,
						Constraint.RelativeToParent ((parent) => { return parent.X; }),
						Constraint.RelativeToParent ((parent) => { return parent.Y; }),
						Constraint.RelativeToParent ((parent) => { return parent.Width; }),
						Constraint.RelativeToParent ((parent) => { return parent.Height/2; })
					);

					relativeLayout.Children.Add (dots, 
						Constraint.Constant (0),
						Constraint.RelativeToView (currentSessionsCarousel, 
							(parent,sibling) => { return sibling.Height - 18; }),
						Constraint.RelativeToParent (parent => parent.Width),
						Constraint.Constant (18)
					);
					break;
				case CarouselLayout.IndicatorStyleEnum.Tabs:
					var tabsHeight = 50;
					relativeLayout.Children.Add (_tabs, 
						Constraint.Constant (0),
						Constraint.RelativeToParent ((parent) => { return parent.Height - tabsHeight; }),
						Constraint.RelativeToParent (parent => parent.Width),
						Constraint.Constant (tabsHeight)
					);

					relativeLayout.Children.Add (currentSessionsCarousel,
						Constraint.RelativeToParent ((parent) => { return parent.X; }),
						Constraint.RelativeToParent ((parent) => { return parent.Y; }),
						Constraint.RelativeToParent ((parent) => { return parent.Width; }),
						Constraint.RelativeToView (_tabs, (parent, sibling) => { return parent.Height - (sibling.Height); })
					);
					break;
				default:
					relativeLayout.Children.Add (currentSessionsCarousel,
						Constraint.RelativeToParent ((parent) => { return parent.X; }),
						Constraint.RelativeToParent ((parent) => { return parent.Y; }),
						Constraint.RelativeToParent ((parent) => { return parent.Width; }),
						Constraint.RelativeToParent ((parent) => { return parent.Height; })
					);
					break;
			}

			Content = relativeLayout;
		}

		CarouselLayout CreateCurrentSessionsCarousel ()
		{
			var carousel = new CarouselLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				IndicatorStyle = _indicatorStyle,
				ItemTemplate = new DataTemplate(typeof(HomeView))
			};
			carousel.SetBinding(CarouselLayout.ItemsSourceProperty, "CurrentSessions");
			carousel.SetBinding(CarouselLayout.SelectedItemProperty, "CurrentSession", BindingMode.TwoWay);

			return carousel;
		}

		View CreatePagerIndicatorContainer()
		{
			return new StackLayout {
				Children = { CreatePagerIndicators() }
			};
		}

		View CreatePagerIndicators()
		{
			var pagerIndicator = new PagerIndicatorDots() { DotSize = 5, DotColor = Color.Black };
			pagerIndicator.SetBinding (PagerIndicatorDots.ItemsSourceProperty, "CurrentSessions");
			pagerIndicator.SetBinding (PagerIndicatorDots.SelectedItemProperty, "CurrentSession");
			return pagerIndicator;
		}

		View CreateTabsContainer()
		{
			return new StackLayout {
				Children = { CreateTabs() }
			};
		}

		View CreateTabs()
		{
			var pagerIndicator = new PagerIndicatorTabs() { HorizontalOptions = LayoutOptions.CenterAndExpand };
			pagerIndicator.RowDefinitions.Add(new RowDefinition() { Height = 50 });
			pagerIndicator.SetBinding(PagerIndicatorTabs.ColumnDefinitionsProperty, "CurrentSessions", BindingMode.Default, new SpacingConverter());
			pagerIndicator.SetBinding (PagerIndicatorTabs.ItemsSourceProperty, "CurrentSessions");
			pagerIndicator.SetBinding (PagerIndicatorTabs.SelectedItemProperty, "CurrentSession");

//			grid.Children.Add(new StackLayout {
//				Orientation = StackOrientation.Vertical,
//				HorizontalOptions = LayoutOptions.Center,
//				Padding = new Thickness(5),
//				Children = {
//					new Image { Source = "icon.png", HeightRequest = 20 },
//					new Label { Text = "Tab One", FontSize = 11 }
//				}
//			}, 0, 0);
//			grid.Children.Add(new StackLayout {
//				Orientation = StackOrientation.Vertical,
//				HorizontalOptions = LayoutOptions.Center,
//				Padding = new Thickness(5),
//				Children = {
//					new Image { Source = "icon.png", HeightRequest = 20 },
//					new Label { Text = "Tab Two", FontSize = 11 }
//				}
//			}, 1, 0);
//			grid.Children.Add(new StackLayout {
//				Orientation = StackOrientation.Vertical,
//				HorizontalOptions = LayoutOptions.Center,
//				Padding = new Thickness(5),
//				Children = {
//					new Image { Source = "icon.png", HeightRequest = 20 },
//					new Label { Text = "Tab Three", FontSize = 11 }
//				}
//			}, 2, 0);
//			grid.Children.Add(new StackLayout {
//				Orientation = StackOrientation.Vertical,
//				HorizontalOptions = LayoutOptions.Center,
//				Padding = new Thickness(5),
//				Children = {
//					new Image { Source = "icon.png", HeightRequest = 20 },
//					new Label { Text = "Tab Four", FontSize = 11 }
//				}
//			}, 3, 0);

			return pagerIndicator;

//			var pagerIndicator = new PagerIndicatorDots() { DotSize = 10, DotColor = Color.White, Spacing = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
//
//			pagerIndicator.SetBinding (PagerIndicatorDots.ItemsSourceProperty, "CurrentSessions");
//			pagerIndicator.SetBinding (PagerIndicatorDots.SelectedItemProperty, "CurrentSession");
//
//			return pagerIndicator;
		}
	}

	public class SpacingConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var items = value as IEnumerable<HomeViewModel>;

			var collection = new ColumnDefinitionCollection();
			foreach(var item in items)
			{
				collection.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
			}
			return collection;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class PipSet : StackLayout
	{
		int _selectedIndex;

		public PipSet ()
		{
			Orientation = StackOrientation.Horizontal;
			HorizontalOptions = LayoutOptions.Center;
			Spacing = 4;
		}

		public static BindableProperty ItemsSourceProperty =
			BindableProperty.Create<PipSet, IList> (
				pips => pips.ItemsSource,
				null,
				BindingMode.OneWay,
				propertyChanging: (bindable, oldValue, newValue) => {
				((PipSet)bindable).ItemsSourceChanging ();
			},
				propertyChanged: (bindable, oldValue, newValue) => {
				((PipSet)bindable).ItemsSourceChanged ();
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
			BindableProperty.Create<PipSet, object> (
				pips => pips.SelectedItem,
				null,
				BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) => {
				((PipSet)bindable).SelectedItemChanged ();
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

			var countDelta = ItemsSource.Count - Children.Count;

			if (countDelta > 0) {
				for (var i = 0; i < countDelta; i++) {
					Children.Add (CreatePip ());
				}
			} else if (countDelta < 0) {
				for (var i = 0; i < -countDelta; i++) {
					Children.RemoveAt (0);
				}
			}

			//            if (_selectedIndex >= 0 && _selectedIndex < ItemsSource.Count)
			//                SelectedItem = ItemsSource [_selectedIndex];

			//            UpdateSelection ();
		}

		void SelectedItemChanged () {
			var selectedIndex = ItemsSource.IndexOf (SelectedItem);
			var pips = Children.Cast<Image> ().ToList ();

			foreach (var pip in pips) UnselectPip (pip);

			if (selectedIndex > -1) SelectPip (pips [selectedIndex]);
		}

		static View CreatePip ()
		{
			return new Image { Source = "pip.png" };
		}

		static void UnselectPip (Image pip)
		{
			pip.Source = "pip.png";
		}

		static void SelectPip (Image pip)
		{
			pip.Source = "pip_selected.png";
		}
	}
}

