namespace MineSweeper.Models;

public class Tile
{
    public int Value { get; set; } = 0;
    public bool IsFlagged { get; set; } = false;
    public bool IsRevealed { get; set; } = false;
}
