using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBox.Data;

namespace WebBox.Configuration
{
    public static class MachineGetter
    {
        public static MachineObject GetLocal()
        {
            WebBoxConfigurationSection config = (WebBoxConfigurationSection)ConfigurationManager.GetSection("webBox");
            MachineObject local = new MachineObject(config.Machine.Id, config.Machine.Name, config.Machine.Origin, config.Machine.Description);
            return local;
        }
    }
}
