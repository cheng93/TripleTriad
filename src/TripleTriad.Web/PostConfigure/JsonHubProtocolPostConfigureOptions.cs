using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;

namespace TripleTriad.Web.PostConfigure
{
    public class JsonHubProtocolPostConfigureOptions : IPostConfigureOptions<JsonHubProtocolOptions>
    {
        public void PostConfigure(string name, JsonHubProtocolOptions options)
        {
            options.PayloadSerializerSettings.Converters.Add(new StringEnumConverter());
        }
    }
}