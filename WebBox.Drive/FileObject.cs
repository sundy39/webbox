using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace WebBox.Data.Drive
{
    public class FileObject : FileSystemObject
    {
        public long? Length { get; private set; }
        public bool? IsReadOnly { get; set; }
        public string MD5 { get; private set; }

        public FileObject(string origin, string route, string path, bool withMD5)
            : base(origin, route, path)
        {
            IsFile = true;

            if (PhysicalPath == null)
            {
                Exists = false;
                return;
            }

            FileInfo info = new FileInfo(PhysicalPath);
            SetObject(info);
            if (Exists)
            {
                Length = info.Length;
                IsReadOnly = info.IsReadOnly;

                if (withMD5)
                {
                    MD5 = ComputeMD5();
                }
            }
        }

        private string ComputeMD5()
        {
            byte[] result;
            FileStream fileStream = new FileStream(PhysicalPath, FileMode.Open, FileAccess.Read);
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                result = md5.ComputeHash(fileStream);
            }
            finally
            {
                fileStream.Close();
            }
            return BitConverter.ToString(result).Replace("-", string.Empty);
        }


    }
}