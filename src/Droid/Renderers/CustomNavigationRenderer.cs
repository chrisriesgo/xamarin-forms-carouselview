using Android.App;
using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CustomLayouts.Droid.Renderers;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationRenderer))]

namespace CustomLayouts.Droid.Renderers
{
	public class CustomNavigationRenderer : NavigationRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
		{
			base.OnElementChanged (e);

			RemoveAppIconFromActionBar ();
		}

		void RemoveAppIconFromActionBar()
		{
			// http://stackoverflow.com/questions/14606294/remove-icon-logo-from-action-bar-on-android
			var actionBar = ((Activity)Context).ActionBar;
			actionBar.SetIcon (new ColorDrawable(Color.Transparent.ToAndroid()));
		}
	}
}