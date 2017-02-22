using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBox.Data.Drive.Extensions;

namespace WebBox.Data.Drive
{
    public abstract class FileSystemObject
    {
        public bool IsDrive { get; protected set; }
        public bool IsDirectory { get; protected set; }
        public bool IsFile { get; protected set; }

        // http://localhost:51099
        public string Origin { get; private set; }

        // /Drives/X
        public string Route { get; private set; }

        // ?path=docs/HR.docs/john.docx
        public string Path { get; private set; }

        protected readonly string PhysicalPath;

        public string GetPhysicalPath()
        {
            return PhysicalPath;
        }

        public FileSystemObject(string origin, string route, string path)
        {
            Origin = origin;
            Route = route;
            Path = string.IsNullOrWhiteSpace(path) ? "/" : '/' + path.TrimStart('/').TrimEnd('/');

            PhysicalPath = DriveObject.Drives.GetDirectory(Route);
            if (PhysicalPath != null)
            {
                PhysicalPath += Path.Replace('/', System.IO.Path.DirectorySeparatorChar);
            }
        }

        public string Name { get; private set; }
        public bool Exists { get; protected set; }
        public string Extension { get; private set; }

        public string Attributes { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? CreationTimeUtc { get; set; }
        public DateTime? LastAccessTime { get; set; }
        public DateTime? LastAccessTimeUtc { get; set; }
        public DateTime? LastWriteTime { get; set; }
        public DateTime? LastWriteTimeUtc { get; set; }

        protected void SetObject(FileSystemInfo info)
        {
            Exists = info.Exists;
            if (Exists)
            {
                Name = info.Name;
                Extension = info.Extension;

                Attributes = info.Attributes.ToString();
                CreationTime = info.CreationTime;
                CreationTimeUtc = info.CreationTimeUtc;
                LastAccessTime = info.LastAccessTime;
                LastAccessTimeUtc = info.LastAccessTimeUtc;
                LastWriteTime = info.LastWriteTime;
                LastWriteTimeUtc = info.LastWriteTimeUtc;
            }
        }


    }
}
