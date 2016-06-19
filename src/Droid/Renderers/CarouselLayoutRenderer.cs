using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Reflection;
using Java.Lang;
using System.Timers;
using Android.Widget;
using Android.Views;
using System.ComponentModel;
using Android.Graphics;
using CustomLayouts.Controls;
using CustomLayouts.Droid.Renderers;

[assembly:ExportRenderer(typeof(CarouselLayout), typeof(CarouselLayoutRenderer))]

namespace CustomLayouts.Droid.Renderers
{
	public class CarouselLayoutRenderer : ScrollViewRenderer {
		int _prevScrollX;
		int _deltaX;
		bool _motionDown;
		Timer _deltaXResetTimer;
		Timer _scrollStopTimer;
		HorizontalScrollView _scrollView;

		protected override void OnElementChanged (VisualElementChangedEventArgs e)
		{
			base.OnElementChanged (e);
			if(e.NewElement == null) return;

			_deltaXResetTimer = new Timer(100) { AutoReset = false };
			_deltaXResetTimer.Elapsed += (object sender, ElapsedEventArgs args) => _deltaX = 0;

			_scrollStopTimer = new Timer (200) { AutoReset = false };
			_scrollStopTimer.Elapsed += (object sender, ElapsedEventArgs args2) => UpdateSelectedIndex ();

			e.NewElement.PropertyChanged += ElementPropertyChanged;
		}

		void ElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == "Renderer") {
				_scrollView = (HorizontalScrollView)typeof(ScrollViewRenderer)
					.GetField ("_hScrollView", BindingFlags.NonPublic | BindingFlags.Instance)
					.GetValue (this);

				_scrollView.HorizontalScrollBarEnabled = false;
				_scrollView.Touch += HScrollViewTouch;
			}
			if (e.PropertyName == CarouselLayout.SelectedIndexProperty.PropertyName && !_motionDown) {
				ScrollToIndex (((CarouselLayout)this.Element).SelectedIndex);
			}
		}

		void HScrollViewTouch (object sender, TouchEventArgs e)
		{
			e.Handled = false;

			switch (e.Event.Action) {
				case MotionEventActions.Move:
					_deltaXResetTimer.Stop ();
					_deltaX = _scrollView.ScrollX - _prevScrollX;
					_prevScrollX = _scrollView.ScrollX;

					UpdateSelectedIndex ();

					_deltaXResetTimer.Start ();
					break;
				case MotionEventActions.Down:
					_motionDown = true;
					_scrollStopTimer.Stop ();
					break;
				case MotionEventActions.Up:
					_motionDown = false;
					SnapScroll ();
					_scrollStopTimer.Start ();
					break;
			}
		}

		void UpdateSelectedIndex () {
			var center = _scrollView.ScrollX + (_scrollView.Width / 2);
			var carouselLayout = (CarouselLayout)this.Element;
			carouselLayout.SelectedIndex = (center / _scrollView.Width);
		}

		void SnapScroll ()
		{
			var roughIndex = (float)_scrollView.ScrollX / _scrollView.Width;

			var targetIndex = 
				_deltaX < 0 ? Math.Floor (roughIndex)
				: _deltaX > 0 ? Math.Ceil (roughIndex)
				: Math.Round (roughIndex);

			ScrollToIndex ((int)targetIndex);
		}

		void ScrollToIndex (int targetIndex)
		{
			var targetX = targetIndex * _scrollView.Width;
			_scrollView.Post (new Runnable (() => {
				_scrollView.SmoothScrollTo(targetX, 0);
			}));
		}

		bool _initialized = false;
		public override void Draw (Canvas canvas)
		{
			base.Draw (canvas);
			if (_initialized) return;
			_initialized = true;
			var carouselLayout = (CarouselLayout)this.Element;
			_scrollView.ScrollTo (carouselLayout.SelectedIndex * Width, 0);
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			if(_initialized && (w != oldw))
			{
				_initialized = false;
			}
			base.OnSizeChanged(w, h, oldw, oldh);
		}
	}
}