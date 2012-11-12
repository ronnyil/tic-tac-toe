using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XOGame;

namespace TicTacToeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            XOGame.XOGame game = new XOGame.XOGame();
            XOPlayer turn = XOPlayer.X;
            int row, column;
            Console.WriteLine("X goes first.");
            while (!game.IsGameOver)
            {                
                PrintBoard(game);
                Console.WriteLine();
                Console.WriteLine("Current Player: " + turn.ToString());
                Console.Write("Row:");
                var key = Console.ReadLine();
                row = int.Parse(key);
                Console.Write("Column:");
                key = Console.ReadLine();
                column = int.Parse(key);
                game.MakeAMove(turn, row, column);
                int tturn = (int)turn;
                tturn = tturn * -1;
                turn = (XOPlayer)tturn;
                Console.Clear();
            }
            switch (game.GetWinner())
            {
                case XOPlayer.X:
                    Console.WriteLine("X is the winner!");
                    break;
                case XOPlayer.O:
                    Console.WriteLine("O is the winner!");
                    break;
                case XOPlayer.NotSet:
                    Console.WriteLine("It's a draw!");
                    break;
                default:
                    break;
            }
            PrintBoard(game);
            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
        static void PrintBoard(XOGame.XOGame game)
        {
            Console.WriteLine();
            for (int i = 0; i < 3; i++)
            {
                Console.Write(" ");
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(" ");
                    switch (game.GetValueInSpot(i,j))
                    {
                        case XOPlayer.X:
                            Console.Write("X");
                            break;
                        case XOPlayer.O:
                            Console.Write("O");
                            break;
                        case XOPlayer.NotSet:
                            Console.Write(" ");
                            break;
                        default:
                            break;
                    }
                    Console.Write(" ");
                    if (j < 2)
                    {
                        
                        Console.Write("|");
                    }
                }
                Console.WriteLine();
                if (i < 2)
                {                    
                    Console.WriteLine(" -----------");
                }
            }
        }
    }
}
