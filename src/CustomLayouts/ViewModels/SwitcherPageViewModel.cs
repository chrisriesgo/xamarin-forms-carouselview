using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CustomLayouts.ViewModels
{
	public class SwitcherPageViewModel : BaseViewModel
	{
		public SwitcherPageViewModel()
		{
			Pages = new List<HomeViewModel>() {
				new HomeViewModel { Title = "1", Background = Color.White, ImageSource = "icon.png" },
				new HomeViewModel { Title = "2", Background = Color.Red, ImageSource = "icon.png" },
				new HomeViewModel { Title = "3", Background = Color.Blue, ImageSource = "icon.png" },
				new HomeViewModel { Title = "4", Background = Color.Yellow, ImageSource = "icon.png" },
			};

			CurrentPage = Pages.First();
		}

		IEnumerable<HomeViewModel> _pages;
		public IEnumerable<HomeViewModel> Pages {
			get {
				return _pages;
			}
			set {
				SetObservableProperty (ref _pages, value);
				CurrentPage = Pages.FirstOrDefault ();
			}
		}

		HomeViewModel _currentPage;
		public HomeViewModel CurrentPage {
			get {
				return _currentPage;
			}
			set {
				SetObservableProperty (ref _currentPage, value);
			}
		}
	}

	public class HomeViewModel : BaseViewModel, ITabProvider
	{
		public HomeViewModel() {}

		public string Title { get; set; }
		public Color Background { get; set; }
		public string ImageSource { get; set; }
	}
}

