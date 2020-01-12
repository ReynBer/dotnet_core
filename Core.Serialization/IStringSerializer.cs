namespace Core.Serialization
{
    public interface IStringSerializer<T> where T : class
    {
        string Serialize(T value);
        T Deserialize(string value);
    }
}
