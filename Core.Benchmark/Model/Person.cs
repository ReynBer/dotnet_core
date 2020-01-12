using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Benchmark.Model
{
    public class Person
    {
        public int Id { get; set; }
        public Name Name { get; set; }
        public string Email { get; set; }

        public override string ToString()
            => $"Id:{Id}, FirstName: {Name?.First}, LastName: {Name?.Last}, Email: {Email}";
    }

    public class Name
    {
        public string First { get; set; }
        public string Last { get; set; }
    }
}
