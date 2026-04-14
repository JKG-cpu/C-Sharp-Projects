using MineSweeper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace MineSweeper.Board;

public class BoardGenerator
{
    readonly private Random rand = new();
    readonly private List<(int dr, int dc)> directions =
    [
        ( -1, -1 ), ( -1, 0 ), ( -1, 1 ),
        ( 0, -1 ), ( 0, 1 ),
        ( 1, -1 ), ( 1, 0 ), ( 1, 1 )
    ];

    private List<(int r, int c)> GenerateRandomPositions((int rows, int cols) size)
    {
        List<(int r, int c)> positions = [];

        for (int r = 0; r < size.rows; r++)
        {
            for (int c = 0; c < size.cols; c++)
            {
                positions.Add((r, c));
            }
        }

        for (int i = positions.Count - 1; i > 0 ; i--)
        {
            int j = rand.Next(i + 1);
            (positions[i], positions[j]) = (positions[j], positions[i]);
        }

        return positions;
    }

    private Tile[,] SetTileNumbers(Tile[,] board)
    {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Tile tile = board[r, c];

                if (tile.Value == 9) continue;

                int mineCount = 0;
                foreach ((int dr, int dc) in directions)
                {
                    int nr = r + dr;
                    int nc = c + dc;

                    if (nr >= 0 && nr < rows && nc >= 0 && nc < cols)
                    {
                        if (board[nr, nc].Value == 9)
                        {
                            mineCount++;
                        }
                    }
                }

                tile.Value = mineCount;
            }
        }

        return board;
    }

    public Tile[,] GenerateBoard((int rows, int cols) size, int mineAmount)
    {
        if (mineAmount > size.rows * size.cols)
        {
            throw new ArgumentException("Too many mines for the board size.");
        }

        Tile[,] board = new Tile[size.rows, size.cols];

        for (int r = 0; r < size.rows; r++)
        {
            for (int c = 0; c < size.cols; c++)
            {
                board[r, c] = new Tile();
            }
        }

        List<(int r, int c)> positions = GenerateRandomPositions(size);

        for (int i = 0; i < mineAmount; i++)
        {
            var (r, c) = positions[i];
            board[r, c].Value = 9;
        }

        board = SetTileNumbers(board);

        return board;
    }
}

public class BoardDisplay(Tile[,] bd, int rowSize, int colSize)
{
    readonly private Tile[,] board = bd;
    readonly private int rowSize = rowSize;
    readonly private int colSize = colSize;

    private string GetVertLines() =>
        " X    " + string.Concat(
            Enumerable.Range(1, colSize)
                .Select(n => n < 10 ? $"{n}   " : $"{n}  ")
        ) + "  ";

    public void DisplayBoard(int flagsLeft)
    {
        Console.WriteLine($"Flags Left: {flagsLeft}");

        string vertLines = GetVertLines();
        Console.WriteLine(vertLines);

        string border = "  +" + new string('-', colSize * 4 + 1) + "+";
        Console.WriteLine("Y" + border);

        for (int r = 0; r < rowSize; r++)
        {
            Console.WriteLine($"{r + 1,-2}  | " + string.Join(" | ",
                Enumerable.Range(0, colSize).Select(c => board[r, c].IsRevealed ? 
                (board[r, c].Value.ToString() == "9" ? "*" : board[r, c].Value.ToString()) : board[r, c].IsFlagged ? "F" : " ")
            ) + " | ");
        }

        Console.WriteLine(" " + border);
    }
}

public class BoardManager(Tile[,] bd, int flagsLeft)
{
    readonly private Tile[,] board = bd;
    readonly private int rowSize = bd.GetLength(0);
    readonly private int colSize = bd.GetLength(1);
    private int flagsLeft = flagsLeft;
    readonly private List<(int dr, int dc)> directions =
    [
        ( -1, -1 ), ( -1, 0 ), ( -1, 1 ),
        ( 0, -1 ), ( 0, 1 ),
        ( 1, -1 ), ( 1, 0 ), ( 1, 1 )
    ];
    readonly private BoardDisplay boardDisplay = new(bd, bd.GetLength(0), bd.GetLength(1));

    private bool IsOutOfBounds(int r, int c)
    {
        if (!(0 <= r && r < rowSize && 0 <= c && c < colSize))
        {
            return true;
        }

        return false;
    }

    private bool IsValid(int r, int c)
    {
        if (IsOutOfBounds(r, c))
        {
            return false;
        }

        if (board[r, c].IsRevealed)
        {
            return false;
        }

        return true;
    }

    private bool CheckWin()
    {
        for (int r = 0; r < rowSize; r++)
        {
            for (int c = 0; c < colSize; c++)
            {
                Tile tile = board[r, c];

                if (tile.Value != 9 && !tile.IsRevealed)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void FloodFill(int r, int c)
    {
        if (IsOutOfBounds(r, c)) return;

        if (board[r, c].IsRevealed) return;

        if (board[r, c].Value != 0)
        {
            if (board[r, c].Value != 9)
            {
                board[r, c].IsRevealed = true;
            }
            return;
        }

        board[r, c].IsRevealed = true;

        foreach ((int dr, int dc) in directions)
        {
            FloodFill(r + dr, c + dc);
        }
    }

    public int MakeMove((int row, int col) slot, int mode)
    {
        int gameState = 0;

        if (!IsValid(slot.row, slot.col)) { return 2; }

        if (mode == 2)
        {
            switch (board[slot.row, slot.col].IsFlagged)
            {
                case true:
                    board[slot.row, slot.col].IsFlagged = false;
                    flagsLeft++;
                    break;

                case false:
                    if (flagsLeft > 0)
                    {
                        board[slot.row, slot.col].IsFlagged = true;
                        flagsLeft--;
                    }
                    break;
            }

            return gameState;
        }

        if (board[slot.row, slot.col].IsFlagged) { return gameState; }

        if (board[slot.row, slot.col].Value == 0) { FloodFill(slot.row, slot.col); }
        else { board[slot.row, slot.col].IsRevealed = true; }

        if (board[slot.row, slot.col].Value == 9) { gameState = -1; }

        if (CheckWin()) { gameState = 1; }

        return gameState;
    }

    public void Display()
    {
        boardDisplay.DisplayBoard(flagsLeft);
    }
}