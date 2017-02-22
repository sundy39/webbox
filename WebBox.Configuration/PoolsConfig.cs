using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Configuration
{
    public class PoolsConfigSourceCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PoolConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PoolConfigurationElement)element).Route;
        }

    }

    public class PoolConfigurationElement : ConfigurationElement
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

        [ConfigurationProperty("infoAccessor", IsRequired = true)]
        public string InfoAccessor
        {
            get { return (string)this["infoAccessor"]; }
            set { this["infoAccessor"] = value; }
        }

        [ConfigurationProperty("saveStrategy", IsRequired = false, DefaultValue = "???")]
        public string SaveStrategy
        {
            get { return (string)this["saveStrategy"]; }
            set { this["saveStrategy"] = value; }
        }

    }

}
