# Table Tower

**TableTower** is a powerful and flexible .NET library for building and rendering beautiful console tables with minimal effort. It provides a fluent API for creating customizable tables with various themes, pagination support, and text formatting features.

## Features

- Multiple built-in themes: ASCII, Minimal, Classic, Double Line, Rounded, NoBorder
- Fluent API using builder pattern
- Pagination support for large datasets
- Automatic data binding from any collection of objects
- Column customization: alignment, width, and more
- Text wrapping support
- High performance and low memory usage

## Installation

```csharp
dotnet add package TableTower.Core --version 1.0.8
```

## Basic Usage

### Automatic Data Binding

```csharp
using TableTower.Core.Builder;
using TableTower.Core.Rendering;
using TableTower.Core.Themes;

var builder = new TableBuilder(opt =>
{
    opt.Title = "Team Members";
    opt.WrapData = false;
})
.WithData(users)
.WithTheme(new RoundedTheme());

var table = builder.Build();
new ConsoleRenderer().Print(table);
```

### Manual Columns with Custom Settings

```csharp
var builder = new TableBuilder()
    .WithTitle("Team Members")
    .WithColumns("ID", "Name", "Occupation", "Country")
    .WithColumn("Description", HorizontalAlignment.Right, 40)
    .WithTheme(new BoxTheme())
    .WrapData(true);

foreach (User user in users)
{
    builder.AddRow(user.ID, user.Name, user.Occupation, user.Country, user.Description);
}

var table = builder.Build();
new ConsoleRenderer().Print(table);
```

## Table Options

You can configure core rendering and behavior via `TableOptions`.

```csharp
var builder = new TableBuilder(opt =>
{
    opt.Title = "My Table";
    opt.WrapData = true;
    opt.ShowRowLines = true;
    opt.EnableDataCount = true;
});
```

Available options include:

- `Title`: Set the table title
- `WrapData`: Enable or disable text wrapping
- `ShowRowLines`: Toggle row separator lines
- `EnableDataCount`: Show the row count at the bottom

## Adding Columns

```csharp
// Default columns
builder.WithColumns("ID", "Name", "Occupation");

// Custom column with alignment and width
builder.WithColumn("Description", HorizontalAlignment.Right, 40);
```

## Adding Data

```csharp
// Manual row
builder.AddRow(1, "Mahammad Ahmadov", "Developer", "Some long description here...");

// Automatic binding
builder.WithData(users);

// Binding with predefined columns
builder.WithData(users, usePredefinedColumns: true);
```

## Formatted Rows and Custom Cell Configuration

You can control each cell’s alignment and optional color using `AddFormattedRow`.

```csharp
var builder = new TableBuilder(opt =>
{
    opt.Title = "Custom Formatted Row";
    opt.WrapData = true;
})
.WithData(InMemoryDatabase.Users)
.AddFormattedRow(
    (1, HorizontalAlignment.Left, null),
    ("Filankes", HorizontalAlignment.Left, null),
    ("None", HorizontalAlignment.Center, null),
    ("Sumgait", HorizontalAlignment.Right, null),
    ("Long Description", HorizontalAlignment.Right, null)
)
.WithTheme(new RoundedTheme());

var table = builder.Build();
new ConsoleRenderer().Print(table);
```

Each tuple consists of (value, alignment, optional color).

## Pagination Support

```csharp
IPager<User> pager = new DefaultPager<User>(users, pageSize: 10);

var consolePager = new ConsolePager<User>(pager, data =>
{
    return new TableBuilder()
        .WithData(data)
        .WithTheme(new RoundedTheme())
        .Build();
});

consolePager.Run();
```

### Example visuals:

> ![image](https://github.com/user-attachments/assets/89f5f1c0-bed1-4ee5-b444-7f6eba2bea84) > ![image](https://github.com/user-attachments/assets/49ab4915-70d8-4c9a-884a-330610758fe0)

## Themes

TableTower includes the following themes:

```
+-------------+----------+----------+
| ASCII       | Classic  | Rounded  |
| Double Line | Minimal  | NoBorder |
+-------------+----------+----------+
```

Example visuals:

> ![image](https://github.com/user-attachments/assets/d0092feb-171b-403e-8753-fd4e7d7dfce5) > ![image](https://github.com/user-attachments/assets/24cd99b0-0ba2-40c0-99f5-4f700ab3e0ea)

## Performance Benchmarks

TableTower is designed for high performance even on large datasets:

```
BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.3155/23H2/2023Update/SunValley3)
13th Gen Intel Core i7-1355U, 1 CPU, 12 logical and 10 physical cores
.NET SDK 9.0.102

[Host] : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2
DefaultJob : .NET 8.0.12 (8.0.1224.60305), X64 RyuJIT AVX2

```

| Method            |       Mean |      Error |     StdDev |     Gen0 |     Gen1 |    Gen2 | Allocated |
| ----------------- | ---------: | ---------: | ---------: | -------: | -------: | ------: | --------: |
| Render_10_Users   |   2.945 μs |  0.0459 μs |  0.0429 μs |   1.7242 |   0.0610 |       - |  10.58 KB |
| Render_100_Users  |  29.033 μs |  1.5081 μs |  4.3511 μs |  13.3057 |   2.1973 |       - |  81.77 KB |
| Render_1000_Users | 427.705 μs | 10.1782 μs | 29.0389 μs | 132.8125 | 132.3242 | 66.4063 | 817.29 KB |

## Complete Sample Application

```csharp
using System.Text;
using TableTower.Core.Builder;
using TableTower.Core.Rendering;
using TableTower.Core.Themes;
using TableTower.Data;

namespace BasicSample;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var builder = new TableBuilder(opt =>
        {
            opt.Title = "Team Members";
            opt.WrapData = false;
        })
        .WithData(InMemoryDatabase.Users)
        .WithTheme(new RoundedTheme());

        var table = builder.Build();
        new ConsoleRenderer().Print(table);
    }
}
```

## Contributing

We welcome contributions from everyone.

Before you begin, ensure you have a local copy of the repository and the latest changes from the `main` branch. When you are ready to work on a new feature or fix, create a new branch using a clear and descriptive name, for example `feature/add-pagination-support` or `fix/table-rendering-bug`.

Make your changes in that branch, writing clear commit messages that explain what you did and why. Once your work is complete, push the branch to your fork or directly to the repository and open a pull request against `main`. In your pull request description, include a concise summary of the changes and any relevant context or testing details. We will review your pull request as soon as possible, provide feedback if needed, and merge it once it meets project guidelines.

Thank you for contributing to TableTower and helping improve its functionality and usability.
