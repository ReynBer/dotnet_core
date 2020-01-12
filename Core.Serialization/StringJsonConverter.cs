using Newtonsoft.Json;
using System;

namespace Core.Serialization
{
    public class StringJsonConverter<T> : JsonConverter<T>
        where T : class
    {
        private readonly IStringSerializer<T> _serializer;
        public StringJsonConverter(Type serializerType) : this((IStringSerializer<T>)Activator.CreateInstance(serializerType, Formatting.Indented))
        {
        }

        public StringJsonConverter(IStringSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
            => _serializer.Deserialize(reader.Value.ToString());//.Dump());

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            writer.WriteRawValue(_serializer.Serialize(value));
        }
    }
}
