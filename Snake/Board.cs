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

    // Quick Tile Grabbing
    public Tile? GetTile(int row, int col)
    {
        if (row < 0 || row >= RowSize || col < 0 || col >= ColSize)
        {
            return null;
        }

        return Board[row, col];
    }

    // Board
    public void ClearBoard()
    {
        for (int r = 0; r < RowSize; r++)
        {
            for (int c = 0; c < ColSize; c++)
            {
                Board[r, c].TileValue = TileValue.Empty;
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

    // Snake Creating / Updating
    public SnakeObject CreateSnake() => new(row: RowSize / 2, col: ColSize / 2);

    public bool UpdateSnake(SnakeObject snake, Direction? newDirection = null)
    {
        // Clear Board
        ClearBoard();

        bool appleEaten = false; // Add Check for apples AFTER apple generation (Leave false for now)

        // Move + Change Direction
        if (newDirection != null)
        {
            snake.ChangeDirection(newDirection.Value);
        }

        snake.Move(appleEaten);

        // Check Collisions
        (int hRow, int hCol) = snake.Head.Position;
        if (GetTile(hRow, hCol) == null)
        {
            return true;
        }

        if (snake.BodyOccupies(hRow, hCol))
        {
            return true;
        }

        // Update board
        SyncSnake(snake);

        return false;
    }
}
