using TableTower.Core.Enums;

namespace TableTower.Core.Extensions;
public static class AlignmentExtension
{
    //public static string ApplyAlignment(this string text, int width, HorizontalAlignment alignment)
    //{
    //    if (text.Length > width)
    //    {
    //        return string.Concat(text.AsSpan(0, width - 3), "...");
    //    }

    //    int inflate = width - text.Length;

    //    return alignment switch
    //    {
    //        HorizontalAlignment.Left => text.PadRight(width),
    //        HorizontalAlignment.Center =>
    //            new string(' ', inflate / 2) +
    //            text +
    //            new string(' ', width - text.Length - inflate / 2),
    //        HorizontalAlignment.Right => text.PadLeft(width),
    //        _ => text,
    //    };
    //}

    public static void ApplyAlignment(this ReadOnlySpan<char> text, Span<char> destination, int width, HorizontalAlignment alignment)
    {
        int textLength = text.Length;

        if (textLength > width)
        {
            text[..(width - 3)].CopyTo(destination);
            "...".AsSpan().CopyTo(destination[(width - 3)..]);
            return;
        }

        int inflate = width - textLength;

        switch (alignment)
        {
            case HorizontalAlignment.Left:
                {
                    text.CopyTo(destination);
                    destination[textLength..].Fill(' ');
                    break;
                }

            case HorizontalAlignment.Center:
                {
                    int leftPadding = inflate / 2;
                    destination[..leftPadding].Fill(' ');
                    text.CopyTo(destination[leftPadding..]);
                    destination[(leftPadding + textLength)..].Fill(' ');
                    break;
                }

            case HorizontalAlignment.Right:
                {
                    destination[..inflate].Fill(' ');
                    text.CopyTo(destination[inflate..]);
                    break;
                }

            default:
                break;
        }
    }

    // Legacy version for backward compatibility
    public static string ApplyAlignment(this string text, int width, HorizontalAlignment alignment)
    {
        Span<char> buffer = stackalloc char[width];
        text.AsSpan().ApplyAlignment(buffer, width, alignment);
        return new string(buffer);
    }
}
