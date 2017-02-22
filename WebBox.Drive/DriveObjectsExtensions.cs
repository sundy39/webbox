using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebBox.Data.Drive.Extensions
{
    public static class DriveObjectsExtensions
    {
        public static string GetDirectory(this IEnumerable<DriveObject> drives, string route)
        {
            DriveObject driveObject = drives.FirstOrDefault(d => d.Route == route);
            return (driveObject == null) ? null : driveObject.GetDirectory();
        }

        public static XElement ToElement(this IEnumerable<DriveObject> drives)
        {
            XElement element = new XElement("Drives");
            foreach (DriveObject drive in drives)
            {
                element.Add(ToElement(drive));
            }
            return element;
        }

        private static XElement ToElement(DriveObject drive)
        {
            XElement element = new XElement("Drive");
            element.SetElementValue("Route", drive.Route);
            if (string.IsNullOrWhiteSpace(drive.Description))
            {
                element.SetElementValue("Description", string.Empty);
            }
            else
            {
                element.SetElementValue("Description", drive.Description);
            }
            return element;
        }

        public static XElement ToElement(this FileSystemObject obj)
        {
            if (obj is DirectoryObject) return ToElement(obj as DirectoryObject);
            return ToElement(obj as FileObject);
        }

        private static XElement ToElement(DirectoryObject dir)
        {
            XElement element;
            if (dir.IsDrive)
            {
                element = new XElement("Drive");
                SetElementRequiredValues(element, dir);
            }
            else
            {
                element = new XElement("Directory");
                SetElement(element, dir);
            }

            XElement directoriesElement = new XElement("Directories");
            foreach (DirectoryObject dirObj in dir.Directories)
            {
                directoriesElement.Add(ToElement(dirObj));
            }
            element.Add(directoriesElement);

            XElement filesElement = new XElement("Files");
            foreach (FileObject file in dir.Files)
            {
                filesElement.Add(ToElement(file));
            }
            element.Add(filesElement);

            return element;
        }

        private static XElement ToElement(FileObject file)
        {
            XElement element = new XElement("File");
            SetElement(element, file);
            if (file.Exists)
            {
                element.SetElementValue("Length", file.Length);
                element.SetElementValue("IsReadOnly", file.IsReadOnly);
                if (string.IsNullOrWhiteSpace(file.MD5))
                {
                    element.SetElementValue("MD5", string.Empty);
                }
                else
                {
                    element.SetElementValue("MD5", file.MD5);
                }
                
            }
            else
            {
                element.SetElementValue("Length", string.Empty);
                element.SetElementValue("IsReadOnly", string.Empty);
                element.SetElementValue("MD5", string.Empty);
            }
            return element;
        }

        private static void SetElement(XElement element, FileSystemObject obj)
        {
            SetElementRequiredValues(element, obj);
            SetElementNullableValues(element, obj);
        }

        private static void SetElementRequiredValues(XElement element, FileSystemObject obj)
        {
            element.SetElementValue("Origin", obj.Origin);
            element.SetElementValue("Route", obj.Route);
            element.SetElementValue("Path", obj.Path);
            element.SetElementValue("Exists", obj.Exists);
        }

        private static void SetElementNullableValues(XElement element, FileSystemObject obj)
        {
            if (obj.Exists)
            {
                element.SetElementValue("Name", obj.Name);
                element.SetElementValue("Extension", obj.Extension);
                element.SetElementValue("Attributes", obj.Attributes);
                element.SetElementValue("CreationTime", ToString(obj.CreationTime));
                element.SetElementValue("CreationTimeUtc", ToString(obj.CreationTimeUtc));
                element.SetElementValue("LastAccessTime", ToString(obj.LastAccessTime));
                element.SetElementValue("LastAccessTimeUtc", ToString(obj.LastAccessTimeUtc));
                element.SetElementValue("LastWriteTime", ToString(obj.LastWriteTime));
                element.SetElementValue("LastWriteTimeUtc", ToString(obj.LastWriteTimeUtc));
            }
            else
            {
                element.SetElementValue("Name", string.Empty);
                element.SetElementValue("Extension", string.Empty);
                element.SetElementValue("Attributes", string.Empty);
                element.SetElementValue("CreationTime", string.Empty);
                element.SetElementValue("CreationTimeUtc", string.Empty);
                element.SetElementValue("LastAccessTime", string.Empty);
                element.SetElementValue("LastAccessTimeUtc", string.Empty);
                element.SetElementValue("LastWriteTime", string.Empty);
                element.SetElementValue("LastWriteTimeUtc", string.Empty);
            }
        }

        private static string ToString(DateTime? value)
        {
            DateTime dateTime = (DateTime)value;
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                //"2014-12-05T04:54:28.9469344Z";
                return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFZ");

            }
            else if (dateTime.Kind == DateTimeKind.Local)
            {
                // 2014-12-05T12:54:28.9469344+08:00                
                TimeSpan offset = TimeZoneInfo.Local.BaseUtcOffset;          
                string suffix = offset.ToString();
                if (!suffix.StartsWith("-")) suffix = "+" + suffix;
                if (offset.Seconds == 0) suffix = suffix.Substring(0, suffix.Length - 3);
                return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFF") + suffix;
            }
            else
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFF");
            }
        }


    }
}
