namespace ConsoleTable.Core.Builder;
public sealed class TableOptions
{
    public string? Title { get; set; }
    public bool ShowRowLines { get; set; }
    public bool WrapData { get; set; }
    public bool EnableDataCount { get; set; } = true;
}