using System;

namespace Tetris
{
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
        static int SleepTime = 40; // (1000/24)= 41,6666 = 40ms (25fps HumanEye)

        //State Info
        static int Score = 0;
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
                }

                //Change State
                Score++;
                Drawnfo();
                Thread.Sleep(SleepTime);
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
        }
    }
}
