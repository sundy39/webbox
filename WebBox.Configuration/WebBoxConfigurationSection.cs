using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace WebBox.Configuration
{
    public class WebBoxConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("drives", IsRequired = false)]
        public DrivesConfigSourceCollection Drives
        {
            get { return (DrivesConfigSourceCollection)this["drives"]; }
            set { this["drives"] = value; }
        }

        [ConfigurationProperty("machine", IsRequired = false)]
        public MachineConfigurationElement Machine
        {
            get { return (MachineConfigurationElement)this["machine"]; }
            set { this["machine"] = value; }
        }

        [ConfigurationProperty("pools", IsRequired = false)]
        public PoolsConfigSourceCollection Pools
        {
            get { return (PoolsConfigSourceCollection)this["pools"]; }
            set { this["pools"] = value; }
        }


    }
}