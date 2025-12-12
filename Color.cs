using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


public class Color
{
    public ushort r;
    public ushort g;
    public ushort b;
    public ushort a;


    public static readonly Color Black = new Color(0,0,0,255);
    public static readonly Color White = new Color(255,255,255, 255);
    public static readonly Color Red = new Color(255,0,0, 255);
    public static readonly Color Green = new Color(0,255,0, 255);
    public static readonly Color Blue = new Color(0,0,255, 255);
    public static readonly Color Yellow = new Color(255,255,0, 255);
    public static readonly Color Magenta = new Color(255,0,255, 255);
    public static readonly Color Cyan = new Color(0,255,255, 255);
    public static readonly Color Transparent = new Color(0,0,0, 0);


    public Color()
    {
        r = 0;
        g = 0;
        b = 0;
        a = 255;
    }

    public Color(ushort r, ushort g, ushort b, ushort a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public Color (UInt32 _color)
    {
        r = (ushort)((_color >> 24) & 255);
        g = (ushort)((_color >> 16) & 255);
        g = (ushort)((_color >> 8) & 255);
        a = (ushort)(_color& 255);
    }

    public Color(Color _color)
    {
        r = _color.r;
        g = _color.g;
        b = _color.b;
        a = _color.a;
    }

    public UInt32 ToInteger()
    {
        return (UInt32)(r << 24) | (UInt32)(g << 16) | (UInt32)(b << 8) | (UInt32)a;
    }

    public override string ToString()
    {
        return $"\x1b[38;2;{r};{g};{b}m"; 
    }

}

