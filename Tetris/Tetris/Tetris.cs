using System;

namespace Tetris
{
    using System.Collections.Generic;
    using System.Threading;
    class Tetris
    {
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
        static int FramesToMove = 20; //Frames to move (Frames * MoveSpeed)
        static int MoveSpeed = FramesToMove * Frame;
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
        static void Main(string[] args)
        {
            Console.Title = "Tetris Game";
            Console.WindowHeight = ConsoleRows+1;
            Console.WindowWidth = ConsoleCols;
            Console.BufferHeight = ConsoleRows+1;
            Console.BufferWidth = ConsoleCols;
            Console.CursorVisible = false;
            DrawBorder();
            Drawnfo();
            while (true)
            {
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
                        //TODO: Move current figur left
                        CurrentFigureCol--; // TODO: Check out of range
                    }
                    else if ((key.Key == ConsoleKey.RightArrow) || (key.Key == ConsoleKey.D))
                    {
                        //TODO: Move current figur right
                        CurrentFigureCol++; // TODO: Check out of range
                    }
                    else if ((key.Key == ConsoleKey.Spacebar) || (key.Key == ConsoleKey.UpArrow) || (key.Key == ConsoleKey.W))
                    {
                        //TODO: Rotate current figur (90 degree)
                    }
                    else if ((key.Key == ConsoleKey.DownArrow) || (key.Key == ConsoleKey.S))
                    {
                        //TODO: Move current figur down and Score++ 
                        Frame = 1;
                    }
                }

                //Change Game State
                if (Frames%FramesToMove==0)
                {
                    //Move current figure
                    Frames = 0;
                    Score++;
                }
                Drawnfo();
                Frames++;
                Thread.Sleep(Frame);
            }
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
        static void Drawnfo()
        {
            Write("Score:", 1, 3 + TetrisCols);
            Write(Score.ToString(), 2, 3+TetrisCols);
            Write("Frame:", 4, 3 + TetrisCols);
            Write(Frames.ToString(), 5, 3 + TetrisCols);
        }
    }
}
