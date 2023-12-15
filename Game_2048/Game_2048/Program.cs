using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Game_2048
{
    internal class Program
    {
        //We initialise the random and other variables we're going to use in the program
        static Random random = new Random();

        static int score = 0;

        const int row = 4;
        const int col = 4;
        static int[,] grid = new int[row, col];
        static int[,] gridBuffer = new int[row, col];

        static void Main(string[] args)
        {
            //First, we generate two numbers in the table and we print it
            for(int i = 0; i < 2; i++)
            {
                GenerateNumber(grid);
            }
            PrintTable();

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
                PrintTable();

                if (weDidIt(grid))
                {
                    Console.WriteLine("Vous avez gagné! \nVous pouvez continuer de jouer si vous le souhaitez ou quitter le programme avec c.");
                }

                if (!canMove(grid))
                {
                    Console.WriteLine("Raté! Réessayez la prcochine fois :)");
                    reference = false;
                }
            }
            Console.ReadKey();
        }

        //We use this function to print the table
        static void PrintTable()
        {
            Console.Clear();

            //Affiche le nom du jeu
            Console.WriteLine("####### 2048 GAME #######\n");

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Colors(grid[i, j]);
                    Console.Write(grid[i, j] + "\t");
                    Console.ResetColor();
                }
                
                for (int z = 0; z < 2; z++)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine("score: {0}\n", score);
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
                int randomIndex = random.Next(emptyTiles.Count);
                Tuple<int, int> randomEmptyTile = emptyTiles[randomIndex];
                int newValue = (random.Next(10) == 0) ? 4 : 2;
                table[randomEmptyTile.Item1, randomEmptyTile.Item2] = newValue;
            }
        }

        //We use this function to change the order in a linear array of 4 (one line of the game)
        //This function also merges the tiles after having moved them
        static int[] changeOrder(int nb0, int nb1, int nb2, int nb3)
        {
            //This is to move the tiles
            if(nb2 == 0 && nb3 > 0)
            {
                nb2 = nb3;
                nb3 = 0;
            }

            if(nb1 == 0 && nb2 > 0)
            {
                nb1 = nb2;
                nb2 = nb3;
                nb3 = 0;
            }

            if(nb0 == 0 && nb1 > 0)
            {
                nb0 = nb1;
                nb1 = nb2;
                nb2 = nb3;
                nb3 = 0;
            }

            //This is to merge the tiles
            if(nb0 == nb1)
            {
                nb0 += nb1;
                nb1 = nb2;
                nb2 = nb3;
                nb3 = 0;
                score += nb0;
            }

            if(nb1 == nb2)
            {
                nb1 += nb2;
                nb2 = nb3;
                nb3 = 0;
                score += nb1;
            }

            if(nb2 == nb3)
            {
                nb2 += nb3;
                nb3 = 0;
                score += nb2;
            }

            int[] ordre = { nb0, nb1, nb2, nb3 };
            return ordre;
        }

        //We use this function to move UP
        static void MoveUp(int[,] table)
        {
            for(int y = 0; y < grid.GetLength(0); y++)
            {
                int[] column = changeOrder(table[0, y], table[1, y], table[2, y], table[3, y]);

                for(int i = 0; i < column.Length; i++)
                {
                    gridBuffer[i, y] = column[i];
                }
            }
            Array.Copy(gridBuffer, grid, grid.Length);
        }

        //we use this function to move DOWN
        static void MoveDown(int[,] table)
        {
            for(int y = 0; y < grid.GetLength(0); y++)
            {
                int[] column = changeOrder(table[3, y], table[2, y], table[1, y], table[0, y]);
                int index = 0;

                for (int i = column.Length - 1 ; i >= 0; i--)
                {
                    gridBuffer[i, y] = column[index];
                    index++;
                }
            }
            Array.Copy(gridBuffer, grid, grid.Length);
        }

        //we use this function to move to the LEFT
        static void MoveLeft(int[,] table)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                int[] line = changeOrder(table[x, 0], table[x, 1], table[x, 2], table[x, 3]);

                for(int i = 0; i < line.Length; i++)
                {
                    gridBuffer[x, i] = line[i];
                }
            }
            Array.Copy(gridBuffer, grid, grid.Length);
        }

        //We use this function to move to the RIGHT
        static void MoveRight(int[,] table)
        {
            for (int x = 0;  x < grid.GetLength(0); x++)
            {
                int[] line = changeOrder(table[x, 3], table[x, 2], table[x, 1], table[x, 0]);
                int index = 0;

                for(int i = line.Length - 1; i >= 0; i--)
                {
                    gridBuffer[x, i] = line[index];
                    index++;
                }
            }
            Array.Copy(gridBuffer, grid, grid.Length);
        }

        //We use this function to give colors to the array's cases depending on their value
        static void Colors(int value)
        {   
            switch(value)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    return;
                
                case 2:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                
                case 4:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                
                case 8:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                
                case 16:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                
                case 32:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case 64:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                
                case 128:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                
                case 256:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                
                case 512:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                
                case 1024:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                
                case 2048:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
            }
        }

        //We use this function to check if the player won by creating a 2048 tile
        static bool weDidIt(int[,] table)
        {
            for(int i = 0; i < table.GetLength(0); i++)
            {
                for(int j = 0; j < table.GetLength(1); j++)
                {
                    if (table[i, j] >= 2048)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //we use this function to check if the player can still move. If he can't, then he looses
        static bool canMove(int[,] table)
        {
            for(int row = 0; row < table.GetLength(0); row++)
            {
                for(int col = 0; col < table.GetLength(1); col++)
                {
                    if (table[row, col] == 0)
                    {
                        return true;
                    }
                    
                    if (row < table.GetLength(0) - 1 && table[row, col] == table[row + 1, col])
                    {
                        return true;
                    }

                    if(col < table.GetLength(0) - 1 && table[row, col] == table[row, col + 1])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

}
