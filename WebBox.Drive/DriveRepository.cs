using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBox.Data.Drive.Extensions;

namespace WebBox.Data.Drive
{
    public class DriveRepository
    {
        public FileSystemObject Query(string origin, string route, IEnumerable<KeyValuePair<string, string>> nameValuePairs)
        {
            string path = nameValuePairs.GetValue("path");
            string searchPattern = nameValuePairs.GetValue("searchPattern");
            string searchOption = nameValuePairs.GetValue("searchOption");
            string md5 = nameValuePairs.GetValue("md5");
            bool withMD5 = string.IsNullOrWhiteSpace(md5) ? false : md5.ToLower() == "md5";
            return CreateFileSystemObject(origin, route, path, searchPattern, searchOption, withMD5);
        }

        public void SaveFileInfo(string physicalPath, IEnumerable<KeyValuePair<string, string>> nameValuePairs)
        {
            FileInfo info = new FileInfo(physicalPath);
            // Attributes
            string creationTime = nameValuePairs.GetValue("creationTime");
            if (!string.IsNullOrWhiteSpace(creationTime))
            {
                info.CreationTime = DateTime.Parse(creationTime);
            }
            string isReadOnly = nameValuePairs.GetValue("isReadOnly");
            if (!string.IsNullOrWhiteSpace(isReadOnly))
            {
                if (isReadOnly == "on" || isReadOnly == "true" || isReadOnly == "True")
                {
                    info.IsReadOnly = true;
                }
            }
            string lastAccessTime = nameValuePairs.GetValue("lastAccessTime");
            if (!string.IsNullOrWhiteSpace(lastAccessTime))
            {
                info.LastAccessTime = DateTime.Parse(lastAccessTime);
            }
            string lastWriteTime = nameValuePairs.GetValue("lastWriteTime");
            if (!string.IsNullOrWhiteSpace(lastWriteTime))
            {
                info.LastWriteTime = DateTime.Parse(lastWriteTime);
            }
        }

        public void Delete(string origin, string route, IEnumerable<KeyValuePair<string, string>> nameValuePairs)
        {
            FileSystemObject obj = Query(origin, route, nameValuePairs);
            if (!obj.Exists) return;

            if (obj is FileObject)
            {
                File.Delete(obj.GetPhysicalPath());
            }
            else if (obj is DirectoryObject)
            {
                if ((obj as DirectoryObject).IsDrive) throw new InvalidOperationException("Drive is not allowed to delete.");

                // 
                string recursive = nameValuePairs.GetValue("recursive");
                Directory.Delete(obj.GetPhysicalPath(), recursive == "true" || recursive == "True");
            }
        }
     
        public void Put(string origin, string route, IEnumerable<KeyValuePair<string, string>> nameValuePairs)
        {
            FileSystemObject obj = Query(origin, route, nameValuePairs);
            string copyto = nameValuePairs.GetValue("copyto");
            if (!string.IsNullOrWhiteSpace(copyto))
            {
                string overwrite = nameValuePairs.GetValue("overwrite");
                CopyTo(obj, copyto, overwrite == "true" || overwrite == "True");
                return;
            }

            //
            string moveto = nameValuePairs.GetValue("moveto");
            if (!string.IsNullOrWhiteSpace(moveto))
            {
                MoveTo(obj, moveto);
                return;
            }

            //
            Update(obj, nameValuePairs);
        }

        private void CopyTo(FileSystemObject obj, string destPath, bool overwrite)
        {
            if (obj is DirectoryObject)
            {
                throw new ArgumentException(string.Format("Source file: {0} specifies a directory.", obj.Path));
            }
            if (!obj.Exists)
            {
                throw new FileNotFoundException(string.Format("Source file: {0} was not found.", obj.Path));
            }

            string destPhysicalPath = DriveObject.Drives.GetDirectory(obj.Route);
            destPhysicalPath += destPath.Replace('/', System.IO.Path.DirectorySeparatorChar);

            string directory = Path.GetDirectoryName(destPhysicalPath);
            Directory.CreateDirectory(directory);

            FileInfo fileInfo = new FileInfo(obj.GetPhysicalPath());
            fileInfo.CopyTo(destPhysicalPath, overwrite);
        }

