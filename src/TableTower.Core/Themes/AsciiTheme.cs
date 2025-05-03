namespace TableTower.Core.Themes;
public class AsciiTheme : ITheme
{
    public char TopLeftCorner => '+';
    public char TopRightCorner => '+';
    public char BottomLeftCorner => '+';
    public char BottomRightCorner => '+';
    
    public char TopTee => '+';
    public char BottomTee => '+';
    public char LeftTee => '+';
    public char RightTee => '+';
    
    public char HorizontalLine => '-';
    public char VerticalLine => '|';
    public char Intersection => '+';

    public ConsoleColor? HeaderForeground => ConsoleColor.White;
    public ConsoleColor? HeaderBackground => ConsoleColor.DarkBlue;
    public ConsoleColor? BorderColor => null;

    public string GetName() => "ASCII";
}