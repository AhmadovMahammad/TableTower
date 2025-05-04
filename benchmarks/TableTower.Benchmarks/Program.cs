using BenchmarkDotNet.Running;

namespace TableTower.Benchmarks;
internal class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<TableRenderingBenchmark>();
    }
}