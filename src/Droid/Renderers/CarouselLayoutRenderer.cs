using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Java.Lang;
using System.Timers;
using Android.Widget;
using Android.Views;
using System.ComponentModel;
using Android.Graphics;
using CustomLayouts.Controls;
using CustomLayouts.Droid.Renderers;
using System.Reflection;

[assembly: ExportRenderer(typeof(CarouselLayout), typeof(CarouselLayoutRenderer))]

namespace CustomLayouts.Droid.Renderers
{
    public class CarouselLayoutRenderer : ScrollViewRenderer
    {
        int _prevScroll;
        int _delta;
        bool _motionDown;
        Timer _deltaResetTimer;
        Timer _scrollStopTimer;
        Android.Widget.ScrollView _verticalScrollView;
        HorizontalScrollView _scrollView;
        bool _isVertical;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
                return;

            _deltaResetTimer = new Timer(100) { AutoReset = false };
            _deltaResetTimer.Elapsed += (object sender, ElapsedEventArgs args) => _delta = 0;

            _scrollStopTimer = new Timer(200) { AutoReset = false };
            _scrollStopTimer.Elapsed += (object sender, ElapsedEventArgs args2) => UpdateSelectedIndex();

            _isVertical = (((CarouselLayout)Element).Orientation == ScrollOrientation.Vertical);

            e.NewElement.PropertyChanged += ElementPropertyChanged;
        }

        void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Renderer")
            {
                if (_isVertical)
                {
                    _verticalScrollView = this;
                    _verticalScrollView.Touch += ScrollViewTouch;
                }
                else
                {
                    _scrollView = (HorizontalScrollView)typeof(ScrollViewRenderer)
                        .GetField("hScrollView", BindingFlags.NonPublic | BindingFlags.Instance)
                        .GetValue(this);

                    _scrollView.HorizontalScrollBarEnabled = false;
                    _scrollView.Touch += ScrollViewTouch;
                }
            }
            if (e.PropertyName == CarouselLayout.SelectedIndexProperty.PropertyName && !_motionDown)
            {
                ScrollToIndex(((CarouselLayout)this.Element).SelectedIndex);
            }
        }

        void ScrollViewTouch(object sender, TouchEventArgs e)
        {
            e.Handled = false;

            switch (e.Event.Action)
            {
                case MotionEventActions.Move:
                    _deltaResetTimer.Stop();
                    if (_isVertical)
                    {
                        _delta = _verticalScrollView.ScrollY - _prevScroll;
                        _prevScroll = _verticalScrollView.ScrollY;
                    }
                    else
                    {
                        _delta = _scrollView.ScrollX - _prevScroll;
                        _prevScroll = _scrollView.ScrollX;
                    }

                    UpdateSelectedIndex();

                    _deltaResetTimer.Start();
                    break;
                case MotionEventActions.Down:
                    _motionDown = true;
                    _scrollStopTimer.Stop();
                    break;
                case MotionEventActions.Up:
                    _motionDown = false;
                    SnapScroll();
                    _scrollStopTimer.Start();
                    break;
            }
        }

        void UpdateSelectedIndex()
        {
            var carouselLayout = (CarouselLayout)this.Element;
            if (_isVertical)
            {
                var center = _verticalScrollView.ScrollY + (_verticalScrollView.Height / 2);
                carouselLayout.SelectedIndex = (center / _verticalScrollView.Height);
            }
            else
            {
                var center = _scrollView.ScrollX + (_scrollView.Width / 2);
                carouselLayout.SelectedIndex = (center / _scrollView.Width);
            }
        }

        void SnapScroll()
        {
            var roughIndex = 0.0;

            if (_isVertical)
                roughIndex = (float)_verticalScrollView.ScrollY / _verticalScrollView.Height;
            else
                roughIndex = (float)_scrollView.ScrollX / _scrollView.Width;

            var targetIndex =
                _delta < 0 ? Math.Floor(roughIndex)
                : _delta > 0 ? Math.Ceil(roughIndex)
                : Math.Round(roughIndex);

            ScrollToIndex((int)targetIndex);
        }

        void ScrollToIndex(int targetIndex)
        {
            if (_isVertical)
            {
                var target = targetIndex * _verticalScrollView.Height;
                _verticalScrollView.Post(new Runnable(() =>
                {
                    _verticalScrollView.SmoothScrollTo(0, target);
                }));
            }
            else
            {
                var target = targetIndex * _scrollView.Width;
                _scrollView.Post(new Runnable(() =>
                {
                    _scrollView.SmoothScrollTo(target, 0);
                }));
            }
        }

        bool _initialized = false;
        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);

            if (_initialized)
                return;

            _initialized = true;
            var carouselLayout = (CarouselLayout)this.Element;
            if (_isVertical)
                _verticalScrollView.ScrollTo(0, carouselLayout.SelectedIndex * Height);
            else
                _scrollView.ScrollTo(carouselLayout.SelectedIndex * Width, 0);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            if (_initialized && (w != oldw))
            {
                _initialized = false;
            }
            base.OnSizeChanged(w, h, oldw, oldh);
        }
    }
}