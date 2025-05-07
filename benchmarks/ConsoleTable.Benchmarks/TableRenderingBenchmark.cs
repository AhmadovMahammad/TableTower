using BenchmarkDotNet.Attributes;
using ConsoleTable.Core.Builder;
using ConsoleTable.Core.Rendering;
using ConsoleTable.Core.Themes;
using ConsoleTable.Data;
using ConsoleTable.Data.Models;
using System.Text;

namespace ConsoleTable.Benchmarks;

[MemoryDiagnoser]
public class TableRenderingBenchmark
{
    private ITheme _theme = null!;
    private ConsoleRenderer _renderer = null!;

    private List<User> _users_10 = new(10);
    private List<User> _users_100 = new(100);
    private List<User> _users_1000 = new(1000);

    [GlobalSetup]
    public void Setup()
    {
        Console.OutputEncoding = Encoding.UTF8;
        _theme = new ClassicTheme();
        _renderer = new ConsoleRenderer();

        _users_10 = [.. InMemoryDatabase.Users];
        _users_100 = Enumerable.Repeat(InMemoryDatabase.Users, 10).SelectMany(x => x).Take(100).ToList();
        _users_1000 = Enumerable.Repeat(InMemoryDatabase.Users, 100).SelectMany(x => x).Take(1000).ToList();
    }

    [Benchmark]
    public void Render_10_Users()
    {
        Render(_users_10);
    }

    [Benchmark]
    public void Render_100_Users()
    {
        Render(_users_100);
    }

    [Benchmark]
    public void Render_1000_Users()
    {
        Render(_users_1000);
    }

    private void Render(List<User> users)
    {
        var table = new TableBuilder(opt => { opt.Title = "Benchmark Test"; opt.WrapData = false; })
            .WithData(users)
            .WithTheme(_theme)
            .Build();

        _renderer.Render(table);
    }
}