        private void MoveTo(FileSystemObject obj, string destPath)
        {
            if (!obj.Exists)
            {
                if (obj is DirectoryObject)
                {
                    throw new DirectoryNotFoundException(string.Format("Source directory: {0} was not found.", obj.Path));
                }
                else
                {
                    Debug.Assert(obj is FileObject);

                    throw new FileNotFoundException(string.Format("Source file: {0} was not found.", obj.Path));
                }
            }

            //
            string destPhysicalPath = DriveObject.Drives.GetDirectory(obj.Route);
            destPhysicalPath += destPath.Replace('/', System.IO.Path.DirectorySeparatorChar);

            if (obj is DirectoryObject)
            { 
                DirectoryInfo dirInfo = new DirectoryInfo(obj.GetPhysicalPath());
                dirInfo.MoveTo(destPhysicalPath);
            }
            else
            {
                Debug.Assert(obj is FileObject);

                string directory = Path.GetDirectoryName(destPhysicalPath);
                Directory.CreateDirectory(directory);

                FileInfo fileInfo = new FileInfo(obj.GetPhysicalPath());
                fileInfo.MoveTo(destPhysicalPath);
            }
        }

        // if the directory does not exist, create it.
        private void Update(FileSystemObject obj, IEnumerable<KeyValuePair<string, string>> nameValuePairs)
        {
            if (!obj.Exists)
            {
                if (obj is DirectoryObject)
                {
                    // Create Directory
                    DirectoryInfo info = new DirectoryInfo(obj.GetPhysicalPath());
                    info.Create();
                    SetFileSystemInfo(info, nameValuePairs);
                }
            }

            if (obj is FileObject)
            {
                FileInfo info = new FileInfo(obj.GetPhysicalPath());
                SetFileInfo(info, nameValuePairs);
            }
            else if (obj is DirectoryObject)
            {
                if ((obj as DirectoryObject).IsDrive) throw new InvalidOperationException("Drive is not allowed to update.");

                DirectoryInfo info = new DirectoryInfo(obj.GetPhysicalPath());
                SetDirectoryInfo(info, nameValuePairs);
            }
        }

        private static void SetFileInfo(FileInfo info, IEnumerable<KeyValuePair<string, string>> nameValuePairs)
        {
            string name = nameValuePairs.GetValue("name");
            if (!string.IsNullOrWhiteSpace(name))
            {
                string directoryName = System.IO.Path.GetDirectoryName(info.FullName);
                string fullName = System.IO.Path.Combine(directoryName, name);
                info.MoveTo(fullName);
            }

            SetFileSystemInfo(info, nameValuePairs);
            string isReadOnly = nameValuePairs.GetValue("isReadOnly");
            if (!string.IsNullOrWhiteSpace(isReadOnly))
            {
                if (isReadOnly == "true" || isReadOnly == "True")
                {
                    info.IsReadOnly = true;
                }
                else if (isReadOnly == "false" || isReadOnly == "False")
                {
                    info.IsReadOnly = false;
                }
            }
        }

        private static void SetDirectoryInfo(DirectoryInfo info, IEnumerable<KeyValuePair<string, string>> nameValuePairs)
        {
            string name = nameValuePairs.GetValue("name");
            if (!string.IsNullOrWhiteSpace(name))
            {
                string fullName = System.IO.Path.Combine(info.Parent.FullName, name);
                info.MoveTo(fullName);
            }

            SetFileSystemInfo(info, nameValuePairs);
        }

        private static void SetFileSystemInfo(FileSystemInfo info, IEnumerable<KeyValuePair<string, string>> nameValuePairs)
        {
            //info.Attributes

            string creationTime = nameValuePairs.GetValue("creationTime");
            if (!string.IsNullOrWhiteSpace(creationTime))
            {
                info.CreationTime = DateTime.Parse(creationTime);
            }

            string lastAccessTime = nameValuePairs.GetValue("lastAccessTime");
            if (!string.IsNullOrWhiteSpace(lastAccessTime))
            {
                info.LastAccessTime = DateTime.Parse(lastAccessTime);
            }

            string lastWriteTime = nameValuePairs.GetValue("lastWriteTime");
            if (!string.IsNullOrWhiteSpace(lastWriteTime))
            {
                info.LastWriteTime = DateTime.Parse(lastWriteTime);
            }
        }

        private static FileSystemObject CreateFileSystemObject(string origin, string route, string path, string searchPattern, string searchOption, bool withMD5)
        {
            FileObject fileObject = new FileObject(origin, route, path, withMD5);
            if (fileObject.Exists) return fileObject;

            DirectoryObject directoryObject = new DirectoryObject(origin, route, path, searchPattern, searchOption);
            if (directoryObject.Exists) return directoryObject;

            // not exists
            if (string.IsNullOrWhiteSpace(path) || path.EndsWith("/")) return directoryObject;
            return fileObject;
        }


    }
}
