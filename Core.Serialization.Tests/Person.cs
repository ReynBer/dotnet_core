using Core.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Core.Serialization.Tests
{
    public class PersonSerializer : SnakeCaseSerializer<Person>
    {
        public PersonSerializer() : base(Formatting.Indented) { }
    }

    public class PersonComparer : IEqualityComparer<Person>
    {
        private readonly ListComparer<int> numbersComparer = new ListComparer<int>();
        private readonly ListComparer<Person> personsComparer;

        public PersonComparer()
        {
            personsComparer = new ListComparer<Person>(Equals);
        }

        public bool Equals([AllowNull] Person x, [AllowNull] Person y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(null, y) || ReferenceEquals(null, x)) return false;
            return true
                && string.Equals(x.LastName, y.LastName)
                && string.Equals(x.FirstName, y.FirstName)
                && numbersComparer.Equals(x.Numbers, y.Numbers)
                && personsComparer.Equals(x.Friends, y.Friends);
        }

        public int GetHashCode([DisallowNull] Person p)
        {
            unchecked
            {
                int hash = 0;
                hash = hash * 397 ^ (p.FirstName?.GetHashCode() ?? 0);
                hash = hash * 397 ^ (p.LastName?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }

    public class Person 
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int[] Numbers { get; set; }
        public Person[] Friends { get; set; }

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
