using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Models;

public class Tile
{
    public int XPosition { get; set; }
    public int YPosition { get; set; }
    public TileValue TileValue = TileValue.Empty;
    public bool IsEmpty => TileValue == TileValue.Empty;
}

public enum TileValue
{
    Empty = 0,
    SnakeBody = 1,
    SnakeHead = 2,
    Apple = 3
}

public static class TileExtensions
{
    public static string GetTileIcon(TileValue tileValue)
    {
        return tileValue switch
        {
            TileValue.Empty => " ",
            TileValue.SnakeBody => "■",
            TileValue.SnakeHead => "●",
            TileValue.Apple => "✦",
            _ => throw new ArgumentOutOfRangeException(nameof(tileValue))
        };
    }
}