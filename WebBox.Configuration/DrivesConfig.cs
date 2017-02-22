using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Configuration
{
    public class DrivesConfigSourceCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DriveConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DriveConfigurationElement)element).Route;
        }
    }

    public class DriveConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("route", IsRequired = true)]
        public string Route
        {
            get { return (string)this["route"]; }
            set { this["route"] = value; }
        }

        [ConfigurationProperty("directory", IsRequired = true)]
        public string Directory
        {
            get { return (string)this["directory"]; }
            set { this["directory"] = value; }
        }

        [ConfigurationProperty("description", IsRequired = false)]
        public string Description
        {
            get { return (string)this["description"]; }
            set { this["description"] = value; }
        }
    }

}
