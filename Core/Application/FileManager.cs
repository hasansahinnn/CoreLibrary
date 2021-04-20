using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Application
{
    public class FileManager
    {
        public static string OfferFiles = Path.Combine("Resources","OfferFiles");
        public static string SaveFile(byte[] fileByte, string fileLocation, string fileName)
        {
            if (fileByte.Length > 0)
            {
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), fileLocation);
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = fileLocation + fileName;
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    stream.Write(fileByte, 0, fileByte.Length);
                }
                return fileName;
            }
            return "";
        }
  
        public static void RemoveFiles(string fileLocation, string fileName) => File.Delete(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), fileLocation), fileName));
    }
}
