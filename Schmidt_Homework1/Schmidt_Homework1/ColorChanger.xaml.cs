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
	public partial class ColorChanger : ContentPage
	{
        Random colorGenerator = new Random();

		public ColorChanger ()
		{
			InitializeComponent ();
		}

        /***
         * Method ChangeColor: Called on click of button on ColorChanger Tab
         * Randomly generates a new color and sets the text in the text field on ColorChanger to that color, then
         * gets hex code of said color and changes the label in the text field to state the color in RGB and hex.
         **/
        void ChangeColor(object sender, EventArgs e)
        {
            //Declare variables
            int redSetting, greenSetting, blueSetting;
            string redHex, blueHex, greenHex, colorHex;

            //Generate new RGB color
            redSetting = colorGenerator.Next(256);
            greenSetting = colorGenerator.Next(256);
            blueSetting = colorGenerator.Next(256);
            //Change text field color
            colorText.TextColor = Color.FromRgb(redSetting, greenSetting, blueSetting);
            //Get hex codes for each color
            redHex = redSetting.ToString("X2");
            blueHex = blueSetting.ToString("X2");
            greenHex = greenSetting.ToString("X2");
            //Assemble color's hex code into one var
            colorHex = redHex + greenHex + blueHex;
            //Change text field text
            colorText.Text = "COLOR: " + redSetting + "r, " + greenSetting + "g, "+ blueSetting + "b, #" + colorHex;
        }
	}
}