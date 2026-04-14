using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Snake.Models;

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

    // Snake Creating / Updating
    public void CreateSnake()
    {

    }
}
