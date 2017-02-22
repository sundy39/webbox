using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBox.Data.Pool;

namespace WebBox.Configuration
{
    public static class PoolsGetter
    {
        public static IEnumerable<PoolObject> GetPools()
        {
            List<PoolObject> poolObjects = new List<PoolObject>();
            WebBoxConfigurationSection config = (WebBoxConfigurationSection)ConfigurationManager.GetSection("webBox");
            foreach (PoolConfigurationElement pool in config.Pools)
            {
                PoolObject poolObject = null; //new PoolObject(pool.Route, pool.Directory, pool.Description, pool.InfoAccessor, pool.SaveStrategy);
                poolObjects.Add(poolObject);
            }
            return poolObjects;
        }


    }
}
