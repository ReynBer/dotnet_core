using BenchmarkDotNet.Running;
using System;

namespace Core.Benchmark
{
    public class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkJsonReader>();
            Console.WriteLine($"Press Enter to quit !");
            Console.ReadLine();
        }
    }
}
