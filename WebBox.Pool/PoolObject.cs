using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Data.Pool
{
    public class PoolObject
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

        private IInfoAccessor _infoAccessor;
        public IInfoAccessor GetInfoAccessor()
        {
            return _infoAccessor;
        }

        private ISaveStrategy _saveStrategy;
        public ISaveStrategy GetSaveStrategy()
        {
            return _saveStrategy;
        }

        public PoolObject(string route, string directory, string description, IInfoAccessor infoAccessor, ISaveStrategy saveStrategy)
        {
            Route = route;
            _directory = directory;
            Description = description;
            _infoAccessor = infoAccessor;
            _saveStrategy = saveStrategy;
        }

        public static Func<IEnumerable<PoolObject>> GetPoolsFunc { get; set; }

        public static IEnumerable<PoolObject> Pools
        {
            get { return GetPoolsFunc(); }
        }

        public static PoolObject Default
        {
            get
            {
                return Pools.FirstOrDefault(p => p.Route == string.Empty);
            }
        }


    }
}
