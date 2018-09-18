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
        public event EventHandler<ColorPickerEventArgs> ColorChosen;

		public ColorPickerPopup (SKColor color, int stroke)
		{
			InitializeComponent ();
            //This is to counteract a bug in Xamarin where the first tap of the slider doesn't fire the ValueChanged event
            //Basically, set the initial value to non-zero, then after 1 millisecond, shift it back down to whatever the 
            //prior value was so the change is made and the event fires from then on.
            red.Value = 1;
            green.Value = 1;
            blue.Value = 1;
            double redImport = (double)color.Red;
            double greenImport = (double)color.Green;
            double blueImport = (double)color.Blue;
            strokeWidth.Value = 0;
            Device.StartTimer(TimeSpan.FromMilliseconds(1), () => {
                red.Value = redImport;
                green.Value = greenImport;
                blue.Value = blueImport;
                strokeWidth.Value = stroke;
                return false;
            });


        }

        //When the stroke width slider changes, update the text.
        private void OnStrokeChange(object sender, ValueChangedEventArgs e)
        {
            strokeLabel.Text = String.Format("Stroke Width: {0}", (int)e.NewValue);
        }

        //When a color slider changes, update the respective text and the preview box's color.
        private void OnColorChange(object sender, ValueChangedEventArgs e)
        {
            if(sender == red)
            {
                redLabel.Text = String.Format("Red: {0}", (int)e.NewValue);
            }
            else if (sender == green)
            {
                greenLabel.Text = String.Format("Green: {0}", (int)e.NewValue);
                green.Value = e.NewValue;
            }
            else if (sender == blue)
            {
                blueLabel.Text = String.Format("Blue: {0}", (int)e.NewValue);
                blue.Value = e.NewValue;
            }
            colorBox.Color = Color.FromRgb((int)red.Value, (int)green.Value, (int)blue.Value);
        }

        //On cancel, just close the modal
        private async void OnCancel(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        //On submit, close the modal but also fill out the event arguments to return and fire the event listener.
        private async void OnSubmit(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
            ColorPickerEventArgs returnArgs = new ColorPickerEventArgs();
            returnArgs.Red = (int)red.Value;
            returnArgs.Blue = (int)blue.Value;
            returnArgs.Green = (int)green.Value;
            returnArgs.StrokeWidth = (int)strokeWidth.Value;
            ColorChosen(this, returnArgs);
        }
    }

    //Class made for returning color and stroke width data by event listener
    public class ColorPickerEventArgs: EventArgs
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public int StrokeWidth { get; set; }
    }
}