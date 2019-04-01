using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace TripleTriad.Requests.Messages
{
    public interface IMessageFactory<TMessageData>
        where TMessageData : IMessageData
    {
        Task<string> Create(TMessageData data);
    }

    public abstract class MessageFactory<TMessageData> : IMessageFactory<TMessageData>
        where TMessageData : IMessageData
    {
        protected abstract Task<object> GetMessage(TMessageData data);

        protected virtual JsonSerializerSettings GetSerializerSettings()
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            serializerSettings.Converters.Add(new StringEnumConverter());
            return serializerSettings;
        }

        public async Task<string> Create(TMessageData data)
            => JsonConvert.SerializeObject(await this.GetMessage(data), GetSerializerSettings());
    }
}