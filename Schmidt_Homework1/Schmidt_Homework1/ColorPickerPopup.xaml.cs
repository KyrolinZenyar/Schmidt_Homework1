using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Schmidt_Homework1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ColorPickerPopup : ContentPage
	{
		public ColorPickerPopup (SKColor color)
		{
			InitializeComponent ();

		}
	}
}