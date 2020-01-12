using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Serialization
{
    public abstract class CamelCaseSerializer<T> : JsonStringSerializer<T>
        where T : class
    {
        private readonly JsonSerializerSettings _serializerSettings;

        protected override JsonSerializerSettings Settings => _serializerSettings;

        protected CamelCaseSerializer(Formatting formatting, params JsonConverter[] converters)
        {
            var contractResolver = new CamelCasePropertyNamesContractResolver();
            _serializerSettings = new JsonSerializerSettings()
            {
                Formatting = formatting,
                ContractResolver = contractResolver,
            };
            foreach (var converter in converters)
                _serializerSettings.Converters.Add(converter);
        }
    }

}
