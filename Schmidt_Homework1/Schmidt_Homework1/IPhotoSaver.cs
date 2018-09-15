using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Schmidt_Homework1
{
    public interface IPhotoSaver
    {
        //Task<Boolean> SaveAsync(byte[] photoData, string saveFile);
        Task<Boolean> SaveAsync(SKData photoData, string saveFile);
    }
}
