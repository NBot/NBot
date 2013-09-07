using System.Collections.Generic;

namespace NBot.Core.Brains
{
    public class SimpleBrain : IBrain
    {
        private readonly Dictionary<string, object> _memory = new Dictionary<string, object>();

        #region IBrain Members

        public object GetValue(string key)
        {
            return _memory[key];
        }

        public void SetValue(string key, object value)
        {
            _memory[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return _memory.ContainsKey(key);
        }

        #endregion
    }
}