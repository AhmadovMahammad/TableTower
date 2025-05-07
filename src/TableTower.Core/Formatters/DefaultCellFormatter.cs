namespace TableTower.Core.Formatters;
public interface ICellFormatter
{
    string Format(object? value);
    bool CanFormat(Type type);
}

public class DefaultCellFormatter : ICellFormatter
{
    public string Format(object? value)
    {
        if (value == null)
        {
            return string.Empty;
        }

        Type type = value.GetType();
        if (CanFormat(type))
        {
            return value.ToString() ?? string.Empty;
        }

        return type.Name ?? "Unknown";
    }

    public bool CanFormat(Type type)
    {
        return type.IsValueType || type == typeof(string);
    }
}
