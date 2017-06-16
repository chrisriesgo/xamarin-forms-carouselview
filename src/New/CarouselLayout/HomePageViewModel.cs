using System;
using System.Collections.Generic;
using MvvmHelpers;
using Xamarin.Forms;

namespace CarouselLayout
{
	public class HomePageViewModel : BaseViewModel
	{
		List<CarouselItem> _items;
		public List<CarouselItem> Items 
		{
			get => _items;
			set => this.SetProperty<List<CarouselItem>> (ref this._items, value, nameof(Items), null);
		}
		
		CarouselItem _selectedItem;
		public CarouselItem SelectedItem
		{
			get => _selectedItem;
			set => this.SetProperty<CarouselItem>(ref this._selectedItem, value, nameof(SelectedItem), null);
		}
		
		public HomePageViewModel()
		{
			Items = new List<CarouselItem>() {
				new CarouselItem { Title = "1", Background = Color.White, ImageSource = "icon.png" },
				new CarouselItem { Title = "2", Background = Color.Red, ImageSource = "icon.png" },
				new CarouselItem { Title = "3", Background = Color.Blue, ImageSource = "icon.png" },
				new CarouselItem { Title = "4", Background = Color.Yellow, ImageSource = "icon.png" },
			};
		}
		
		
	}
}
