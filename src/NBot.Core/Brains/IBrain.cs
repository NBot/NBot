namespace NBot.Core.Brains
{
    public interface IBrain
    {
        object GetValue(string key);
        void SetValue(string key, object value);
        bool ContainsKey(string key);
        void RemoveKey(string key);
    }
}