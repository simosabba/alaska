using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Caching.Instances
{
    public class FilesCache : CacheInstance
    {
        private Dictionary<string, FileSystemWatcher> _watchers = new Dictionary<string, FileSystemWatcher>();

        public FilesCache()
        { }

        public string GetFile(string path, bool watchFileChanges)
        {
            var key = path.ToLower().TrimEnd('\\');
            if (watchFileChanges)
                RegisterFileWatcher(key);
            return Retreive<string>(key, () => ReadFile(key), TimeSpan.MaxValue);
        }

        private string ReadFile(string path)
        {
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }

        private void RegisterFileWatcher(string path)
        {
            if (_watchers.ContainsKey(path))
                return;

            lock (this)
            {
                //check if watcher has alreaby been concurrently initialized
                if (_watchers.ContainsKey(path))
                    return;

                if (!File.Exists(path))
                    return;

                var watcher = CreateWatcher(path);
                _watchers.Add(path, watcher);
            }
        }

        private FileSystemWatcher CreateWatcher(string path)
        {
            var directory = Path.GetDirectoryName(path);
            var file = Path.GetFileName(path);
            var watcher = new FileSystemWatcher(directory, file);
            watcher.Changed += delegate (object sender, FileSystemEventArgs e)
            {
                if (e.FullPath.Equals(path, StringComparison.OrdinalIgnoreCase))
                    Remove(path);
            };
            watcher.Renamed += delegate (object sender, RenamedEventArgs e)
            {
                if (!File.Exists(path))
                {
                    Remove(path);
                    UnregisterWatcher(path, watcher);
                }
            };
            watcher.Deleted += delegate (object sender, FileSystemEventArgs e)
            {
                if (!File.Exists(path))
                {
                    Remove(path);
                    UnregisterWatcher(path, watcher);
                }
            };
            watcher.EnableRaisingEvents = true;
            return watcher;
        }

        private void UnregisterWatcher(string path, FileSystemWatcher watcher)
        {
            watcher.Dispose();
            _watchers.Remove(path);
        }
    }
}
