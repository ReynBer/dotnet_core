using BenchmarkDotNet.Attributes;
using Core.Serialization;
using System.Threading.Tasks;

namespace Core.Benchmark
{
    [SimpleJob(targetCount: 100)]
    [MinColumn, Q1Column, Q3Column, MaxColumn]
    public class BenchmarkJsonReader
    {
        private JsonReaderFile<Model.Person> Reader => new JsonReaderFile<Model.Person>(new PersonSerializer());
        public BenchmarkJsonReader()
        {

        }

        private static void DoNothing(Model.Person p) { }

        [Benchmark]
        public async Task ToRunAsync()
        {
            await foreach (var person in Reader.ReadValuesAsync("input.json"))
                DoNothing(person);
        }

        [Benchmark]
        public void ToRun()
        {
            foreach (var p in Reader.ReadValues("input.json"))
                DoNothing(p);
        }
    }
}
