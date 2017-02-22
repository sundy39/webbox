using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebBox.Data.Pool;

namespace WebBox.Web.Http.Pool.Models
{
    internal class UploadModel
    {
        public PoolFile Upload(string origin, string route, string pool)
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            if (request.Files.Count == 0)
            {
                return new PoolFile(null);
                //return new PoolFile(origin, route, pool, null);
            }
            if (request.Files.Count == 1)
            {
                string physicalDirectory = GetPhysicalDirectory(pool);
                string id = GenerateId();
                string physicalPath = Path.Combine(physicalDirectory, id);

                //
                string dirName = Path.GetDirectoryName(physicalPath);
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }
                            
                //
                request.Files[0].SaveAs(physicalPath);

                //
                //PoolFile poolFile = new PoolFile(origin, route, pool, id);
                PoolFile poolFile = new PoolFile(id);
                new PoolRepository().SaveFileInfo(poolFile, physicalPath);

                return poolFile;
            }

            throw new NotSupportedException();
        }

        private string GetPhysicalDirectory(string pool)
        {
            return pool;
        }

        private string GenerateId()
        {
            return Guid.NewGuid().ToString("N");
        }


    }
}