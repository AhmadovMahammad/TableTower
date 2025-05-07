using BenchmarkDotNet.Running;

namespace ConsoleTable.Benchmarks;
internal class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<TableRenderingBenchmark>();
    }
}