/*
 
LightNode

    - On or Off
    - Position on board
        - X = row, Y = col
            - [
                [1, 2, 3],   => X
                 ^= Y =^
                [4, 5, 6]    => X
              ]
        - Figure out adjacent ones

BoardNode
    - Size (Tuple<int, int>?)
    - LightNodes 
        - List<List<LightNode>> or List<LightNode>

 */

using System;
using System.Xml;

namespace LightsOut.Models;

public class LightNode
{
    public (int row, int col) Position { get; }
    public bool IsOn { get; set; }
    public LightNode (int row, int col)
    {
        Position = (row, col);
        IsOn = false;
    }
}

public class BoardNode
{
    private readonly LightNode[,] __grid;

    public int Rows;
    public int Cols;

    public BoardNode(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;
        __grid = new LightNode[rows, cols];

        Initialize();
    }

    private void Initialize()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                __grid[r, c] = new LightNode(r, c);
            }
        }
    }

    public LightNode Get(int row, int col)
    {
        return __grid[row, col];
    }

    public IEnumerable<LightNode> GetNeighbors(int row, int col)
    {
        var directions = new (int dr, int dc)[]
        {
            (-1, 0),
            (1, 0),
            (0, -1),
            (0, 1)
        };

        foreach (var (dr, dc) in directions)
        {
            int newRow = row + dr;
            int newCol = col + dc;

            if (IsInBounds(newRow, newCol))
                yield return __grid[newRow, newCol];
        }
    }

    private bool IsInBounds(int r, int c)
    {
        return r >= 0 && r < Rows && c >= 0 && c < Cols;
    }

    public void Toggle(int row, int col)
    {
        var center = __grid[row, col];
        center.IsOn = !center.IsOn;

        foreach (var neighbor in GetNeighbors(row, col))
        {
            neighbor.IsOn = !neighbor.IsOn;
        }
    }

    public void Display()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                Console.Write(__grid[r, c].IsOn ? "1 " : "0 ");
            }
            Console.WriteLine();
        }
    }
}