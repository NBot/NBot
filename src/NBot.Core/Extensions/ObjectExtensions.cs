using Newtonsoft.Json;

namespace NBot.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object target)
        {
            return JsonConvert.SerializeObject(target, Formatting.None);
        }
    }
}