using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Data
{
    public class MachineObject
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Origin { get; private set; }
        public string Description { get; private set; }

        public MachineObject(string id, string name, string origin, string description)
        {
            Id = id;
            Name = name;
            Origin = origin;
            Description = description;
        }

        public static Func<MachineObject> GetLocalFunc { get; set; }

        public static MachineObject Local
        {
            get { return GetLocalFunc(); }
        }


    }
}
