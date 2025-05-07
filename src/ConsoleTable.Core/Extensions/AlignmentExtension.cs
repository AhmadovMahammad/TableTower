using ConsoleTable.Core.Enums;

namespace ConsoleTable.Core.Extensions;
public static class AlignmentExtension
{
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

    public static string ApplyAlignment(this string text, int width, HorizontalAlignment alignment)
    {
        Span<char> buffer = stackalloc char[width];
        text.AsSpan().ApplyAlignment(buffer, width, alignment);
        return new string(buffer);
    }
}
