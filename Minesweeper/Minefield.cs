using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Minesweeper
{
    class Minefield
    {
        private bool[,] _bombLocations = new bool[5, 5];
        public int initiated = 0;
        public string[,] gameField = new string[5, 5]; //Hidden
        public string[,] gameBoard = new string[5, 5]; //"Visible"
        public int end = 0;

        public string[,] Populate()
        {
            int h = gameField.GetLength(1);
            int w = gameField.GetLength(0);
            for (int q = w - 1; q >= 0; q--)
            {
                for (int p = 0; p < h; p++)
                {
                    gameBoard[p, q] = "?";
                }
            }
            return gameBoard;
        }

        public void SetBomb(int x, int y)
        {
            _bombLocations[x, y] = true;
        }

        //Get the true/false value at the given x,y coordinates
        public bool GetValue(int x, int y)
        {
            return _bombLocations[x, y];
        }

        //Visualize the gamefield.
        public string[,] CreateGameField()
        {

            //FIRST, set the X:es to mark where there are bombs. Be careful not to do too much here, we don't want to overwrite the bomb locations.
            for (int m = 0; m < 5; m++)
            {
                for (int n = 0; n < 5; n++)
                {
                    if (_bombLocations[m, n] == true)
                    {
                        gameField[m, n] = "X";
                    }
                    else
                    {
                        //place a space where there are no bombs
                        gameField[m, n] = " ";
                    }
                }
            }

            //With the bombs safely placed in gameField, we can count adjacent bombs and set values.
            for (int m = 0; m < 5; m++)
            {
                for (int n = 0; n < 5; n++)
                {
                    int counter = 0;
                    if (gameField[m, n].ToString().Equals(" "))
                    {

                        if (m + 1 < 5)
                        {
                            if (gameField[m + 1, n].ToString().Equals("X"))
                            {
                                counter++;
                            }
                        }

                        if (n + 1 < 5)
                        {
                            if (gameField[m, n + 1].ToString().Equals("X"))
                            {
                                counter++;
                            }
                        }

                        if (m + 1 < 5 && n + 1 < 5)
                        {
                            if (gameField[m + 1, n + 1].ToString().Equals("X"))
                            {
                                counter++;
                            }
                        }

                        if (m - 1 >= 0)
                        {
                            if (gameField[m - 1, n].ToString().Equals("X"))
                            {
                                counter++;
                            }
                        }

                        if (n - 1 >= 0)
                        {
                            if (gameField[m, n - 1].ToString().Equals("X"))
                            {
                                counter++;
                            }
                        }

                        if (m - 1 >= 0 && n - 1 >= 0)
                        {
                            if (gameField[m - 1, n - 1].ToString().Equals("X"))
                            {
                                counter++;
                            }
                        }

                        if (m + 1 < 5 && n - 1 >= 0)
                        {
                            if (gameField[m + 1, n - 1].ToString().Equals("X"))
                            {
                                counter++;
                            }
                        }

                        if (m - 1 >= 0 && n + 1 < 5)
                        {
                            if (gameField[m - 1, n + 1].ToString().Equals("X"))
                            {
                                counter++;
                            }
                        }
                    }
                    //Add the total number of bombs close by to relevant location
                    if (counter > 0)
                    {
                        gameField[m, n] = counter.ToString();
                    }
                    //Reset counter before moving to next coordinate
                    counter = 0;
                }
            }
            return gameField;
        }

        //Update the game field after every input.
        public string[,] UpdateBoard(int X, int Y)
        {
            //Find the location of the coordinates and then loop outward. So while the next step us " ".

            //Vertical
            for (int y = Y; y < 5; y++)
            {
                if (gameField[X, y].ToString().Equals(" "))
                {
                    gameBoard[X, y] = " ";
                    if (X + 1 < 5 && y + 1 < 5)
                    {
                        if (gameField[X + 1, y + 1].ToString().Equals(" "))
                        {
                            gameBoard[X + 1, y + 1] = " ";
                        }
                        if (!gameField[X + 1, y + 1].ToString().Equals(" "))
                        {
                            gameBoard[X + 1, y + 1] = gameField[X + 1, y + 1].ToString();
                        }
                    }

                    if (X + 1 < 5 && y - 1 >= 0)
                    {
                        if (gameField[X + 1, y - 1].ToString().Equals(" "))
                        {
                            gameBoard[X + 1, y - 1] = " ";
                        }
                        if (!gameField[X + 1, y - 1].ToString().Equals(" "))
                        {
                            gameBoard[X + 1, y - 1] = gameField[X + 1, y - 1].ToString();
                        }
                    }

                    if (X - 1 >= 0 && y - 1 >= 0)
                    {
                        if (gameField[X - 1, y - 1].ToString().Equals(" "))
                        {
                            gameBoard[X - 1, y - 1] = " ";
                        }
                        if (!gameField[X - 1, y - 1].ToString().Equals(" "))
                        {
                            gameBoard[X - 1, y - 1] = gameField[X - 1, y - 1].ToString();
                        }
                    }

                    if (X - 1 >= 0 && y + 1 < 5)
                    {
                        if (gameField[X - 1, y + 1].ToString().Equals(" "))
                        {
                            gameBoard[X - 1, y + 1] = " ";
                        }
                        if (!gameField[X - 1, y + 1].ToString().Equals(" "))
                        {
                            gameBoard[X - 1, y + 1] = gameField[X - 1, y + 1].ToString();
                        }
                    }
                }

                if (!gameField[X, y].ToString().Equals(" "))
                {
                    gameBoard[X, y] = gameField[X, y].ToString();
                    break;
                }
            }

            for (int yy = Y; yy >= 0; yy--)
            {
                if (gameField[X, yy].ToString().Equals(" "))
                {
                    gameBoard[X, yy] = " ";
                    if (X + 1 < 5 && yy + 1 < 5)
                    {
                        if (gameField[X + 1, yy + 1].ToString().Equals(" "))
                        {
                            gameBoard[X + 1, yy + 1] = " ";
                        }
                        if (!gameField[X + 1, yy + 1].ToString().Equals(" "))
                        {
                            gameBoard[X + 1, yy + 1] = gameField[X + 1, yy + 1].ToString();
                        }
                    }

                    if (X + 1 < 5 && yy - 1 >= 0)
                    {
                        if (gameField[X + 1, yy - 1].ToString().Equals(" "))
                        {
                            gameBoard[X + 1, yy - 1] = " ";
                        }
                        if (!gameField[X + 1, yy - 1].ToString().Equals(" "))
                        {
                            gameBoard[X + 1, yy - 1] = gameField[X + 1, yy - 1].ToString();
                        }
                    }

                    if (X - 1 >= 0 && yy - 1 >= 0)
                    {
                        if (gameField[X - 1, yy - 1].ToString().Equals(" "))
                        {
                            gameBoard[X - 1, yy - 1] = " ";
                        }
                        if (!gameField[X - 1, yy - 1].ToString().Equals(" "))
                        {
                            gameBoard[X - 1, yy - 1] = gameField[X - 1, yy - 1].ToString();
                        }
                    }

                    if (X - 1 >= 0 && yy + 1 < 5)
                    {
                        if (gameField[X - 1, yy + 1].ToString().Equals(" "))
                        {
                            gameBoard[X - 1, yy + 1] = " ";
                        }
                        if (!gameField[X - 1, yy + 1].ToString().Equals(" "))
                        {
                            gameBoard[X - 1, yy + 1] = gameField[X - 1, yy + 1].ToString();
                        }
                    }
                }

                if (!gameField[X, yy].ToString().Equals(" "))
                {
                    gameBoard[X, yy] = gameField[X, yy].ToString();
                    break;
                }
            }

            //Horizontal
            for (int x = X; x < 5; x++)
            {
                if (gameField[x, Y].ToString().Equals(" "))
                {
                    gameBoard[x, Y] = " ";
                    if (x + 1 < 5 && Y + 1 < 5)
                    {
                        if (gameField[x + 1, Y + 1].ToString().Equals(" "))
                        {
                            gameBoard[x + 1, Y + 1] = " ";
                        }
                        if (!gameField[x + 1, Y + 1].ToString().Equals(" "))
                        {
                            gameBoard[x + 1, Y + 1] = gameField[x + 1, Y + 1].ToString();
                        }
                    }

                    if (x + 1 < 5 && Y - 1 >= 0)
                    {
                        if (gameField[x + 1, Y - 1].ToString().Equals(" "))
                        {
                            gameBoard[x + 1, Y - 1] = " ";
                        }
                        if (!gameField[x + 1, Y - 1].ToString().Equals(" "))
                        {
                            gameBoard[x + 1, Y - 1] = gameField[x + 1, Y - 1].ToString();
                        }
                    }

                    if (x - 1 >= 0 && Y - 1 >= 0)
                    {
                        if (gameField[x - 1, Y - 1].ToString().Equals(" "))
                        {
                            gameBoard[x - 1, Y - 1] = " ";
                        }
                        if (!gameField[x - 1, Y - 1].ToString().Equals(" "))
                        {
                            gameBoard[x - 1, Y - 1] = gameField[x - 1, Y - 1].ToString();
                        }
                    }

                    if (x - 1 >= 0 && Y + 1 < 5)
                    {
                        if (gameField[x - 1, Y + 1].ToString().Equals(" "))
                        {
                            gameBoard[x - 1, Y + 1] = " ";
                        }
                        if (!gameField[x - 1, Y + 1].ToString().Equals(" "))
                        {
                            gameBoard[x - 1, Y + 1] = gameField[x - 1, Y + 1].ToString();
                        }
                    }

                }

                if (!gameField[x, Y].ToString().Equals(" "))
                {
                    gameBoard[x, Y] = gameField[x, Y].ToString();

                    break;
                }
            }

            for (int xx = X; xx >= 0; xx--)
            {
                if (gameField[xx, Y].ToString().Equals(" "))
                {
                    gameBoard[xx, Y] = " ";
                    if (xx + 1 < 5 && Y + 1 < 5)
                    {
                        if (gameField[xx + 1, Y + 1].ToString().Equals(" "))
                        {
                            gameBoard[xx + 1, Y + 1] = " ";
                        }
                        if (!gameField[xx + 1, Y + 1].ToString().Equals(" "))
                        {
                            gameBoard[xx + 1, Y + 1] = gameField[xx + 1, Y + 1].ToString();
                        }
                    }

                    if (xx + 1 < 5 && Y - 1 >= 0)
                    {
                        if (gameField[xx + 1, Y - 1].ToString().Equals(" "))
                        {
                            gameBoard[xx + 1, Y - 1] = " ";
                        }
                        if (!gameField[xx + 1, Y - 1].ToString().Equals(" "))
                        {
                            gameBoard[xx + 1, Y - 1] = gameField[xx + 1, Y - 1].ToString();
                        }
                    }

                    if (xx - 1 >= 0 && Y - 1 >= 0)
                    {
                        if (gameField[xx - 1, Y - 1].ToString().Equals(" "))
                        {
                            gameBoard[xx - 1, Y - 1] = " ";
                        }
                        if (!gameField[xx - 1, Y - 1].ToString().Equals(" "))
                        {
                            gameBoard[xx - 1, Y - 1] = gameField[xx - 1, Y - 1].ToString();
                        }
                    }

                    if (xx - 1 >= 0 && Y + 1 < 5)
                    {
                        if (gameField[xx - 1, Y + 1].ToString().Equals(" "))
                        {
                            gameBoard[xx - 1, Y + 1] = " ";
                        }
                        if (!gameField[xx - 1, Y + 1].ToString().Equals(" "))
                        {
                            gameBoard[xx - 1, Y + 1] = gameField[xx - 1, Y + 1].ToString();
                        }
                    }
                }

                if (!gameField[xx, Y].ToString().Equals(" "))
                {
                    gameBoard[xx, Y] = gameField[xx, Y].ToString();
                    break;
                }
            }
            return gameBoard;
        }

        //Prints the board after every board update while the game still runs + the starting board.
        public void PrintBoard()
        {
            //Print the starting field
            if (initiated == 0)
            {
                System.Console.WriteLine("-----------------------------\n  01234\n4|?????\n3|?????\n2|?????\n1|?????\n0|?????\nPlease choose coordinates x and y separated by space:".Replace("\n", Environment.NewLine)); //UPDATE THIS?
                initiated = 1;
            }
            else if (initiated == 1)
            {
                System.Console.WriteLine("-----------------------------");
                System.Console.WriteLine("  01234");
                int h = gameBoard.GetLength(1);
                int w = gameBoard.GetLength(0);
                int o = gameBoard.GetLength(1) - 1;
                for (int q = w - 1; q >= 0; q--)
                {
                    for (int p = 0; p < h; p++)
                    {
                        if (p == 0)
                        {
                            System.Console.Write(o + "|".Replace("\n", Environment.NewLine));
                            o--;
                        }

                        System.Console.Write("{0}", gameBoard[p, q]);
                    }
                    System.Console.WriteLine();
                }
                System.Console.WriteLine("\nPlease choose coordinates x and y separated by space:".Replace("\n", Environment.NewLine));
            }
        }

        //Game functionality
        public void PlayGame(int X, int Y)
        {
            int correct = 0;
            if (_bombLocations[X, Y] == true)
            {
                end = 1;
            }
            else
            {
                end = 0;
                UpdateBoard(X, Y);
                for (int a = 0; a < 5; a++)
                {
                    for (int b = 0; b < 5; b++)
                    {
                        if (!gameField[a, b].ToString().Equals("X"))
                        {
                            if (gameField[a, b].ToString().Equals(gameBoard[a, b].ToString())) {
                                correct++;
                            }
                        }
                    }
                }
                if (correct == 20)
                {
                    end = 2;
                }
                else
                {
                    correct = 0;
                }
            }
        }
    }
}
