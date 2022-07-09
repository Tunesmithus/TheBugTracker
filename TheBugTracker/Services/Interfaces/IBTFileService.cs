using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBugTracker.Services.Interfaces
{
    public interface IBTFileService
    {

        Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);
        string ConvertByteArrayToFile(byte[] fileData, string extension);

        string GetFileIcon(string file);
        string FormatFileSize(long bytes);

    }
}
