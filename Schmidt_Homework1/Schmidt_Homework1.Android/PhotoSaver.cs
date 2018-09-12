using Schmidt_Homework1;
using System;
using Java.IO;
using System.Threading.Tasks;
using Xamarin.Android;
using Xamarin.Forms;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.App;

[assembly: Dependency(typeof(PhotoSaver))]
public class PhotoSaver: IPhotoSaver
{
    public async Task<string> SaveAsync(byte[] photoData, string saveFileName)
    {
        string test;
        try
        {
            //File pictureDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            string pictureDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            File saveFile = new File(pictureDir, saveFileName);
            //Android.App.Application.Context context;
            
            test = saveFile.AbsolutePath;
            //Activity activity = (Activity)Android.App.Application.Context;
            //if (ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted)
            //{
            //    ActivityCompat.RequestPermissions(activity, new String[] { Manifest.Permission.WriteExternalStorage }, 1);

            //}
            //else
            //{

            //}
            FileOutputStream outputStream = new FileOutputStream(saveFile);

            saveFile.CreateNewFile();

            //await outputStream.WriteAsync(photoData);
        }
        catch(Exception e)
        {
            test = e.InnerException.Message;
            return test;
        }

        return test;
    }
}