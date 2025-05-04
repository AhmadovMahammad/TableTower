using BenchmarkDotNet.Attributes;
using System.Text;
using TableTower.Core.Builder;
using TableTower.Core.Models;
using TableTower.Core.Rendering;
using TableTower.Core.Themes;
using TableTower.Data;

namespace TableTower.Benchmarks;

[MemoryDiagnoser]
public class TableRenderingBenchmark
{
    private ITheme _theme = null!;
    private ConsoleRenderer _renderer = null!;

    [GlobalSetup]
    public void Setup()
    {
        Console.OutputEncoding = Encoding.UTF8;
        _theme = new ClassicTheme();
        _renderer = new ConsoleRenderer();
    }

    [Benchmark]
    public void Render_10_Users()
    {
        var users = InMemoryDatabase.Users.ToList();
        Render(users);
    }

    [Benchmark]
    public void Render_100_Users()
    {
        var users = Enumerable.Repeat(InMemoryDatabase.Users, 10).SelectMany(x => x).Take(100).ToList();
        Render(users);
    }

    [Benchmark]
    public void Render_1000_Users()
    {
        var users = Enumerable.Repeat(InMemoryDatabase.Users, 100).SelectMany(x => x).Take(1000).ToList();
        Render(users);
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
