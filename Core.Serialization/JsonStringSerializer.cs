using Newtonsoft.Json;

namespace Core.Serialization
{
    public abstract class JsonStringSerializer<T> : IStringSerializer<T>
    where T : class
    {
        protected abstract JsonSerializerSettings Settings { get; }

        public string Serialize(T value)
            => Settings == null
                ? JsonConvert.SerializeObject(value)
                : JsonConvert.SerializeObject(value, Settings);

        public T Deserialize(string json)
            => Settings == null
                ? JsonConvert.DeserializeObject<T>(json)
                : JsonConvert.DeserializeObject<T>(json, Settings);
    }
}
