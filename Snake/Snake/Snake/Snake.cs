using System;

namespace Snake
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    class Snake
    {
        static Random RandomnumberGenerator = new Random();
        struct Position
        {
            public int row;
            public int col;
            public Position(int row, int col)
            {
                this.row = row;
                this.col = col;
            }
        }
        static Position[] positions = new Position[]
            {
                new Position(0,1), // Move Right
                new Position(0,-1), // Move Left
                new Position(1,0), // Move Down
                new Position(-1,0) // Move Up
            };
        static int speed = 200;
        static bool GameOver = false;

        static void Main(string[] args)
        {
            Console.BufferHeight = Console.WindowHeight;
            int direction = 0; //0-Right, 1-Left, 2-Down, 3-Up
            Queue<Position> SnakeElements = new Queue<Position>();
            Position food = GetFoodPosition();
            for (int i = 0; i < 5; i++)
            {
                SnakeElements.Enqueue(new Position(0, i));
            }
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var Input = Console.ReadKey();
                    if (Input.Key == ConsoleKey.Escape)
                    {
                        return;
                    }
                    else if (Input.Key == ConsoleKey.RightArrow)
                    {
                        direction = 0;
                    }
                    else if (Input.Key == ConsoleKey.LeftArrow)
                    {
                        direction = 1;
                    }
                    else if (Input.Key == ConsoleKey.DownArrow)
                    {
                        direction = 2;
                    }
                    else if (Input.Key == ConsoleKey.UpArrow)
                    {
                        direction = 3;
                    }
                }
                Position CurrHeadPosition = SnakeElements.Last();
                if (food.row == CurrHeadPosition.row && food.col == CurrHeadPosition.col)
                {
                    SnakeElements = MoveSnake(SnakeElements, direction);
                    if (GameOver)
                    {
                        BreakGeame(SnakeElements.Count);
                        return;
                    }
                    PrintSnake(SnakeElements);
                    food = GetFoodPosition();
                }
                else
                {
                    SnakeElements = MoveSnake(SnakeElements, direction);
                    SnakeElements.Dequeue();
                    if (GameOver)
                    {
                        BreakGeame(SnakeElements.Count);
                        return;
                    }
                    PrintSnake(SnakeElements);
                    PrintFood(food);
                }
                Thread.Sleep(speed);
            }
        }

        private static void BreakGeame(int SnakeElements)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Game Over");
            Console.WriteLine("Your points are {0}", (SnakeElements - 4) * 100);
        }

        static Position GetFoodPosition()
        {
            Position foodPos = new Position(RandomnumberGenerator.Next(0, Console.WindowHeight), RandomnumberGenerator.Next(0, Console.WindowWidth));
            return foodPos;
        }
        static void PrintFood(Position food)
        {
            Console.SetCursorPosition(food.col, food.row);
            Console.Write("@");
            Console.CursorVisible = false;
        }

        static Queue<Position> MoveSnake(Queue<Position> snakeElements, int direction)
        {
            Position snakeHead = snakeElements.Last();
            Position NextDirection = positions[direction];
            Position SnakeHeadNewPosition = new Position(snakeHead.row + NextDirection.row, snakeHead.col + NextDirection.col);
            if (SnakeHeadNewPosition.row < 0 ||
                SnakeHeadNewPosition.col < 0 ||
                SnakeHeadNewPosition.row >= Console.WindowHeight ||
                SnakeHeadNewPosition.col >= Console.BufferWidth)
            {
                GameOver = true;
            }
            else
            {
                snakeElements.Enqueue(SnakeHeadNewPosition);
            }
            return snakeElements;
        }

        static void PrintSnake(Queue<Position> snakeElements)
        {
            Console.Clear();
            foreach (var item in snakeElements)
            {
                Console.SetCursorPosition(item.col, item.row);
                Console.Write("o");
            }
        }
    }
}
