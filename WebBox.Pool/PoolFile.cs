using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBox.Data.Pool
{
    public class PoolFile
    {
        public string Origin { get; set; }
        public string Route { get; set; }

        public string Id { get; internal set; }

        public bool Exists { get; internal set; }

        public long? Length { get; internal set; }

        public string CRC32 { get; internal set; }
        public string MD5 { get; internal set; }
        public string SHA1 { get; internal set; }

        private string _machine;
        public string GetMachine()
        {
            return _machine;
        }

        internal void SetMachine(string value)
        {
            _machine = value;
        }

        private string _physicalPath;
        public string GetPhysicalPath()
        {
            return _physicalPath;
        }

        internal void SetPhysicalPath(string value)
        {
            _physicalPath = value;
        }

        private DateTime? _creationTimeUtc;     

        internal void SetCreationTimeUtc(DateTime value)
        {
            _creationTimeUtc = value;
        }

        private DateTime? _lastWriteTimeUtc;

        internal DateTime? GetCreationTimeUtc()
        {
            return _creationTimeUtc;
        }

        internal void SetLastWriteTimeUtc(DateTime value)
        {
            _lastWriteTimeUtc = value;
        }

        internal DateTime? GetLastWriteTimeUtc()
        {
            return _lastWriteTimeUtc;
        }

        public PoolFile(string id)
        {
            Id = id;
        }

 


    }
}
