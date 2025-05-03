using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTower.Core.Themes;
public class RoundedTheme : ITheme
{
    public char TopLeftCorner => '╭';
    public char TopRightCorner => '╮';
    public char BottomLeftCorner => '╰';
    public char BottomRightCorner => '╯';
    
    public char TopTee => '┬';
    public char BottomTee => '┴';
    public char LeftTee => '├';
    public char RightTee => '┤';
    
    public char HorizontalLine => '─';
    public char VerticalLine => '│';
    public char Intersection => '┼';

    public ConsoleColor? HeaderForeground => ConsoleColor.Cyan;
    public ConsoleColor? HeaderBackground => null;
    public ConsoleColor? BorderColor => ConsoleColor.DarkCyan;

    public string GetName() => "Rounded";
}
