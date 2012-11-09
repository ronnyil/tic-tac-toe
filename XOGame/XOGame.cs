using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace XOGame
{

    public enum Player
    {
        X = 1, O = -1, NotSet
    }
    public class XOGame
    {
        private const int ROWS = 3;
        private const int COLUMNS = 3;
        private Player[,] _board;
        private Player _winner;
        private int _winningSolution;
        private List<int> _solutions;
        private int _moves;

        public XOGame()
        {            
            _board = new Player[ROWS, COLUMNS];
            ResetGame();
            InitializeSolutions();
        }
        public List<System.Drawing.Point> WinningSolution
        {
            get
            {
                if (_winningSolution == 0)
                {
                    return null;
                }
                List<Point> retList = new List<Point>();
                for (int i = 0; i < ROWS; i++)
                {
                    for (int j = 0; j < COLUMNS; j++)
                    {
                        int cur = (int)Math.Pow(2, i * 3 + j);
                        if ((_winningSolution & cur) == cur)
                        {
                            retList.Add(new Point(i, j));
                        }
                    }
                }
                return retList;
            }
        }
        public void ResetGame()
        {
            _winner = Player.NotSet;
            _winningSolution = 0;
            _moves = 0;
            ResetBoard();
        }
        private void ResetBoard()
        {
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    _board[i, j] = Player.NotSet;
                }
            }
        }

        private void InitializeSolutions()
        {
            _solutions = new List<int>() 
            {
                7,56,448,292,146,73,273,84
            };            
        }

        public void MakeAMove(Player movingPlayer, int row, int column)
        {
            if (_board[row,column] != Player.NotSet)
            {
                throw new Exception("You can't set an already set place on the board!");
            }
            if (row >= ROWS || row < 0 || column >= COLUMNS || column < 0)
            {
                throw new ArgumentOutOfRangeException();                
            }
            _board[row, column] = movingPlayer;
            _moves++;
            GetWinner();
        }

        public Player GetWinner()
        {
            if (_winner != Player.NotSet)
            {
                return _winner;
            }
            int x = 0, o = 0;
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    if (_board[i,j] == Player.X)
                    {
                        x |= (int)Math.Pow(2.0, i * 3 + j);
                    }
                    else if (_board[i,j] == Player.O)
                    {
                        o |= (int)Math.Pow(2.0, i * 3 + j);
                    }
                }
            }
            foreach (var solution in _solutions)
            {
                if ((x & solution) == solution)
                {
                    _winner = Player.X;
                    _winningSolution = solution;
                    return _winner;
                }
                if (((o & solution) == solution))
                {
                    _winner = Player.O;
                    _winningSolution = solution;
                    return _winner;
                }
            }
            return _winner;
        }
        public Player GetValueInSpot(int row, int column)
        {
            return _board[row, column];
        }
        public bool IsGameOver
        {
            get
            {
                return (_winner != Player.NotSet) || (_moves >= 9);
            }
        }

    }
}
