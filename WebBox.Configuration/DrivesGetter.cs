using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBox.Data.Drive;

namespace WebBox.Configuration
{
    public static class DrivesGetter
    {
        public static IEnumerable<DriveObject> GetDrives()
        {
            List<DriveObject> driveObjects = new List<DriveObject>();
            WebBoxConfigurationSection config = (WebBoxConfigurationSection)ConfigurationManager.GetSection("webBox");
            foreach (DriveConfigurationElement drive in config.Drives)
            {
                DriveObject driveObject = new DriveObject(drive.Route, drive.Directory, drive.Description);
                driveObjects.Add(driveObject);
            }
            return driveObjects;
        }


    }
}
