using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Configuration
{
    public class MachineConfigurationElement : ConfigurationElement 
    {
        [ConfigurationProperty("id", IsRequired = true)] // MAC Addr
        public string Id
        {
            get { return (string)this["id"]; }
            set { this["id"] = value; }
        }

        [ConfigurationProperty("name", IsRequired = false)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("origin", IsRequired = false)]
        public string Origin
        {
            get { return (string)this["origin"]; }
            set { this["origin"] = value; }
        }

        [ConfigurationProperty("description", IsRequired = false)]
        public string Description
        {
            get { return (string)this["description"]; }
            set { this["description"] = value; }
        }


    }
}
