using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Snake.Models;
using Snake.Snake;

namespace Snake.Board;

public static class BoardGenerator
{
    public static Tile[,] GenerateBoard((int row, int col) size)
    {
        Tile[,] board = new Tile[size.row, size.col];

        for (int r = 0; r < size.row; r++)
        {
            for (int c = 0; c < size.col; c++)
            {
                Tile tile = new()
                {
                    XPosition = c,
                    YPosition = r
                };
                board[r, c] = tile;
            }
        }

        return board;
    }
}

public static class BoardRendering
{
    public static void DisplayBoard(Tile[,] board)
    {
        (int rowSize, int colSize) = (board.GetLength(0), board.GetLength(1));
        StringBuilder sb = new();

        string horizontalLine = "+" + new string('-', colSize * 2) + "+\n";

        sb.Append(horizontalLine);

        for (int r = 0; r < rowSize; r++)
        {
            sb.Append('|');
            for (int c = 0; c < colSize; c++)
            {
                sb.Append(TileExtensions.GetTileIcon(board[r, c].TileValue) + " ");
            }
            sb.Append("|\n");
        }

        sb.Append(horizontalLine);

        Console.Write(sb.ToString());
    }
}

public class BoardManager(Tile[,] board)
{
    private readonly Tile[,] Board = board;
    public int RowSize = board.GetLength(0);
    public int ColSize = board.GetLength(1);
    private readonly Random rand = new();

    public Tile? GetTile(int row, int col)
    {
        if (row < 0 || row >= RowSize || col < 0 || col >= ColSize)
        {
            return null;
        }

        return Board[row, col];
    }

    public void ClearBoard()
    {
        for (int r = 0; r < RowSize; r++)
        {
            for (int c = 0; c < ColSize; c++)
            {
                Board[r, c].TileValue = Board[r, c].TileValue == TileValue.Apple ? TileValue.Apple : TileValue.Empty;
            }
        }
    }

    public void SyncSnake(SnakeObject snake)
    {
        List<SnakePart> body = snake.GetBody();

        foreach (SnakePart part in body)
        {
            (int row, int col) = part.Position;
            if (snake.Head == part)
            {
                Board[row, col].TileValue = TileValue.SnakeHead;
            } else
            {
                Board[row, col].TileValue = TileValue.SnakeBody;
            }
        }
    }

    public bool SpawnApple(SnakeObject snake)
    {
        List<(int row, int col)> availableTiles = [];

        for (int r = 0; r < RowSize; r++)
        {
            for (int c = 0; c < ColSize; c++)
            {
                if (Board[r, c].IsEmpty && !snake.Occupies(r, c))
                {
                    availableTiles.Add((r, c));
                }
            }
        }

        if (availableTiles.Count == 0) return false;

        (int randRow, int randCol) = availableTiles[rand.Next(availableTiles.Count)];
        Board[randRow, randCol].TileValue = TileValue.Apple;
        return true;
    }

    public SnakeObject CreateSnake() => new(row: RowSize / 2, col: ColSize / 2);

    public int UpdateSnake(SnakeObject snake, Direction? newDirection = null)
    {
        ClearBoard();

        if (newDirection != null) { snake.ChangeDirection(newDirection.Value); }

        (int dRow, int dCol) = DirectionExtensions.DirectionChange(snake.Direction);
        (int nextRow, int nextCol) = (snake.Head.Position.row + dRow, snake.Head.Position.col + dCol);
        bool appleEaten = GetTile(nextRow, nextCol)?.TileValue == TileValue.Apple;

        snake.Move(appleEaten);

        if (appleEaten)
        {
            bool canSpawn = SpawnApple(snake);
            if (!canSpawn) { return 1; }
        }

        (int hRowC, int hColC) = snake.Head.Position;
        if (GetTile(hRowC, hColC) == null) { return -1; }

        if (snake.BodyOccupies(hRowC, hColC)) { return -1; }

        SyncSnake(snake);

        return 0;
    }
}
