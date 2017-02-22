using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebBox.Data.Drive
{
    public class DirectoryObject : FileSystemObject
    {
        private DirectoryObject[] _directories = null;
        public DirectoryObject[] Directories
        {
            get
            {
                return _directories;
            }
        }

        private FileObject[] _files = null;
        public FileObject[] Files
        {
            get
            {
                return _files;
            }
        }

        private DirectoryInfo _dir = null;
        private string _searchPattern;
        private SearchOption _searchOption;

        public DirectoryObject(string origin, string route, string path, string searchPattern, string searchOption)
            : base(origin, route, path)
        {
            _searchPattern = string.IsNullOrWhiteSpace(searchPattern) ? "*" : searchPattern;

            string loweredSearchOption = string.IsNullOrWhiteSpace(searchOption) ? "topdirectoryonly" : searchOption.ToLower();
            _searchOption = (loweredSearchOption == "all" || loweredSearchOption == "alldirectories") ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            if (Path == "/")
            {
                IsDrive = true;
            }
            else
            {
                IsDirectory = true;
            }

            if (PhysicalPath == null)
            {
                Exists = false;
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(PhysicalPath);
            Exists = dir.Exists;

            if (Exists)
            {
                if (IsDirectory)
                {
                    SetObject(dir);
                }
                _dir = dir;
            }
        }

        private DirectoryObject(string origin, string route, string path, string searchPattern, SearchOption searchOption)
            : base(origin, route, path)
        {
            IsDirectory = true;

            _searchPattern = searchPattern;

            if (PhysicalPath == null)
            {
                Exists = false;
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(PhysicalPath);
            SetObject(dir);

            if (Exists && searchOption == SearchOption.AllDirectories)
            {
                _dir = dir;
            }
        }
        
        private void SetChildren()
        {
            if (_dir==null)
            {
                _directories = new DirectoryObject[0];
                _files = new FileObject[0];
                return;
            }

            List<DirectoryObject> dirs = new List<DirectoryObject>();
            List<FileObject> files = new List<FileObject>();
            FileSystemInfo[] infos = _dir.GetFileSystemInfos(_searchPattern);
            foreach (FileSystemInfo info in infos)
            {
                if (info is DirectoryInfo)
                {
                    dirs.Add(new DirectoryObject(Origin, Route, this.Path + "/" + info.Name, _searchPattern, _searchOption));
                    continue;
                }
                if (info is FileInfo)
                {
                    files.Add(new FileObject(Origin, Route, this.Path + "/" + info.Name, false));
                }
            }
            _directories = dirs.ToArray();
            _files = files.ToArray();
        }


    }
}