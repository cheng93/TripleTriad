using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TripleTriad.Logic.Entities;

namespace TripleTriad.Logic.Extensions
{
    public static class GameDataExtensions
    {
        public static string ToJson(this GameData data)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.Converters.Add(new StringEnumConverter());
            return JsonConvert.SerializeObject(data, serializerSettings);
        }
    }
}