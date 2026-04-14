using Snake.Board;
using Snake.Models;
using Snake.Snake;

namespace Snake;

public static class Program
{
    public static void Main()
    {
        (Tile[,] board, BoardManager boardManager, SnakeObject snake) = Setup((25, 25));

        bool running = true;
        Direction dir = Direction.Right;

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

/*

1. Build board
    - Slots
   
2. Start with UI
    - Display Board
    - Display Movement
    - Frames + Clearing

3. Display Snake
    - Drawing / Adding snake
    - Moving

4. Input Detection
    - Console.AvailabeKey

5. Border Detection + Collision
    - Lose

6. Apples
    - Spawing + eating

7. Win Condition
    - If no empty spaces

8. Entry Point
    - Settings

 */