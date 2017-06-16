using System;
using MvvmHelpers;
using Xamarin.Forms;

namespace CarouselLayout
{
	public class CarouselItem : BaseViewModel
	{
		public Color Background { get; set; }
		public string ImageSource { get; set; }
	}
}
