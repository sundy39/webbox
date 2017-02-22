using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Data.Pool
{
    public interface IInfoAccessor
    {
        PoolFile Find(string pool, string id);
        int Count(string pool, long? length, string crc32, string md5, string sha1);
        IEnumerable<PoolFile> Get(string pool, long? length, string crc32, string md5, string sha1);

        void Add(PoolFile poolFile);
        void Update(PoolFile poolFile);
        void Delete(string pool, string id);       
    }
}
