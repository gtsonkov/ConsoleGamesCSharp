using System;

namespace Snake
{
    using System.Collections.Generic;
    using System.Linq;

    class Snake
    {
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
        static void Main(string[] args)
        {
            int direction = 0; //0-Right, 1-Left, 2-Down, 3-Up
            Queue<Position> SnakeElements = new Queue<Position>();
            for (int i = 0; i < 5; i++)
            {
                SnakeElements.Enqueue(new Position(0, i));
            }

            while (true)
            {
                var Input = Console.ReadKey();
                if (Input.Key == ConsoleKey.Escape)
                {
                    return;
                }
                else if (Input.Key == ConsoleKey.RightArrow )
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
                SnakeElements = MoveSnake(SnakeElements, direction);
                PrintSnake(SnakeElements);
            }
        }
        static Queue<Position> MoveSnake(Queue<Position> snakeElements, int direction)
        {
            snakeElements.Dequeue();
            Position snakeHead = snakeElements.Last();
            Position NextDirection = positions[direction];
            Position SnakeHeadNewPosition = new Position(snakeHead.row + NextDirection.row, snakeHead.col + NextDirection.col);
            snakeElements.Enqueue(SnakeHeadNewPosition);
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
