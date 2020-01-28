using System.Linq;
using Xunit;

namespace Core.Serialization.Tests
{

    public class JsonReaderFileTest
    {
        [Fact]
        public void CheckJsonReaderFile()
        {
            var personsExpected = new[]
            {
                new Person { FirstName = "Savannah", LastName = "Rosario", Numbers = new [] { 1 }, Friends = new [] { new Person { FirstName="Jack"} } }
                , new Person { FirstName = "Haynes", LastName = "Hanson", Numbers = new [] { 3, 5 }, Friends = new Person[0] { } } 
            };
            var reader = new JsonReaderFile<Person>(Person.Serializer);
            var personsRead = reader.ReadValues("input.json").ToList();
            Assert.Equal(personsExpected, personsRead, new PersonComparer());
        }
    }
}
