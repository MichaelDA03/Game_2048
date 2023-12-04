using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Game_2048
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            GenerateNumber(grid);
            GenerateNumber(grid);
            AfficherTableau();

            bool reference = true;

            while (reference)
            {

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                ConsoleKey key = keyInfo.Key;

                //We use a switch to detect which key the user pressed and move the tiles accordingly

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        MoveUp(grid);
                        break;

                    case ConsoleKey.DownArrow:
                        MoveDown(grid);
                        break;

                    case ConsoleKey.LeftArrow:
                        MoveLeft(grid);
                        break;

                    case ConsoleKey.RightArrow:
                        MoveRight(grid);
                        break;

                    case ConsoleKey.C:
                        reference = false;
                        break;

                    default:
                        Console.WriteLine("Veuillez appuyer sur un touche valide");
                        break;
                }
                GenerateNumber(grid);
                AfficherTableau();
            }
            Console.ReadKey();
        }

        //We initialise the user's score

        static int score = 0;

        //We initiate the table

        const int row = 4;
        const int col = 4;
        static int[,] grid = new int[row, col];

        //We use this function to print the table

        static void AfficherTableau()
        {
            Console.Clear();
            //Affiche le nom du jeu
            Console.WriteLine("####### 2048 GAME #######\n");

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Console.Write(grid[i, j] + "\t ");
                }

                Console.WriteLine();
                Console.WriteLine();

            }

        }

        //We use this function to generate a random number and put it in the grid

        static void GenerateNumber(int[,] table)
        {
            int size = table.GetLength(0);

            //We first get the coordinates of the enmpty tiles in the table
            List<Tuple<int, int>> emptyTiles = new List<Tuple<int, int>>();

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (table[x, y] == 0)
                    {
                        emptyTiles.Add(Tuple.Create(x, y));
                    }
                }
            }

            //If there are empty tiles, we generate either a 2 or a 4 and place it in a random tile
            if (emptyTiles.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(emptyTiles.Count);
                Tuple<int, int> randomEmptyTile = emptyTiles[randomIndex];
                int newValue = (random.Next(10) == 0) ? 4 : 2;
                table[randomEmptyTile.Item1, randomEmptyTile.Item2] = newValue;
            }
        }

        //We use this function to move the tiles UP
        static void MoveUp(int[,] table)
        {
            int size = table.GetLength(0);

            for (int y = 0; y < size; y++)
            {
                for (int x = 1; x < size; x++)
                {
                    if (table[x, y] != 0)
                    {
                        int row = x;

                        //We check if we have space in the tile upwards to move
                        //Or if the tile upwards is the same to make the fusion
                        while (row > 0 && (table[row - 1, y] == 0 || (table[row - 1, y] == table[row, y] && table[row, y] != 0)))
                        {
                            //If we have space upwards
                            if (table[row - 1, y] == 0)
                            {
                                table[row - 1, y] = table[row, y];
                                table[row, y] = 0;
                                row--;
                            }
                            //If there is tile to make the fusion
                            else if (table[row - 1, y] == table[row, y])
                            {
                                table[row - 1, y] *= 2;
                                table[row, y] = 0;
                                //We increment the score according to the tile the user fused
                                score += table[row - 1, y];
                                break;
                            }
                        }
                    }
                }
            }
        }

        //We use this function to move the tiles DOWN
        static void MoveDown(int[,] table)
        {
            int size = table.GetLength(0);

            for (int y = 0; y < size; y++)
            {
                for (int x = size - 2; x >= 0; x--)
                {
                    if (table[x, y] != 0)
                    {
                        int row = x;

                        //We check if we have space in the tile underneath to move
                        //Or if the tile underneath is the same to make the fusion
                        while (row < size - 1 && (table[row + 1, y] == 0 || (table[row + 1, y] == table[row, y] && table[row, y] != 0)))
                        {
                            //If we have space underneath
                            if (table[row + 1, y] == 0)
                            {
                                table[row + 1, y] = table[row, y];
                                table[row, y] = 0;
                                row++;
                            }
                            //If there is a tile to make tu fusion
                            else if (table[row + 1, y] == table[row, y])
                            {
                                table[row + 1, y] *= 2;
                                table[row, y] = 0;
                                // We increment the score according to the tile the user fused
                                score += table[row + 1, y];
                                break;
                            }
                        }
                    }
                }
            }
        }

        //We use this function to move the tiles RIGHT
        static void MoveRight(int[,] table)
        {
            int size = table.GetLength(0);

            for (int x = 0; x < size; x++)
            {
                for (int y = size - 2; y >= 0; y--)
                {
                    if (table[x, y] != 0)
                    {
                        int col = y;

                        //We move the tiles to the right
                        while (col < size - 1 && table[x, col + 1] == 0)
                        {
                            table[x, col + 1] = table[x, col];
                            table[x, col] = 0;
                            col++;
                        }
                    }
                }
            }
        }

        //We use this function to move the tiles LEFT
        static void MoveLeft(int[,] table)
        {
            int size = table.GetLength(0);

            for (int x = 0; x < size; x++)
            {
                for (int y = 1; y < size; y++)
                {
                    if (table[x, y] != 0)
                    {
                        int col = y;

                        //We move the tiles to the left
                        while (col > 0 && table[x, col - 1] == 0)
                        {
                            table[x, col - 1] = table[x, col];
                            table[x, col] = 0;
                            col--;
                        }
                    }
                }
            }
        }
    }
}
