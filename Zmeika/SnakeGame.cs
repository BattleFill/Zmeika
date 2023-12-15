using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zmeika
{
    internal class SnakeGame
    {
        private List<(int, int)> snake;
        private (int, int) direction;
        private Random random;
        private (int, int) apple;
        private bool gameLive;
        private int boardWidth;
        private int boardHeight;

        public SnakeGame()
        {
            snake = new List<(int, int)> { (Convert.ToInt32(Border.MaxRight) / 2, Convert.ToInt32(Border.MaxBottom) / 2) };
            direction = (0, 0);
            random = new Random();
            apple = (random.Next(0, Convert.ToInt32(Border.MaxRight) - 1), random.Next(0, Convert.ToInt32(Border.MaxBottom) - 1));
            gameLive = true;
            boardWidth = (int)Border.MaxRight;
            boardHeight = (int)Border.MaxBottom;
        }

        public void Start()
        {
            Console.SetWindowSize(Math.Min((int)Border.MaxRight, Console.WindowWidth), Math.Min((int)Border.MaxBottom, Console.WindowHeight));
            Console.CursorVisible = false;

            ConsoleKeyInfo start = Console.ReadKey(true);

            if (start.Key == ConsoleKey.UpArrow)
            {
                direction = (0, -1);
            }
            if (start.Key == ConsoleKey.DownArrow)
            {
                direction = (0, 1);
            }
            if (start.Key == ConsoleKey.RightArrow)
            {
                direction = (1, 0);
            }
            if (start.Key == ConsoleKey.LeftArrow)
            {
                direction = (-1, 0);
            }

            while (gameLive)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    direction = key.Key switch
                    {
                        ConsoleKey.UpArrow when direction.Item2 == 0 => (0, -1),
                        ConsoleKey.DownArrow when direction.Item2 == 0 => (0, 1),
                        ConsoleKey.LeftArrow when direction.Item1 == 0 => (-1, 0),
                        ConsoleKey.RightArrow when direction.Item1 == 0 => (1, 0),
                        _ => direction
                    };
                }

                DrawSnake();
                MoveSnake();

                
                if (snake[^1].Item1 < 0 || snake[^1].Item1 >= boardWidth ||
                    snake[^1].Item2 < 0 || snake[^1].Item2 >= boardHeight)
                {
                    gameLive = false;
                    
                    break;
                }

                
                for (int i = 0; i < snake.Count - 1; i++)
                {
                    if (snake[i] == snake[^1])
                    {
                        gameLive = false;
                        break;
                    }
                }

                DrawApple();

                if (snake[^1] == apple)
                {
                    snake.Add(apple);
                    do
                    {
                        apple = (random.Next(0, boardWidth), random.Next(0, boardHeight));
                    } while (snake.Contains(apple));
                }

                Thread.Sleep(50);
            }

            Console.Clear();
        }

        private void DrawSnake()
        {
            foreach (var segment in snake)
            {
                WriteAt('+', segment.Item1, segment.Item2);
            }
        }

        private void MoveSnake()
        {
            WriteAt(' ', snake[0].Item1, snake[0].Item2);
            for (int i = 0; i < snake.Count - 1; i++)
            {
                snake[i] = snake[i + 1];
            }
            snake[^1] = (snake[^1].Item1 + direction.Item1, snake[^1].Item2 + direction.Item2);
            WriteAt('#', snake[^1].Item1, snake[^1].Item2);
        }

        private void DrawApple()
        {
            WriteAt('X', apple.Item1, apple.Item2);
        }

        private void WriteAt(char ch, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(ch);
        }
    }
}

