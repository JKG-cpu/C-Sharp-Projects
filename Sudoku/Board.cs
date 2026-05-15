using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku.Board;

public static class BoardGenerator
{
    public static Tile[,] GenerateBoard() => new Tile[9, 9];
    public static void InitBoard(Tile[,] board)
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; r < 9; c++)
            {
                board[r, c] = new Tile(r, c);
            }
        }
    }
}

public static class BoardRenderer
{

}

public class BoardManager(Tile[,] board)
{
    private readonly Tile[,] Board = board;
    private readonly (int RowSize, int ColSize) Size = (board.GetLength(0), board.GetLength(1));

    // Setting Tiles
    public void SetTile(int row, int col, int value) { Board[row, col].GivenValue = value; }
    public void SetTileNote(int row, int col, int value) { Board[row, col].Note = value; }

    // Solving + Generating

}