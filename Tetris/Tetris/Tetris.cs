using System;

namespace Tetris
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
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
        static int FigureIndex = 0;
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
        static int HighScore = 0;
        static int CurrentFigureRow = 0;
        static int CurrentFigureCol = 0;
        static bool[,] Field = new bool[TetrisRows, TetrisCols];
        static bool[,] CurrentFigure = TetrisFigures[FigureIndex];
        static void Main(string[] args)
        {
            
            if (File.Exists("HighscoresTetris.txt"))
            {
                var allScores = File.ReadAllLines("HighscoresTetris.txt");
                foreach (var item in allScores)
                {
                   var ScoreMatch = Regex.Match(item, @"=> (?<score>[0-9]+)");
                   HighScore = Math.Max(HighScore, int.Parse(ScoreMatch.Groups["score"].Value));
                }
            }
            Console.Title = "Tetris Game";
            Console.WindowHeight = ConsoleRows + 1;
            Console.WindowWidth = ConsoleCols;
            Console.BufferHeight = ConsoleRows + 1;
            Console.BufferWidth = ConsoleCols;
            Console.CursorVisible = false;
            DrawBorder();
            DrawInfo();
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
                        if (CurrentFigureCol >= 1)
                        {
                            if (!CollisionLeft())
                            {
                                CurrentFigureCol--;
                            }
                        }
                    }
                    else if ((key.Key == ConsoleKey.RightArrow) || (key.Key == ConsoleKey.D))
                    {
                        if (CurrentFigureCol < TetrisCols - CurrentFigure.GetLength(1))
                        {
                            if (!CollisionRight())
                            {
                                CurrentFigureCol++;
                            }
                        }
                    }
                    else if ((key.Key == ConsoleKey.Spacebar) || (key.Key == ConsoleKey.UpArrow) || (key.Key == ConsoleKey.W))
                    {
                        //TODO: Rotate current figur (90 degree)
                    }
                    else if (((key.Key == ConsoleKey.DownArrow) || (key.Key == ConsoleKey.S)) && (!(Collision())))
                    {
                            Frames = 1;
                            CurrentFigureRow++;
                            Score++;
                    }
                }

                //Change Game State
                if (Frames % FramesToMove == 0)
                {
                    Frames = 0;
                    if (!(Collision()))
                    {
                        CurrentFigureRow++;
                    }
                    else
                    {
                        AddCurrentFigureToField();
                        FigureIndex = GetRandomIndex();
                        CurrentFigure = TetrisFigures[FigureIndex];
                        CurrentFigureRow = 0;
                        CurrentFigureCol = 0;
                        if (Collision())
                        {
                            File.AppendAllLines("HighscoresTetris.txt", new List<string>
                            {
                                $"[{DateTime.Now.ToLongTimeString()}] - {Environment.UserName} => {Score}"
                            });
                            var StringScore = Score.ToString();
                            StringScore += new string(' ', 7 - StringScore.Length);
                            Write("╔═════════╗", 5, 5);
                            Write("║ Game    ║", 6, 5);
                            Write("║   Over! ║", 7, 5);
                            Write($"║ {StringScore} ║", 8, 5);
                            Write("╚═════════╝", 9, 5);
                            break;
                        }
                    }
                }
                DrawBorder();
                DrawInfo();
                DrawField();
                DrawCurrentFigure();
                Thread.Sleep(Frame);
            }
            Write("Press Esc to exit", 18, 3,ConsoleColor.Red);
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    return;
                }
            }
        }

        static void AddCurrentFigureToField()
        {
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    if (true)
                    {
                        if (CurrentFigure[row, col])
                        {
                            Field[CurrentFigureRow + row, CurrentFigureCol + col] = true;
                        }
                    }
                }
            }
        }

        //Get Random Index
        static int GetRandomIndex()
        {
            int RandomIndex = RandomGenerator.Next(0, 6);
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
            string Firstline = "╔";
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
            for (int i = 0; i < ConsoleRows - 2; i++)
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
        static void DrawInfo()
        {
            Write("Score:", 1, 3 + TetrisCols);
            Write(Score.ToString(), 2, 3 + TetrisCols);
            Write("Frame:", 4, 3 + TetrisCols);
            Write(Frames.ToString(), 5, 3 + TetrisCols);
            Write("Position:", 7, 3 + TetrisCols);
            Write(CurrentFigureRow + "," + CurrentFigureCol, 8, 3 + TetrisCols);
            Write("High score", 9, 3 + TetrisCols);
            Write($"{HighScore}", 10, 3 + TetrisCols);
        }
        static void DrawCurrentFigure()
        {
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    if (CurrentFigure[row, col])
                    {
                        Write("*", row + 1 + CurrentFigureRow, col + 1 + CurrentFigureCol);
                    }
                }
            }
        }
        static void DrawField()
        {
            for (int row = 0; row < Field.GetLength(0); row++)
            {
                for (int col = 0; col < Field.GetLength(1); col++)
                {
                    if (Field[row, col])
                    {
                        Write("*", row + 1, col + 1);
                    }
                }
            }
        }
        static bool Collision()
        {
            //TODO: Colide with existing figures
            if (CurrentFigureRow + CurrentFigure.GetLength(0) == TetrisRows)
            {
                return true;
            }
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLongLength(1); col++)
                {
                    if (CurrentFigure[row, col] && Field[CurrentFigureRow + row + 1, CurrentFigureCol + col])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        static bool CollisionLeft()
        {
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLongLength(1); col++)
                {
                    if (CurrentFigure[row, col] && Field[CurrentFigureRow + row, CurrentFigureCol + col - 1])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        static bool CollisionRight()
        {
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLongLength(1); col++)
                {
                    if (CurrentFigure[row, col] && Field[CurrentFigureRow + row, CurrentFigureCol + col + 1])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
