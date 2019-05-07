using System;

namespace Tetris
{
    class Tetris
    {
        static int HeaderRows = 1;
        static int FooterRows = 1;
        static int TetrisRows = 20;
        static int BorederCols = 3;
        static int TetrisCols = 10;
        static int InfoCols = 10;
        static int ConsoleRows = HeaderRows + TetrisRows + FooterRows;
        static int ConsoleCols = BorederCols + TetrisCols + InfoCols;
        static void Main(string[] args)
        {
            Console.Title = "Tetris Game";
            Console.WindowHeight = ConsoleRows+1;
            Console.WindowWidth = ConsoleCols;
            Console.BufferHeight = ConsoleRows+1;
            Console.BufferWidth = ConsoleCols;
            DrawBorder();
            Console.ReadKey();
        }

        static void DrawBorder()
        {
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
    }
}
