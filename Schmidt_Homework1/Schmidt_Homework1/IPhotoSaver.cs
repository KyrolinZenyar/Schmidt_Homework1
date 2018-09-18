using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Schmidt_Homework1
{
    public interface IPhotoSaver
    {
        //Interface for photo saver functionality
        Task<Boolean> SaveAsync(SKData photoData, string saveFile);
    }
}
