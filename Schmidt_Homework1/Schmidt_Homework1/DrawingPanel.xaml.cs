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
        private Dictionary<long, SKPath> paths = new Dictionary<long, SKPath>();
        private Dictionary<long, SKPaint> paints = new Dictionary<long, SKPaint>();
        private SKColor color = new SKColor(0, 0, 0);
        private int strokeWidth = 3;
        private SKBitmap paintBitmap;
        private SKPaint paint;
        private long pathID = 0;
        //private SKImage snapshot;
        private long numberOfSaves = 0;

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

            //PROBLEM HERE?
            // Create bitmap the size of the display surface
            if (paintBitmap == null)
            {
                paintBitmap = new SKBitmap(e.Info.Width, e.Info.Height);
            }
            // Or create new bitmap for a new size of display surface
            else if (paintBitmap.Width < e.Info.Width || paintBitmap.Height < e.Info.Height)
            {
                SKBitmap newBitmap = new SKBitmap(Math.Max(paintBitmap.Width, e.Info.Width),
                                                  Math.Max(paintBitmap.Height, e.Info.Height));

                using (SKCanvas newCanvas = new SKCanvas(newBitmap))
                {
                    newCanvas.Clear(SKColors.White);
                    newCanvas.DrawBitmap(paintBitmap, 0, 0);
                }

                paintBitmap = newBitmap;
            }
            e.Surface.Canvas.DrawBitmap(paintBitmap, 0, 0);
            //snapshot = e.Surface.Snapshot();

        }

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

        void UpdateBitmap()
        {
            using (SKCanvas saveBitmapCanvas = new SKCanvas(paintBitmap))
            {
                saveBitmapCanvas.Clear(SKColors.White);

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

        }

        private async void OnColorPick(object sender, EventArgs e)
        {
            var colorPage = new ColorPickerPopup(color, strokeWidth);
            await Navigation.PushModalAsync(colorPage);
            colorPage.ColorChosen += ChangeStrokeColor;
        }

        private void ChangeStrokeColor(object sender, ColorPickerEventArgs e)
        {
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

            //SKImage paintPhoto = SKImage.FromBitmap(paintBitmap);
            //SKData photoData = paintPhoto.Encode();

            SKData photoData = SKImage.FromBitmap(paintBitmap).Encode(SKEncodedImageFormat.Jpeg, 100);
            //photoData.SaveTo()

            if(photoData == null)
            {
                Save.Text = "photo data null";
            }
            //else if (photoData.Length == 0)
            //{
            //    Save.Text = "encode returned empty";
            //}
            else
            {
                var photoSaver = DependencyService.Get<IPhotoSaver>();
                //CHANGE NAME OF FILE
                //Boolean success = await DependencyService.Get<IPhotoSaver>().SaveAsync(photoData.ToArray(), "test.jpg");
                string saveFileName = String.Format("Schmidt_HW_1_Image_{0}.jpg", numberOfSaves);
                Boolean success = await DependencyService.Get<IPhotoSaver>().SaveAsync(photoData, saveFileName);
                if (success == true)
                {
                    Save.Text = "Save success";
                    numberOfSaves++;
                }
                else
                {
                    Save.Text = "save failed";
                }
                Device.StartTimer(TimeSpan.FromMilliseconds(1000), () => {
                    Save.Text = "Save";
                    return false;
                });
                //test.Text = success;
            }

        }

    }
}