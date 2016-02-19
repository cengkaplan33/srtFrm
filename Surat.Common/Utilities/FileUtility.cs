using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Surat.Common.Utilities
{
    public class FileUtility
    {
        private static readonly object lockObject = new object();

        #region Methods

        public static void WriteFile(string filePath, string fileName,string data,bool append)
        {
            lock (lockObject)
            {
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                using (StreamWriter writer = new StreamWriter(filePath + fileName, append))
                {
                    writer.WriteLine(data);
                    writer.Close();
                }
            }

        }

        public static string ReadFromFile(string filePath, string fileName)
        {
            string fileContent;
            using (StreamReader reader = new StreamReader(filePath + fileName))
            {
                fileContent = reader.ReadToEnd();
            }

            return fileContent;
        }

        #endregion

    }
}
