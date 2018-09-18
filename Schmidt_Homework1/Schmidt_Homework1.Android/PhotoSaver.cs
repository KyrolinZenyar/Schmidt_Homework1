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
    /**
     *  Async task to save the canvas to gallery on button press.
     * Parameters: photoData - the canvas data to be saved, saveFileName - the file name to save the photo under
     * Gets permissions from user if needed and saves to gallery if permission given
     **/
    public async Task<Boolean> SaveAsync(SKData photoData, string saveFileName)
    {
        try
        {
            //Get picture directory on filesystem and assemble file data for creation
            File pictureDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            File saveFile = new File(pictureDir, saveFileName);
           
            //Check if permissions granted
            var permStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (permStatus != PermissionStatus.Granted)
            {
                //If permissions aren't granted, request them
                var permResults = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                permStatus = permResults[Permission.Storage];
                //Return false for permissions denied
                if (permStatus != PermissionStatus.Granted)
                {
                    return false;
                }
                    
                
            }

            if (permStatus == PermissionStatus.Granted)
            {
                //If permissions are granted, create output stream for file
                FileOutputStream outputStream = new FileOutputStream(saveFile);
                //Create the file, then save the file
                saveFile.CreateNewFile();
                //Save photo file
                var stream = System.IO.File.OpenWrite(pictureDir.AbsolutePath + '/' + saveFileName);
                photoData.SaveTo(stream);
                //Close the stream
                stream.Close();
            }
        }
        catch
        {
            //If an exception occurs, return that the saving failed.
            return false;
        }
        //If the saving succeeds, return that.
        return true;
    }
}