using Core.Common;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Core.Serialization.Tests
{
    public class PersonSerializer : SnakeCaseSerializer<Person>
    {
        public PersonSerializer() : base(Formatting.Indented) { }
    }

    public class Person : IEquatable<Person>
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int[] Numbers { get; set; }
        public Person[] Friends { get; set; }

        public bool Equals(Person other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return
                FirstName == other.FirstName
                && LastName == other.LastName
                && new ListComparer<int>().Equals(Numbers, other.Numbers)
                && new ListComparer<Person>().Equals(Friends, other.Friends);
        }

        public override bool Equals(object obj)
            => Equals(obj as Person);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 0;
                hash = hash * 397 ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                hash = hash * 397 ^ (LastName != null ? LastName.GetHashCode() : 0);
                return hash;
            }
        }

        public static IStringSerializer<Person> Serializer { get => new PersonSerializer(); }

        public override string ToString()
            => string.Join(", ",
                new string[] { FirstName != null ? "FirstName: {FirstName}" : string.Empty
                    , FirstName != null ? "FirstName: {FirstName}" : string.Empty
                    , Numbers != null ? $"Numbers: [{string.Join(", ", Numbers)}]" : string.Empty
                    , Friends != null ? $"Friends: [{string.Join(", ", Friends.Select(f => f.ToString()).ToArray())}]" : string.Empty }
                .Where(s => !string.IsNullOrEmpty(s)));
    }
}
