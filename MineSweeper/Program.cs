using MineSweeper.Board;
using MineSweeper.Models;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace MineSweeper;

public static class UIHelper
{
    public static void InvalidInput(string message = "Invalid input.", string action = "Press Enter to continue.")
    {
        Console.WriteLine(message);
        Console.Write(action);
        Console.ReadKey(true);
        Console.Clear();
    }

    public static void DisplayGamemodes(List<(string Name, (int, int) Size, int Mines)> gameSettings)
    {
        for (int i = 0; i < gameSettings.Count; i++)
        {
            var (Name, Size, Mines) = gameSettings[i];

            Console.WriteLine(
                $"{i + 1}. {Name} - Grid Size: {Size.Item1}x{Size.Item2}. Mines: {Mines}"
            );
        }
    }
}

public static class Program
{
    private readonly static BoardGenerator boardGenerator = new();
    private readonly static List<(string Name, (int, int) Size, int Mines)> gameSettings =
    [
        ("Easy", (9, 9), 10),
        ("Medium", (16, 16), 40),
        ("Hard", (30, 16), 99)
    ];

    private static BoardManager Setup((int, int) sizeSettings, int mineAmount)
    {
        Tile[,] board = boardGenerator.GenerateBoard(sizeSettings, mineAmount);
        BoardManager boardManager = new(board, mineAmount);
        return boardManager;
    }

    private static (string? errorMessage, (int xcoord, int ycoord) coords) GetCoords(string userInput)
    {
        (string? errorMessage, (int xcoord, int ycoord) coords) result = (null, (0, 0));

        if (!userInput.Contains(','))
        {
            result.errorMessage = "You must seperate the coords with a comma.";
            return result;
        }

        if (userInput.AsSpan().Count(',') > 1)
        {
            result.errorMessage = "You can only input two commas. Like x,y.";
            return result;
        }

        string[] coords = userInput.Split(',');

        if (int.TryParse(coords[0], out int xcoord) && int.TryParse(coords[1], out int ycoord))
        {
            result.coords = (xcoord - 1, ycoord - 1);
            return result;
        }
        else
        {
            result.errorMessage = "You must enter in two integers seperated by a comma.";
            return result;
        }
    }

    private static void MainLoop((int, int) sizeSettings, int mineAmount)
    {
        bool running = true;
        int mode = 1;

        BoardManager boardManager = Setup(sizeSettings, mineAmount);
        
        while (running)
        {
            Console.Clear();

            boardManager.Display();

            string modeText = mode == 1 ? "Normal" : "Flags";
            Console.WriteLine($"Current Mode: {modeText}");

            Console.Write("Enter in a move (y, x) or type E to exit, or type S to switch modes > ");

            string? userInput = Console.ReadLine();

            if (string.IsNullOrEmpty(userInput))
            {
                UIHelper.InvalidInput("You must type something in.");
                continue;
            }

            userInput = string.Concat(userInput.Where(c => !char.IsWhiteSpace(c)));

            if (userInput.ToUpper()[0] == 'E')
            {
                running = false;
                continue;
            }

            if (userInput.ToUpper()[0] == 'S')
            {
                mode = mode == 1 ? 2 : 1;
                continue;
            }

            (string? errorMessage, (int xcoord, int ycoord) coords) = GetCoords(userInput);

            if (errorMessage != null)
            {
                UIHelper.InvalidInput(errorMessage);
                continue;
            } else
            {
                int result = boardManager.MakeMove(coords, mode);
            
                switch (result)
                {
                    case 2:
                        UIHelper.InvalidInput("That it not a valid move.");
                        break;

                    case -1:
                        Console.Clear();
                        boardManager.Display();
                        UIHelper.InvalidInput("You lost!");
                        running = false;
                        break;

                    case 1:
                        Console.Clear();
                        boardManager.Display();
                        UIHelper.InvalidInput("You won!");
                        running = false;
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public static void Main()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            UIHelper.DisplayGamemodes(gameSettings);

            Console.Write("Select a game mode (number) or type E to exit > ");

            string? userInput = Console.ReadLine();

            if (string.IsNullOrEmpty(userInput))
            {
                UIHelper.InvalidInput("You must type something in.");
                continue;
            }

            userInput = string.Concat(userInput.Where(c => !char.IsWhiteSpace(c)));

            if (userInput.ToUpper()[0] == 'E')
            {
                running = false;
                continue;
            }

            if (int.TryParse(userInput, out int setting))
            {
                int index = setting - 1;

                if (index < 0 || index >= gameSettings.Count)
                {
                    UIHelper.InvalidInput("That is not a valid option...");
                    continue;
                }

                var (Name, Size, Mines) = gameSettings[index];

                MainLoop(Size, Mines);
            }
        }
    }
}