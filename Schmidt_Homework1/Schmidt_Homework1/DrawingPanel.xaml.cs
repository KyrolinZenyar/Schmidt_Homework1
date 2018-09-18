using SkiaSharp;
using SkiaSharp.Views.Forms;
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
	public partial class DrawingPanel : ContentPage
	{
        //Variable declarations
        private Dictionary<long, SKPath> tempPaths = new Dictionary<long, SKPath>();
        private Dictionary<long, SKPath> paths = new Dictionary<long, SKPath>();
        private Dictionary<long, SKPaint> paints = new Dictionary<long, SKPaint>();
        private SKColor color = new SKColor(0, 0, 0);
        private int strokeWidth = 3;
        private SKBitmap paintBitmap;
        private SKPaint paint;
        private long pathID = 0;
        private long numberOfSaves = 0;

        public DrawingPanel ()
		{
			InitializeComponent ();
		}

        //When painting is occuring, this gets fired
        private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        {
            //Get surface and canvas for painting
            var paintSurface = e.Surface;
            var paintCanvas = paintSurface.Canvas;
            
            //Set white background for the canvas
            paintCanvas.Clear(SKColors.White);

            //Define paint features
            paint = new SKPaint
            {
                Color = color,
                Style = SKPaintStyle.Stroke,
                IsAntialias = true,
                StrokeWidth = strokeWidth
            };

            //As new paths (temporary or permanent) get created, draw them on the canvas
            foreach (var path in tempPaths)
            {
                paintCanvas.DrawPath(path.Value, paint);
            }
            foreach (var path in paths)
            {
                var id = path.Key;

                paintCanvas.DrawPath(path.Value, paints[id]);
            }

            // Create bitmap the size of the display surface if no bitmap exists yet
            if (paintBitmap == null)
            {
                paintBitmap = new SKBitmap(e.Info.Width, e.Info.Height);
            }
            // Or create new bitmap for a new size of display surface if size has changed (more drawing outside prior drawing bounds)
            else if (paintBitmap.Width < e.Info.Width || paintBitmap.Height < e.Info.Height)
            {
                //Create new bitmap based on whichever is bigger, the existing or new width/height
                SKBitmap newBitmap = new SKBitmap(Math.Max(paintBitmap.Width, e.Info.Width),
                                                  Math.Max(paintBitmap.Height, e.Info.Height));

                //create new temp canvas for the bitmap and set its background to white, then overlay the existing bitmap on it
                SKCanvas newCanvas = new SKCanvas(newBitmap);
                newCanvas.Clear(SKColors.White);
                newCanvas.DrawBitmap(paintBitmap, 0, 0);
                
                paintBitmap = newBitmap;
            }
            //Draw new paths from canvas on the bitmap
            e.Surface.Canvas.DrawBitmap(paintBitmap, 0, 0);
        }

        //When a touch event occurs on the canvas, this gets fired
        private void OnTouch(object sender, SKTouchEventArgs e)
        {

            if (e.ActionType == SKTouchAction.Pressed)
            {
                //When a line begins to be drawn (finger down)
                var path = new SKPath();
                path.MoveTo(e.Location);
                tempPaths[e.Id] = path;
                UpdateBitmap();
            }
            else if (e.ActionType == SKTouchAction.Moved) {
                //While the line is being drawn (finger moving)
                if (e.InContact)
                {
                    tempPaths[e.Id].LineTo(e.Location);
                }
                UpdateBitmap();
            }
            else if (e.ActionType == SKTouchAction.Released) {
                //When the line ends (finger up)
                paths.Add(pathID, tempPaths[e.Id]);
                paints.Add(pathID, paint);
                tempPaths.Remove(e.Id);
                pathID++;
                UpdateBitmap();
            }

            //Once the event is handled, mark it as such and refresh the UI.
            e.Handled = true;
            ((SKCanvasView)sender).InvalidateSurface();
        }

        //This gets fired after every touch even to update the bitmap as the canvas is being changed
        void UpdateBitmap()
        {
            //Create new canvas for the bitmap and set its background to white
            SKCanvas saveBitmapCanvas = new SKCanvas(paintBitmap);
            saveBitmapCanvas.Clear(SKColors.White);
            //Draw each new temporary and permanent path on the canvas
            foreach (var path in tempPaths)
            {
                saveBitmapCanvas.DrawPath(path.Value, paint);
            }
            foreach (var path in paths)
            {
                var id = path.Key;
                saveBitmapCanvas.DrawPath(path.Value, paints[id]);
            }
        }

        //When the colorpicker button is presed, this is called
        private async void OnColorPick(object sender, EventArgs e)
        {
            //Pop up a new color picker modal page
            var colorPage = new ColorPickerPopup(color, strokeWidth);
            await Navigation.PushModalAsync(colorPage);
            //On the event listener being fired, change the stroke width and color,
            //passing through the new color and stroke width
            colorPage.ColorChosen += ChangeStrokeColor;
        }

        //Change the color and stroke width as returned by the color picker modal
        private void ChangeStrokeColor(object sender, ColorPickerEventArgs e)
        {
            //Set the new stroke color and width
            color = new SKColor((byte)e.Red, (byte)e.Green, (byte)e.Blue);
            strokeWidth = e.StrokeWidth;
        }

        /***
         * Method OnClear: Called on click of Clear button on Drawing Panel Tab
         * Clears all drawn paths from drawing canvas.
         **/
        private void OnClear(object sender, EventArgs e)
        {
            //Clear list of paints and paths (temporary and permanent) and refresh UI
            tempPaths.Clear();
            paths.Clear();
            paints.Clear();
            paintBitmap.Reset();
            Canvas.InvalidateSurface();
        }

        //When the save button is pressed, this is called
        private async void OnSave(object sender, EventArgs e)
        {
            //Get canvas width and height
            var info = new SKImageInfo((int)Canvas.Width, (int)Canvas.Height);
            //Get the photo data to save from the bitmap that is keeping the canvas drawings, and encode it as a high quality JPG
            SKData photoData = SKImage.FromBitmap(paintBitmap).Encode(SKEncodedImageFormat.Jpeg, 100);
            
            //If the photo data is null, display that
            if(photoData == null)
            {
                Save.Text = "Data Null";
            }
            //If the photo data isn't null, try to save it
            else
            {
                //Define the photo saver method as per the interface/dependency injection
                var photoSaver = DependencyService.Get<IPhotoSaver>();
                //Set the file name to be dependent on how many times an image has been saved.
                string saveFileName = String.Format("Schmidt_HW_1_Image_{0}.jpg", numberOfSaves);
                //Call the photo saver
                Boolean success = await DependencyService.Get<IPhotoSaver>().SaveAsync(photoData, saveFileName);
                //If the saving succeeded, increment the save number and tell the user
                if (success == true)
                {
                    Save.Text = "Save success";
                    numberOfSaves++;
                }
                //If the save failed, tell the user
                else
                {
                    Save.Text = "save failed";
                }
                //After a second, change the save button's text back to its normal text.
                Device.StartTimer(TimeSpan.FromMilliseconds(1000), () => {
                    Save.Text = "Save";
                    return false;
                });
            }

        }

    }
}