using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Snake.Snake;

public class SnakePart(int row, int col)
{
    public (int row, int col) Position = (row, col);
}

public class SnakeObject
{
    private Direction Direction = Direction.Right;
    private readonly List<SnakePart> Body = [];
    public SnakePart Head => Body[0];

    public SnakeObject(int row, int col)
    {   
        for (int i = 0; i < 4; i++)
        {
            SnakePart part = new(row, col);
            Body.Add(part);
            col--;
        }
    }

    public void Move(bool AppleEaten)
    {
        (int drow, int dcol) = DirectionExtensions.DirectionChange(Direction);
        SnakePart newHead = new(Head.Position.row + drow, Head.Position.col + dcol);

        Body.Insert(0, newHead);

        if (!AppleEaten)
        {
            Body.RemoveAt(Body.Count - 1);
        }
    }

    public void ChangeDirection(Direction dir)
    {
        Direction OppositeDirection = DirectionExtensions.OppositeDirection(Direction);
        if (OppositeDirection != dir)
        {
            Direction = dir;
        }
    }

    public bool BodyOccupies(int row, int col) => Body.Skip(1).Any(p => p.Position == (row, col));
    public List<SnakePart> GetBody() => Body;
}

public enum Direction
{
    Up,
    Down,
    Right,
    Left
}

public static class DirectionExtensions
{
    public static Direction OppositeDirection(Direction dir)
    {
        return dir switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(dir))
        };
    }

    public static (int row, int col) DirectionChange(Direction dir)
    {
        return dir switch
        {
            Direction.Up => (-1, 0),
            Direction.Down => (1, 0),
            Direction.Left => (0, -1),
            Direction.Right => (0, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(dir))
        };
    }
}
