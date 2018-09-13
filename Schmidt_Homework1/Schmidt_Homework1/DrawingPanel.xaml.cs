using Amporis.Xamarin.Forms.ColorPicker;
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
        private Dictionary<long, SKPath> tempPaths = new Dictionary<long, SKPath>();
        private List<SKPath> paths = new List<SKPath>();
        private SKColor color = new SKColor(0, 0, 0);
        private int strokeWidth = 3;
        SKBitmap paintBitmap;

        public DrawingPanel ()
		{
			InitializeComponent ();
		}

        private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        {
            var paintSurface = e.Surface;
            var paintCanvas = paintSurface.Canvas;
            
            paintCanvas.Clear(SKColors.White);

            //Define SKPaint here (need to make colorpicker and strokeWidth picker)
            var paint = new SKPaint
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
                paintCanvas.DrawPath(path, paint);
            }

            paintBitmap = new SKBitmap((int)Canvas.Width, (int)Canvas.Height);
            e.Surface.Canvas.DrawBitmap(paintBitmap, 0, 0);

        }

        private void OnTouch(object sender, SKTouchEventArgs e)
        {

            if (e.ActionType == SKTouchAction.Pressed)
            {
                //When a line begins to be drawn (finger down)
                var path = new SKPath();
                path.MoveTo(e.Location);
                tempPaths[e.Id] = path;
            }
            else if (e.ActionType == SKTouchAction.Moved) {
                //While the line is being drawn (finger moving)
                if (e.InContact)
                {
                    tempPaths[e.Id].LineTo(e.Location);
                }
            }
            else if (e.ActionType == SKTouchAction.Released) {
                //When the line ends (finger up)
                paths.Add(tempPaths[e.Id]);
                tempPaths.Remove(e.Id);
            }

            //Once the event is handled, mark it as such and refresh the UI.
            e.Handled = true;
            ((SKCanvasView)sender).InvalidateSurface();
        }

        //NEED TO ADD COLOR PICKER
        private async void OnColorPick(object sender, EventArgs e)
        {
            //ColorPickerPopup popupPage = new ColorPickerPopup();
            //await Rg.Plugins.Popup.Extensions.NavigationExtension.PushPopupAsync(this.Navigation, popupPage);
            //await PopupNavigation.Instance.PushAsync(_colorPickerPopup);
            PickColor.Text = "Clicked!";
            var colorPage = new ColorPickerPopup(color);
            await Navigation.PushModalAsync(colorPage);
            //var newColor = await ColorPickerDialog.Show(mainGrid, "Pick a Color", Color.Black, null);
        }

        /***
         * Method OnClear: Called on click of Clear button on Drawing Panel Tab
         * Clears all drawn paths from drawing canvas.
         **/
        private void OnClear(object sender, EventArgs e)
        {
            //Clear list of drawn paths and refresh UI
            paths.Clear();
            Canvas.InvalidateSurface();
        }

        //NEED TO ADD SAVE FUNCTIONALITY
        private async void OnSave(object sender, EventArgs e)
        {
            var info = new SKImageInfo((int)Canvas.Width, (int)Canvas.Height);

            //var surface = pain
            //var canvas = paintSurface.Canvas;
            //canvas.Clear();
            //var paint = new SKPaint
            //{
            //    Color = color,
            //    Style = SKPaintStyle.Stroke,
            //    IsAntialias = true,
            //    StrokeWidth = strokeWidth
            //};

            //foreach (SKPath path in paths)
            //    canvas.DrawPath(path, paint);

            //foreach (SKPath path in tempPaths.Values)
            //    canvas.DrawPath(path, paint);

            //canvas.Flush();

            //var snap = surface.Snapshot();
            //var pngImage = snap.Encode();

            //SKImage image = SKImage.FromBitmap();

            //byte[] photoData = pngImage.ToArray();

            //SKBitmap paintBitmap = new SKBitmap(info.Width, info.Height);
            //canvas.DrawBitmap(paintBitmap, 0, 0);

            SKImage paintPhoto = SKImage.FromBitmap(paintBitmap);
            SKData photoData = paintPhoto.Encode();

            if(photoData == null)
            {
                Save.Text = "photo data null";
            }
            //else if(photoData.Length == 0)
            //{
            //    Save.Text = "encode returned empty";
            //}
            else
            {
                var photoSaver = DependencyService.Get<IPhotoSaver>();
                string success = await DependencyService.Get<IPhotoSaver>().SaveAsync(photoData.ToArray(), "test.jpg");
                //if (success == true)
                //{
                //    Save.Text = "Save success";
                //}
                //else
                //{
                //    Save.Text = "save failed";
                //}
                //test.Text = success;
            }

        }

    }
}