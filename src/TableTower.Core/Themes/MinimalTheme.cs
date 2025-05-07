namespace TableTower.Core.Themes;
public class MinimalTheme : ITheme
{
    public char TopLeftCorner => ' ';
    public char TopRightCorner => ' ';
    public char BottomLeftCorner => ' ';
    public char BottomRightCorner => ' ';

    public char TopTee => '-';
    public char BottomTee => '-';
    public char LeftTee => '|';
    public char RightTee => '|';

    public char HorizontalLine => '-';
    public char VerticalLine => '|';
    public char Intersection => '+';

    public ConsoleColor? HeaderForeground => ConsoleColor.Yellow;
    public ConsoleColor? HeaderBackground => null;
    public ConsoleColor? BorderColor => null;

    public string GetName() => "Minimal";
}