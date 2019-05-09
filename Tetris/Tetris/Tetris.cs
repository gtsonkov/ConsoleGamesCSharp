using System;

namespace Tetris
{
    using System.Collections.Generic;
    using System.Threading;
    class Tetris
    {
        //Call Ramndom Class
        static Random RandomGenerator = new Random();
        // Settings
        static int HeaderRows = 1;
        static int FooterRows = 1;
        static int TetrisRows = 20;
        static int BorederCols = 3;
        static int TetrisCols = 10;
        static int InfoCols = 10;
        static int ConsoleRows = HeaderRows + TetrisRows + FooterRows;
        static int ConsoleCols = BorederCols + TetrisCols + InfoCols;
        static int Frame = 40; // (1000/24)= 41,6666 = 40ms (25fps HumanEye)
        static int Frames = 0;
        static int FigureIndex = 0; //TODO: Random
        static int FramesToMove = 20; //Frames to move (Frames * MoveSpeed)
        static List<bool[,]> TetrisFigures = new List<bool[,]>(7)
            {
            new bool[,] //I
            { 
                {true,true,true,true}
            },
            new bool[,] //O
            {
                {true,true},
                {true,true}
            },
            new bool[,] //T
            {
                {false,true,false},
                {true,true,true}
            },
            new bool[,] //S
            {
                {false,true,true},
                {true,true,false}
            },
            new bool[,] //Z
            {
                {true,true,false},
                {false,true,true}
            },
            new bool[,] //J
            {
                {true,false,false},
                {true,true,true}
            },
            new bool[,] //L
            {
                {true,true,true},
                {false,false,true}
            },
        };

        //State Info
        static int Score = 0;
        static int CurrentFigureRow = 0;
        static int CurrentFigureCol = 0;
        static bool[,] Field = new bool[TetrisRows,TetrisCols];
        static bool[,] CurrentFigure = TetrisFigures[FigureIndex];
        static void Main(string[] args)
        {
            Console.Title = "Tetris Game";
            Console.WindowHeight = ConsoleRows+1;
            Console.WindowWidth = ConsoleCols;
            Console.BufferHeight = ConsoleRows+1;
            Console.BufferWidth = ConsoleCols;
            Console.CursorVisible = false;
            DrawBorder();
            Drawinfo();
            FigureIndex = GetRandomIndex();
            CurrentFigure = TetrisFigures[FigureIndex];
            while (true)
            {
                Frames++;
                //User Input
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        return;
                    }
                    else if ((key.Key == ConsoleKey.LeftArrow) || (key.Key == ConsoleKey.A))
                    {
                        if (CurrentFigureCol>=1)
                        {
                            CurrentFigureCol--;
                        }
                        
                    }
                    else if ((key.Key == ConsoleKey.RightArrow) || (key.Key == ConsoleKey.D))
                    {
                        if (CurrentFigureCol<TetrisCols-CurrentFigure.GetLength(1))
                        {
                            CurrentFigureCol++;
                        }
                    }
                    else if ((key.Key == ConsoleKey.Spacebar) || (key.Key == ConsoleKey.UpArrow) || (key.Key == ConsoleKey.W))
                    {
                        //TODO: Rotate current figur (90 degree)
                    }
                    else if ((key.Key == ConsoleKey.DownArrow) || (key.Key == ConsoleKey.S))
                    {
                        Frames = 1;
                        CurrentFigureRow++;
                        Score++;
                    }
                }

                //Change Game State
                if (Frames%FramesToMove==0)
                {
                    Frames = 0;
                    //Score++;
                    CurrentFigureRow++;
                }
                DrawBorder();
                Drawinfo();
                DworCurrentFigur();
                Thread.Sleep(Frame);
            }
        }

        static int GetRandomIndex()
        {
            int RandomIndex = RandomGenerator.Next(-1, 7);
            return RandomIndex;
        }

        //Write whit exact parameters
        static void Write(string text, int row, int col, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(col, row);
            Console.Write(text);
            Console.ResetColor();
        }
        static void DrawBorder()
        {
            Console.SetCursorPosition(0, 0);
            string Firstline ="╔";
            Firstline += new string('═', TetrisCols);
            Firstline += "╦";
            Firstline += new string('═', InfoCols);
            Firstline += "╗";
            Console.WriteLine(Firstline);
            string MiddleLines = "║";
            MiddleLines += new string(' ', TetrisCols);
            MiddleLines += "║";
            MiddleLines += new string(' ', InfoCols);
            MiddleLines += "║";
            for (int i = 0; i < ConsoleRows -2; i++)
            {
                Console.WriteLine(MiddleLines);
            }
            string Endline = "╚";
            Endline += new string('═', TetrisCols);
            Endline += "╩";
            Endline += new string('═', InfoCols);
            Endline += "╝";
            Console.WriteLine(Endline);
        }
        static void Drawinfo()
        {
            Write("Score:", 1, 3 + TetrisCols);
            Write(Score.ToString(), 2, 3+TetrisCols);
            Write("Frame:", 4, 3 + TetrisCols);
            Write(Frames.ToString(), 5, 3 + TetrisCols);
            Write("Position:",7, 3 + TetrisCols);
            Write(CurrentFigureRow + "," + CurrentFigureCol, 8, 3 + TetrisCols);
        }
        static void DworCurrentFigur()
        {
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    if (CurrentFigure[row,col])
                    {
                        Write("*", row + 1 + CurrentFigureRow, col + 1 + CurrentFigureCol);
                    }
                }
            }
        }

    }
}
