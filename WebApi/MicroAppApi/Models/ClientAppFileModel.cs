

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.Security.Cryptography;

namespace MicroAppApi.Models
{

    public class ClientAppFileModel
    {
        public string Filename { get; set; }
        public string FilePath { get; set; }
        public string Hash { get; set; }

        public ClientAppFileModel(string path, string contentRootPath)
        {
            this.Filename = Path.GetFileName(path);
            string relativePath = Path.GetRelativePath(contentRootPath, path);
            this.FilePath = relativePath;
            try
            {
                SHA256 hashfunction = SHA256.Create();
                byte[] hashValue;

                using (FileStream fileStream =
                File.OpenRead(path))
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    byte[] streamOfFile = new byte[reader.BaseStream.Length];
                    reader.BaseStream.Read(streamOfFile, 0, (int)reader.BaseStream.Length);

                    this.Hash = ConvertByteArrayToString(hashfunction.ComputeHash(streamOfFile));
                }

            }
            catch (Exception ex)
            {

            }


        }

        public string ConvertByteArrayToString(byte[] array)
        {
            string returnStr = "";
            int i;
            for (i = 0; i < array.Length; i++)
            {
                returnStr += String.Format("{0:X2}", array[i]);
                //if ((i % 4) == 3) Console.Write(" ");
            }
            return returnStr;
        }

    }

}

// using (FileStream fileStream =
//             File.Create(@"C:\programs\example1.txt"))
//         using (StreamWriter writer = new StreamWriter(fileStream))
//         {
//             writer.WriteLine("Example 1 written");
//         }