

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace MicroAppApi.Models
{

    public class ClientAppFileModel
    {
        public string Filename { get; set; }
        public string FilePath { get; set; }
        public string Hash { get; set; }

        private string _contentFullServerPath = null;

        public ClientAppFileModel(string path, string contentRootPath)
        {
            this.Filename = Path.GetFileName(path);
            string relativePath = Path.GetRelativePath(contentRootPath, path);
            this.FilePath = relativePath;


            _contentFullServerPath = path;
            Console.WriteLine($"Passed in full path: {_contentFullServerPath}");

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

        public string ReadFileToString()
        {
            if (String.IsNullOrWhiteSpace(_contentFullServerPath))
            {
                return null;
            }

            try
            {

                // Byte[] bytes = File.ReadAllBytes("path");
                // String file = Convert.ToBase64String(bytes);

                FileStream fileStream = new FileStream(_contentFullServerPath, FileMode.Open);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line = reader.ReadToEnd();

                    var isBinaryFile = line.Contains("\0\0");
                    //https://stackoverflow.com/questions/910873/how-can-i-determine-if-a-file-is-binary-or-text-in-c

                    if (isBinaryFile)
                    {
                        //We have a binary file...we can't remove any bytes just read the file in exactly as byte[] and pass back...
                        Byte[] bytes = File.ReadAllBytes(_contentFullServerPath);
                        String file = Convert.ToBase64String(bytes);
                        return file;
                    }
                    else
                    {
                        //we have a text file...we can remove any odd bytes

                        string s = "søme string";
                        s = Regex.Replace(line, @"[^\u0000-\u007F]+", string.Empty);
                        return s;
                    }
                }
            }
            catch (Exception error)
            {
                //TODO LOG THIS ERROR!!
            }
            return null;

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