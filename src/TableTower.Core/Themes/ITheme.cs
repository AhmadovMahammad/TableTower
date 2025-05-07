namespace TableTower.Core.Themes;
public interface ITheme
{
    // corners
    char TopLeftCorner { get; }
    char TopRightCorner { get; }
    char BottomLeftCorner { get; }
    char BottomRightCorner { get; }

    // tees
    char TopTee { get; }
    char BottomTee { get; }
    char LeftTee { get; }
    char RightTee { get; }

    // lines
    char HorizontalLine { get; }
    char VerticalLine { get; }
    char Intersection { get; }

    ConsoleColor? HeaderForeground { get; }
    ConsoleColor? HeaderBackground { get; }
    ConsoleColor? BorderColor { get; }

    string GetName();
}
