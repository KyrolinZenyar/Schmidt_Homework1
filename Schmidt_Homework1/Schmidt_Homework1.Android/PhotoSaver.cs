using Schmidt_Homework1;
using System;
using Java.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SkiaSharp;

[assembly: Dependency(typeof(PhotoSaver))]
public class PhotoSaver: IPhotoSaver
{
    public async Task<Boolean> SaveAsync(SKData photoData, string saveFileName)
    {
        //string test;
        try
        {
            File pictureDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            //string pictureDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            File saveFile = new File(pictureDir, saveFileName);
            //Android.App.Application.Context context;

            //test = saveFile.AbsolutePath;
            //Activity activity = (Activity)Android.App.Application.Context;
            //if (ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted)
            //{
            //    ActivityCompat.RequestPermissions(activity, new String[] { Manifest.Permission.WriteExternalStorage }, 1);

            //}
            //else
            //{

            //}

            var permStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (permStatus != PermissionStatus.Granted)
            {
                //test = "Perms no";
                var permResults = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                permStatus = permResults[Permission.Storage];
                
            }

            if (permStatus == PermissionStatus.Granted)
            {
                FileOutputStream outputStream = new FileOutputStream(saveFile);
                saveFile.CreateNewFile();
                //await outputStream.WriteAsync(photoData);
                using (var stream = System.IO.File.OpenWrite(pictureDir.AbsolutePath + '/' + saveFileName))
                {
                    photoData.SaveTo(stream);
                }
                //test = String.Format("Perms yes {0}", photoData.Length);
            }



        }
        catch
        {
            //test = e.InnerException.Message;
            return false;
        }

        return true;
    }
}