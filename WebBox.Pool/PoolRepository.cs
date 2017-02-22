using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Data.Pool
{
    public class PoolRepository
    {
        private InfoAccessorFactory _infoAccessorFactory = new InfoAccessorFactory();

        private IInfoAccessor CreateInfoAccessor()
        {
            return _infoAccessorFactory.Create();
        }

        public PoolFile Find(string id)
        {
            PoolFile poolFile = new PoolFile(id);
            return poolFile;
        }

        public PoolFile Find(string pool, long? length, string crc32, string md5, string sha1)
        {
            IEnumerable<PoolFile> result = CreateInfoAccessor().Get(pool, length, crc32, md5, sha1);
            int count = result.Count();
            if (count == 0) return new PoolFile(null);
            if (count == 1) return result.First();

            return new PoolFile("more than one") { Exists = true };
        }

        public void SaveFileInfo(PoolFile poolFile, string physicalPath)
        {
            FileInfo fileInfo = new FileInfo(physicalPath);
            poolFile.Exists = fileInfo.Exists;
            poolFile.Length = fileInfo.Length;
            poolFile.SetPhysicalPath(physicalPath);
            poolFile.SetCreationTimeUtc(fileInfo.CreationTimeUtc);
            poolFile.SetLastWriteTimeUtc(fileInfo.LastWriteTimeUtc);

            //
            CreateInfoAccessor().Add(poolFile);
        }

        public void Delete(string pool, string id)
        {
            PoolFile poolFile = Find(id);

            //
            CreateInfoAccessor().Delete(pool, id);

            //
            if (poolFile.Exists)
            {
                File.Delete(poolFile.GetPhysicalPath());
            }
        }


    }
}
