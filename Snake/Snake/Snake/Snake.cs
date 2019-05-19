using System;

namespace Snake
{
    using System.Collections.Generic;

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
        static void Main(string[] args)
        {
            Position[] positions = new Position[]
            {
                new Position(0,1), // Move Right
                new Position(0,-1), // Move Left
                new Position(1,0), // Move Down
                new Position(-1,0) // Move Up
            };
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

                foreach (Position pos in SnakeElements)
                {
                    Console.SetCursorPosition(pos.col, pos.row);
                    Console.Write("*");
                }
            }
        }
    }
}
