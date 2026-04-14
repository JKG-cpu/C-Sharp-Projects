using Snake.Board;
using Snake.Models;
using Snake.Snake;

namespace Snake;

public static class UIHelper
{
    public static void DisplayBoardSizes(Dictionary<string, (int, int)> boardSizes)
    {
        Console.WriteLine("----- Board Sizes -----");
        Console.WriteLine();
        foreach (var (key, (x, y)) in boardSizes)
        {
            Console.WriteLine($"{key} => {x} by {y}");
        }
        Console.WriteLine();
        string bottomString = new('-', "----- Board Sizes -----".Length);
        Console.WriteLine(bottomString);
        Console.WriteLine();
    }

    public static void InvalidInput(string prompt = "Invalid Input.", string con = "Press enter to continue.", bool inputContinue = true)
    {
        Console.WriteLine(prompt);
        if (inputContinue)
        {
            Console.WriteLine(con);
            Console.ReadKey(true);
        }
    }
}

public static class Program
{
    private static readonly Dictionary<string, (int, int)> BoardSizes = new()
    {
        { "Small", (10, 10) },
        { "Medium", (20, 20) },
        { "Large", (25, 35) }
    };

    public static void Main()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            UIHelper.DisplayBoardSizes(BoardSizes);

            Console.WriteLine("Movement type: WASD\n");

            Console.Write("Type in a gamemode (S, M, or L) or type E to exit > ");

            string? userInput = Console.ReadLine();

            if (string.IsNullOrEmpty(userInput))
            {
                UIHelper.InvalidInput(prompt: "Please type in something.");
                continue;
            }

            switch (userInput.ToLower()[0])
            {
                case 's':
                    RunGame(BoardSizes["Small"]);
                    break;

                case 'm':
                    RunGame(BoardSizes["Medium"]);
                    break;

                case 'l':
                    RunGame(BoardSizes["Large"]);
                    break;

                case 'e':
                    running = false;
                    break;

                default:
                    UIHelper.InvalidInput(prompt: "That is not a valid option.");
                    continue;
            }
        }
    }

    public static void RunGame((int, int) size)
    {
        (Tile[,] board, BoardManager boardManager, SnakeObject snake) = Setup(size);

        bool running = true;
        Direction dir = Direction.Right;

        Console.WriteLine("Getting Ready...");
        Console.Write("Press any key to start!");
        Console.ReadKey(true);

        while (running)
        {
            Console.SetCursorPosition(0, 0);

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                
                switch (key.Key)
                {
                    case ConsoleKey.W:
                        dir = Direction.Up;
                        break;

                    case ConsoleKey.S:
                        dir = Direction.Down; 
                        break;

                    case ConsoleKey.A:
                        dir = Direction.Left;
                        break;

                    case ConsoleKey.D:
                        dir = Direction.Right;
                        break;

                    case ConsoleKey.Escape:
                        running = false;
                        continue;

                    default:
                        continue;
                }
            }

            int gameOver = boardManager.UpdateSnake(snake, dir);
            BoardRendering.DisplayBoard(board);

            if (gameOver == -1)
            {
                running = false;
                Console.Clear();
                Console.WriteLine("You Lost!");
                continue;
            }
            else if (gameOver == 1)
            {
                running = false;
                Console.Clear();
                Console.WriteLine("You Won!");
                continue;
            }

            Thread.Sleep(150);
        }
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey(true);
    }

    public static (Tile[,] board, BoardManager boardManager, SnakeObject snake) Setup((int x, int y) size)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();

        Tile[,] board = BoardGenerator.GenerateBoard(size);
        BoardManager boardManager = new(board);
        SnakeObject snake = boardManager.CreateSnake();

        boardManager.SpawnApple(snake);

        return (board, boardManager, snake);
    }
}
