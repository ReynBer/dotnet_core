using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Serialization
{
    public abstract class SnakeCaseSerializer<T> : JsonStringSerializer<T>
        where T : class
    {
        private readonly JsonSerializerSettings _serializerSettings;

        protected override JsonSerializerSettings Settings => _serializerSettings;

        protected SnakeCaseSerializer(Formatting formatting, params JsonConverter[] converters)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
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
