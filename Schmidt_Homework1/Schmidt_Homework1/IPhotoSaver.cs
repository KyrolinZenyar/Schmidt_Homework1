using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Schmidt_Homework1
{
    public interface IPhotoSaver
    {
        Task<string> SaveAsync(byte[] photoData, string saveFile);
    }
}
