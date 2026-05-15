using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku.Models;

public class Tile(int row, int col)
{
    public int Row { get; } = row;
    public int Col { get; } = col;
    public TileStatus Status { get; set; } = TileStatus.None;

    private int _Value = 0;
    private int _GivenValue;
    private int _Note;

    public int GivenValue
    {
        get => _GivenValue;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 9);
            _GivenValue = value;
            if (_GivenValue == 0) { Status = TileStatus.None; }
            else 
            { 
                Status = _Value == _GivenValue 
                    ? TileStatus.Correct : TileStatus.Incorrect; 
            }
        }
    }

    public int Value
    {
        get => _Value;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 1);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 9);
            _Value = value;
        }
    }

    public int Note
    {
        get => _Note;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 9);
            _Note = value;
        }
    }
}

public enum TileStatus
{
    None,
    Correct,
    Incorrect
}