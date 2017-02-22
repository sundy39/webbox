using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebBox.Data.Pool;

namespace WebBox.Data.Pool.Extensions
{
    public static class PoolExtensions
    {
        public static string GetDirectory(this IEnumerable<PoolObject> pools, string uri)
        {
            PoolObject poolObject = pools.FirstOrDefault(p => p.Route == uri);
            return (poolObject == null) ? null : poolObject.GetDirectory();
        }

        public static XElement ToElement(this IEnumerable<PoolObject> pools)
        {      
            XElement element = new XElement("Pools");
            foreach (PoolObject pool in pools)
            {
                element.Add(ToElement(pool));
            }
            return element;
        }

        private static XElement ToElement(PoolObject pool)
        {
            XElement element = new XElement("Pool");
            element.SetElementValue("Uri", pool.Route);
            if (string.IsNullOrWhiteSpace(pool.Description))
            {
                element.SetElementValue("Description", string.Empty);
            }
            else
            {
                element.SetElementValue("Description", pool.Description);
            }
            return element;
        }


    }
}
