using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NBot.Core.Extensions
{
    public static class StringExtensions
    {
        public static dynamic FromJson(this string target)
        {
            object result = JsonConvert.DeserializeObject(target);
            return result;
        }

        public static T FromJson<T>(this string target)
        {
            var result = JsonConvert.DeserializeObject<T>(target);
            return result;
        }

        public static T FromJson<T>(this string target, string rootName)
        {
            JObject x = JObject.Parse(target);
            return x[rootName].ToObject<T>();
        }
    }
}