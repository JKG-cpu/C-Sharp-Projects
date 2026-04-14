using LightsOut.Models;
using System.Data;

public class Program
{
    public static void Main()
    {
        var board = new BoardNode(3, 3);

        board.Toggle(1, 1);
        board.Display();
    }
}