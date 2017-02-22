using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using WebBox.Data.Drive;
using WebBox.Data.Drive.Extensions;

namespace WebBox.Web.Http.Drive.Models
{
    internal class UploadModel
    {
        public void Upload(string route)
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            if (request.Files.Count == 0) return;
            if (request.Files.Count == 1)
            {
                string path = GetValue("path");
                string physicalPath = GetPhysicalPath(route, path);

                //
                if (Directory.Exists(physicalPath))
                {
                    throw new IOException(string.Format("{0} is a directory.", path));
                }
                if (File.Exists(physicalPath)) throw new IOException(string.Format("{0} already exist.", path));

                //
                string dirName = Path.GetDirectoryName(physicalPath);
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }

                //
                request.Files[0].SaveAs(physicalPath);

                //
                List<KeyValuePair<string, string>> nameValuePairs = new List<KeyValuePair<string, string>>();
                nameValuePairs.Add(new KeyValuePair<string, string>("attributes", GetValue("attributes")));
                nameValuePairs.Add(new KeyValuePair<string, string>("creationTime", GetValue("creationTime")));
                nameValuePairs.Add(new KeyValuePair<string, string>("isReadOnly", GetValue("isReadOnly")));
                nameValuePairs.Add(new KeyValuePair<string, string>("lastAccessTime", GetValue("lastAccessTime")));
                nameValuePairs.Add(new KeyValuePair<string, string>("lastWriteTime", GetValue("lastWriteTime")));
                new DriveRepository().SaveFileInfo(physicalPath, nameValuePairs);
            }
        }

        private string GetPhysicalPath(string route, string path)
        {
            string directory = DriveObject.Drives.GetDirectory(route);
            if (string.IsNullOrWhiteSpace(directory)) throw new DirectoryNotFoundException(route);
            if (string.IsNullOrWhiteSpace(path))
            {
                HttpRequest request = HttpContext.Current.Request;
                path = request.Files.Keys[0];
            }
            path = path.TrimStart('/');
            path = path.Replace('/', Path.DirectorySeparatorChar);
            return Path.Combine(directory, path);
        }

        private string GetValue(string key)
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request.Form.AllKeys.Contains(key))
            {
                return request.Form[key];
            }
            if (request.QueryString[key] != null)
            {
                return request.QueryString[key];
            }
            return null;
        }


    }
}