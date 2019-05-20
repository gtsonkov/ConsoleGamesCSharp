using System;

namespace Tetris
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
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
        static int Level = 1;
        static int ConsoleRows = HeaderRows + TetrisRows + FooterRows;
        static int ConsoleCols = BorederCols + TetrisCols + InfoCols;
        static int Frame = 40; // (1000/24)= 41,6666 = 40ms (25fps HumanEye)
        static int Frames = 0;
        static int FigureIndex = 0;
        static int FramesToMove = 16; //Frames to move (Frames * MoveSpeed)
        static int[] ScorePerLines = { 0, 40, 100, 300, 1200 };
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
        static int CurrentRow = 0;
        static int CurrentCol = 0;
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
                //Update Level
                UpdateLevel();
                //User Input
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        GameOver();
                        return;
                    }
                    else if (((key.Key == ConsoleKey.LeftArrow) || (key.Key == ConsoleKey.A)) && !(Collision(CurrentFigure)))
                    {
                        if (CurrentCol >= 1)
                        {
                            if (!CollisionLeft(CurrentFigure))
                            {
                                CurrentCol--;
                            }
                        }
                    }
                    else if (((key.Key == ConsoleKey.RightArrow) || (key.Key == ConsoleKey.D)) && !(Collision(CurrentFigure)))
                    {
                        if (CurrentCol < TetrisCols - CurrentFigure.GetLength(1))
                        {
                            if (!CollisionRight(CurrentFigure))
                            {
                                CurrentCol++;
                            }
                        }
                    }
                    else if ((key.Key == ConsoleKey.Spacebar) || (key.Key == ConsoleKey.UpArrow) || (key.Key == ConsoleKey.W))
                    {
                        RotateCurrentFigure();
                    }
                    else if (((key.Key == ConsoleKey.DownArrow) || (key.Key == ConsoleKey.S)) && (!Collision(CurrentFigure)))
                    {
                        Frames = 1;
                        CurrentRow++;
                        Score++;
                    }
                }

                //Change Game State
                if (Frames % (FramesToMove - Level) == 0)
                {
                    Frames = 0;
                    if (!(Collision(CurrentFigure)))
                    {
                        CurrentRow++;
                    }
                    else
                    {
                        AddCurrentFigureToField();
                        int lines = ChekForFullLines();
                        Score += ((ScorePerLines[lines])*Level);
                        FigureIndex = GetRandomIndex();
                        CurrentFigure = TetrisFigures[FigureIndex];
                        CurrentRow = 0;
                        CurrentCol = 0;
                        if (Collision(CurrentFigure))
                        {
                            GameOver();
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
            Write("Press Esc to exit", 18, 3);
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    return;
                }
            }
        }

        private static void GameOver()
        {
            DrawBorder();
            DrawInfo();
            DrawField();
            DrawCurrentFigure();
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
        }

        private static void UpdateLevel()
        {
            if (Score <= 9)
            {
                Level = 1;
                return;
            }

            Level = (int)(Math.Log10(Score));
        }

        static void RotateCurrentFigure()
        {
            var NewFigure = new bool[CurrentFigure.GetLength(1), CurrentFigure.GetLength(0)];
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    NewFigure[col, CurrentFigure.GetLength(0) - row - 1] = CurrentFigure[row, col];
                }
            }
            if (!Collision(NewFigure))
            {
                CurrentFigure = NewFigure;
            }
        }
        static int ChekForFullLines()
        {
            int lines = 0;
            for (int row = 0; row < Field.GetLength(0); row++)
            {
                bool rowIsFull = true;
                for (int col = 0; col < Field.GetLongLength(1); col++)
                {
                    if (Field[row, col] == false)
                    {
                        rowIsFull = false;
                        break;
                    }
                }
                if (rowIsFull)
                {
                    for (int rowToMove = row; rowToMove >= 1; rowToMove--)
                    {
                        for (int col = 0; col < Field.GetLength(1); col++)
                        {
                            Field[rowToMove, col] = Field[rowToMove - 1, col];
                        }
                    }
                    lines++;
                }
            }
            return lines;
        }
        static void AddCurrentFigureToField()
        {
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    if (CurrentFigure[row, col])
                    {
                        Field[CurrentRow + row, CurrentCol + col] = true;
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
        static void Write(string text, int row, int col)
        {
            Console.SetCursorPosition(col, row);
            Console.Write(text);
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
            Write("Level:", 3, 3 + TetrisCols);
            Write(Level.ToString(), 4, 3 + TetrisCols);
            if (Score > HighScore)
            {
                HighScore = Score;
            }
            Write("Best", 5, 3 + TetrisCols);
            Write($"{HighScore}", 6, 3 + TetrisCols);
        }
        static void DrawCurrentFigure()
        {
            for (int row = 0; row < CurrentFigure.GetLength(0); row++)
            {
                StringBuilder line = new StringBuilder();
                for (int col = 0; col < CurrentFigure.GetLength(1); col++)
                {
                    if (CurrentFigure[row, col])
                    {
                        Write("*", row + 1 + CurrentRow, col + 1 + CurrentCol);
                    }
                }
            }
        }
        static void DrawField()
        {
            for (int row = 0; row < Field.GetLength(0); row++)
            {
                StringBuilder line = new StringBuilder();
                for (int col = 0; col < Field.GetLength(1); col++)
                {
                    if (Field[row, col])
                    {
                        line.Append("*");
                    }
                    else
                    {
                        line.Append(" ");
                    }
                }
                Write(line.ToString(), row + 1, 1);
            }
        }
        static bool Collision(bool[,] figure)
        {
            var FigureLowestRow = CurrentRow + figure.GetLength(0);
            if (FigureLowestRow > TetrisRows)
            {
                return true;
            }

            if (CurrentCol > TetrisCols - figure.GetLength(1))
            {
                return true;
            }
            if (CurrentRow + figure.GetLength(0) == TetrisRows)
            {
                return true;
            }
            for (int row = 0; row < figure.GetLength(0); row++)
            {
                for (int col = 0; col < figure.GetLongLength(1); col++)
                {
                    if (figure[row, col] && Field[CurrentRow + row + 1, CurrentCol + col])
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        static bool CollisionLeft(bool[,] figure)
        {
            for (int row = 0; row < figure.GetLength(0); row++)
            {
                for (int col = 0; col < figure.GetLongLength(1); col++)
                {
                    if (figure[row, col] && Field[CurrentRow + row, CurrentCol + col - 1])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        static bool CollisionRight(bool[,] figure)
        {
            for (int row = 0; row < figure.GetLength(0); row++)
            {
                for (int col = 0; col < figure.GetLongLength(1); col++)
                {
                    if (figure[row, col] && Field[CurrentRow + row, CurrentCol + col + 1])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
