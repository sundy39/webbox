using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Data.Drive
{
    public class DriveObject
    {
        public string Route { get; private set; }
        public string Description { get; private set; }

        private string _directory;

        public string GetDirectory()
        {
            if (!Directory.Exists(_directory))
            {
                throw new DirectoryNotFoundException(Route + ": Physical directory not found.");
            }

            return _directory;
        }

        public DriveObject(string route, string directory, string description)
        {
            Route = route;
            _directory = directory;
            Description = description;
        }

        public static Func<IEnumerable<DriveObject>> GetDrivesFunc { get; set; }

        public static IEnumerable<DriveObject> Drives
        {
            get { return GetDrivesFunc(); }
        }


    }
}



