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
			CurrentSessions = new List<HomeViewModel>() {
				new HomeViewModel { Title = "1", Background = Color.White, ImageSource = "icon.png" },
				new HomeViewModel { Title = "2", Background = Color.Red, ImageSource = "icon.png" },
				new HomeViewModel { Title = "3", Background = Color.Blue, ImageSource = "icon.png" },
				new HomeViewModel { Title = "4", Background = Color.Yellow, ImageSource = "icon.png" },
			};

			CurrentSession = CurrentSessions.First();
		}

		IEnumerable<HomeViewModel> _currentSessions;
		public IEnumerable<HomeViewModel> CurrentSessions {
			get {
				return _currentSessions;
			}
			set {
				SetObservableProperty (ref _currentSessions, value);
				CurrentSession = CurrentSessions.FirstOrDefault ();
			}
		}

		HomeViewModel _currentSession;
		public HomeViewModel CurrentSession {
			get {
				return _currentSession;
			}
			set {
				SetObservableProperty (ref _currentSession, value);
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

