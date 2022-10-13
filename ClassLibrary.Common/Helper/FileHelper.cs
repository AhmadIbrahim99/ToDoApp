using System;
using System.IO;
using ClassLibrary.Common.Exceptions;

namespace ClassLibrary.Common.Helper
{
    public class FileHelper
    {
        public static string SaveImage(string base64img,string baseFolder)
        {
            if (string.IsNullOrWhiteSpace(baseFolder)) throw new AhmadException(401,"No Base Folder");
            if (string.IsNullOrWhiteSpace(base64img)) throw new AhmadException(401, "No Image");
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), baseFolder);
            if(!Directory.Exists(folderPath))Directory.CreateDirectory(folderPath);
            var base64Array = base64img.Split(";base64,");
            if (base64Array.Length < 1) throw new AhmadException(401, "Empty base64Array");
            base64img = base64Array[1]; 
            var fileName = $"{Guid.NewGuid()}{"Logo.png"}".Replace("-","", StringComparison.InvariantCultureIgnoreCase);
            var url = $@"{baseFolder}\{fileName}";
            fileName = $@"{folderPath}\{fileName}";
            File.WriteAllBytes(fileName, Convert.FromBase64String(base64img));
            return url;
        }
    }
}
