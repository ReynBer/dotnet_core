using Core.Serialization;
using Newtonsoft.Json;

namespace Core.Benchmark
{
    public class PersonSerializer : CamelCaseSerializer<Model.Person>
    {
        public PersonSerializer() : base(Formatting.Indented) { }
    }
}
