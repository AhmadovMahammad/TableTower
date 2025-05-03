using TableTower.Core.Enums;

namespace TableTower.Core.Extensions;
public static class AlignmentExtension
{
    public static string ApplyAlignment(this string text, int width, HorizontalAlignment alignment)
    {
        if (text.Length > width)
        {
            return string.Concat(text.AsSpan(0, width - 3), "...");
        }

        int inflate = width - text.Length;

        return alignment switch
        {
            HorizontalAlignment.Left => text.PadRight(width),
            HorizontalAlignment.Center =>
                new string(' ', inflate / 2) +
                text +
                new string(' ', width - text.Length - inflate / 2),
            HorizontalAlignment.Right => text.PadLeft(width),
            _ => text,
        };
    }
}
