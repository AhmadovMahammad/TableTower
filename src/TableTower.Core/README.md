# Table Tower

**TableTower** is a powerful and flexible .NET library for building and rendering beautiful console tables with minimal effort. It provides a fluent API for creating customizable tables with various themes, pagination support, and text formatting features.

## Features

- Multiple built-in themes: ASCII, Minimal, Classic, Double Line, Rounded, NoBorder
- Fluent API using builder pattern
- Pagination support for large datasets
- Automatic data binding from both any collection of objects and non list data
- Column customization: alignment, width, and more
- Text wrapping support
- High performance and low memory usage

## Installation

```csharp
dotnet add package TableTower.Core --version 1.0.9
```

## Basic Usage

### 1. Automatic Data Binding

```csharp
using TableTower.Core.Builder;
using TableTower.Core.Rendering;
using TableTower.Core.Themes;

TableBuilder builder = new TableBuilder(opt =>
{
    opt.Title = "Users";
    opt.ShowRowLines = true;
    opt.WrapData = false;
    opt.EnableDataCount = true;
})
.WithDataCollection(InMemoryDatabase.Users) // .WithData(InMemoryDatabase.NonListData)
.WithTheme(new RoundedTheme());

var table = builder.Build();
new ConsoleRenderer().Print(table);
```

### 2. Manual Columns with Custom Settings

```csharp
var builder = new TableBuilder(opt => { opt.Title = "Choosen Users Details"; })
    .WithColumns("ID", "Name")
    .WithColumn("Country", HorizontalAlignment.Center)
    .WithColumn("Occupation", HorizontalAlignment.Right, 30)
    .WithTheme(theme);

foreach (var user in InMemoryDatabase.Users)
{
    builder.AddRow(user.ID, user.Name, user.Country, user.Occupation);
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
    opt.WrapData = false;
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
builder.WithDataCollection(users);

// Binding with predefined columns
builder.WithDataCollection(users, usePredefinedColumns: true); // .WithData(InMemoryDatabase.NonListData)
```

## Formatted Rows and Custom Cell Configuration

You can control each cell’s alignment and optional color using `AddFormattedRow`.

```csharp
var builder = new TableBuilder(opt => { opt.Title = "Formatted Users Details"; })
    .WithColumns("ID", "Name", "Occupation", "Country")
    .WithTheme(theme);

foreach (var user in InMemoryDatabase.Users)
{
    builder.AddFormattedRow(
        (user.ID, HorizontalAlignment.Center, ConsoleColor.Yellow),
        (user.Name, HorizontalAlignment.Left, ConsoleColor.Green),
        (user.Occupation, HorizontalAlignment.Center, ConsoleColor.Cyan),
        (user.Country, HorizontalAlignment.Right, ConsoleColor.Magenta)
    );
}

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
        .WithDataCollection(data) // .WithData(InMemoryDatabase.NonListData)
        .WithTheme(new RoundedTheme())
        .Build();
});

consolePager.Run();
```

### Example visuals in [GitHub](https://github.com/AhmadovMahammad/TableTower):

## Themes

TableTower includes the following themes:

```
+-------------+----------+----------+
| ASCII       | Classic  | Rounded  |
| Double Line | Minimal  | NoBorder |
+-------------+----------+----------+
```

### Example visuals in [GitHub](https://github.com/AhmadovMahammad/TableTower):

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
        TableBuilder builder = new TableBuilder(opt =>
        {
            opt.Title = "Users";
            opt.ShowRowLines = false;
            opt.WrapData = false;
            opt.EnableDataCount = true;
        })
        .WithDataCollection(InMemoryDatabase.Users) // .WithData(InMemoryDatabase.NonListData)
        .WithTheme(theme);

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
