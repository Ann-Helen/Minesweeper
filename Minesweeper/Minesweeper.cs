using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Minesweeper
{
    class Minesweeper
    {
        static void Main()
        {
            var field = new Minefield();
            string playerInput = "";
            int X;
            int Y;
            List<String> inputLog = new List<String>();

            //set the bombs...
            field.SetBomb(0, 0);
            field.SetBomb(0, 1);
            field.SetBomb(1, 1);
            field.SetBomb(1, 4);
            field.SetBomb(4, 2);

            //the mine field should look like this now:
            //  01234
            //4|1X1
            //3|11111
            //2|2211X
            //1|XX111
            //0|X31

            // Game code...

            //populate board with "?"
            field.Populate();
            //visualize the field behind the board
            field.CreateGameField();
            //print starting board
            field.PrintBoard();

            //keep the game running until game over
            while (field.end == 0)
            {
                System.Console.WriteLine();
                playerInput = Console.ReadLine().ToString();


                //split input
                string[] instructions = playerInput.Split(' ');


                //make sure the input isn't too long or too short. It should always be 2 values (without the space in the initial input)
                if (instructions.Length == 2)
                {
                    //make sure this input haven't already been used, check the inputLog!
                    for (int c = 0; c <= inputLog.Count; c++)
                    {

                        //make sure values are numbers
                        if (Int32.TryParse(instructions[0], out int tryInt) && Int32.TryParse(instructions[1], out tryInt))
                        {
                            if (inputLog.Contains(playerInput))
                            {
                                System.Console.WriteLine("You have already checked these coordinates.");
                                break;
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("Those are not numbers. Incorrect input. Try again.");
                            //clear the instructions variable or it carries to next round!
                            instructions[0] = "";
                            instructions[1] = "";
                        }

                        if (Int32.TryParse(instructions[0], out tryInt) && Int32.TryParse(instructions[1], out tryInt))
                        {
                            //Log the input data!
                            inputLog.Add(playerInput.ToString());

                            //convert to int
                            X = Int32.Parse(instructions[0]);
                            Y = Int32.Parse(instructions[1]);

                            //Make sure there's no out of bounds!
                            if (X >= 0 && X < 5 && Y >= 0 && Y < 5)
                            {
                                //Call game function
                                field.PlayGame(X, Y);
                                if (field.end == 0)
                                {
                                    field.PrintBoard();
                                }
                                break;
                            }
                            else
                            {
                                System.Console.WriteLine("Incorrect input. Try again.");
                                break;
                            }


                        }
                        //clear the instructions variable or it carries to next round!
                        instructions[0] = "";
                        instructions[1] = "";
                    }
                }
                else
                {
                    System.Console.WriteLine("Incorrect input. Did you use numbers? Try again.");
                }
            }


            //make the hidden field visible here on game over
            if (field.end == 1)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("+++++ BOOOOOOOM! +++++");
                System.Console.WriteLine("  01234");
                int h = field.gameField.GetLength(1);
                int w = field.gameField.GetLength(0);
                int o = field.gameField.GetLength(1) - 1;
                for (int q = w - 1; q >= 0; q--)
                {
                    for (int p = 0; p < h; p++)
                    {
                        if (p == 0)
                        {
                            System.Console.Write(o + "|".Replace("\n", Environment.NewLine));
                            o--;
                        }

                        System.Console.Write("{0}", field.gameField[p, q]);
                    }
                    System.Console.WriteLine();
                }
                System.Console.WriteLine("\nGAME OVER. Thanks for playing!".Replace("\n", Environment.NewLine));
            }

            if (field.end == 2)
            {
                System.Console.WriteLine(". o O * VICTORY !* O o .");
                System.Console.WriteLine("  01234");
                int h = field.gameField.GetLength(1);
                int w = field.gameField.GetLength(0);
                int o = field.gameField.GetLength(1) - 1;
                for (int q = w - 1; q >= 0; q--)
                {
                    for (int p = 0; p < h; p++)
                    {
                        if (p == 0)
                        {
                            System.Console.Write("\n"+ o + "|".Replace("\n", Environment.NewLine));
                            o--;
                        }

                        System.Console.Write("{0}", field.gameField[p, q]);
                    }
                }
                System.Console.WriteLine("\nYOU WON. Thanks for playing!".Replace("\n", Environment.NewLine));
            }
        }
    }
}
