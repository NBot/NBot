using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NBot.Core.Extensions;

namespace NBot.Core.Brains
{
    public class FileBrain : IBrain
    {
        private static readonly object _locker = new object();
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
        private readonly string _fileStorragePath;

        public FileBrain(string fileStorragePath)
        {
            _fileStorragePath = fileStorragePath;

            if (!Directory.Exists(_fileStorragePath))
            {
                Directory.CreateDirectory(_fileStorragePath);
            }
        }

        #region IBrain Members

        public object GetValue(string key)
        {
            lock (_locker)
            {
                if (KeyExists(key))
                {
                    return GetKeyValue(key);
                }
            }

            return null;
        }

        public void SetValue(string key, object value)
        {
            lock (_locker)
            {
                _cache[key] = value;

                // Run this in the background
                Task.Factory.StartNew(() => SetKeyValue(key, value));
            }
        }

        public bool ContainsKey(string key)
        {
            lock (_locker)
            {
                return KeyExists(key);
            }
        }

        #endregion

        private void SetKeyValue(string key, object value)
        {
            using (StreamWriter stream = File.CreateText(GetKeyFile(key)))
            {
                stream.Write(value.ToJson());
            }
        }

        private object GetKeyValue(string key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }

            string json;

            using (StreamReader stream = File.OpenText(GetKeyFile(key)))
            {
                json = stream.ReadToEnd();
            }

            _cache[key] = json.FromJson<object>();

            return _cache[key];
        }

        private string GetKeyFile(string key)
        {
            return Path.Combine(_fileStorragePath, string.Format("{0}.key", key));
        }

        private bool KeyExists(string key)
        {
            return _cache.ContainsKey(key) || File.Exists(GetKeyFile(key));
        }
    }
}