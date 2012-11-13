using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace XOGame
{

    public enum XOPlayer
    {
        X = 1, O = -1, NotSet
    }
    /// <summary>
    /// The main TicTacToe game class. 
    /// </summary>
    /// <remarks>
    /// Keeps track of a TicTacToe game.
    /// </remarks>
    public class XOGame
    {
        private const int ROWS = 3;
        private const int COLUMNS = 3;
        private XOPlayer[,] _board;
        private XOPlayer _winner;
        private int _winningSolution;
        private List<int> _solutions;
        private int _moves;
        private XOPlayer _lastMove;

        /// <summary>
        /// The XOGame constructor. Initializes all the fields.
        /// </summary>
        /// <seealso cref="ResetGame"/>
        public XOGame()
        {            
            _board = new XOPlayer[ROWS, COLUMNS];
            ResetGame();
            InitializeSolutions();
        }

        public List<System.Drawing.Point> WinningSolution
        {
            get
            {               
                List<Point> retList = new List<Point>();
                if (_winningSolution == 0)
                {
                    return retList;
                }
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
            _winner = XOPlayer.NotSet;
            _lastMove = XOPlayer.NotSet;
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
                    _board[i, j] = XOPlayer.NotSet;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="movingPlayer"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <exception cref="XOInvalidMoveException"></exception>
        public void MakeAMove(XOPlayer movingPlayer, int row, int column)
        {
            if (row >= ROWS || row < 0)
            {
                throw new ArgumentOutOfRangeException("row");
            }
            if (column >= COLUMNS || column < 0)
            {
                throw new ArgumentOutOfRangeException("column");
            }
            if (IsGameOver)
            {
                throw new XOInvalidMoveException("The game is over!", XOInvalidMoveExceptionReasons.GameAlreadyOver);
            }
            if (_lastMove == movingPlayer)
            {
                throw new XOInvalidMoveException("You can't go twice in a row!", XOInvalidMoveExceptionReasons.SamePlayerTwice);
            } 
            if (_board[row,column] != XOPlayer.NotSet)
            {
                throw new XOInvalidMoveException("You can't set an already set place on the board!", XOInvalidMoveExceptionReasons.AlreadySetPlace);
            }
              
            _board[row, column] = movingPlayer;
            _lastMove = movingPlayer;
            _moves++;
            GetWinner();
        }

        public XOPlayer GetWinner()
        {
            if (_winner != XOPlayer.NotSet)
            {
                return _winner;
            }
            int x = 0, o = 0;
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    if (_board[i,j] == XOPlayer.X)
                    {
                        x |= (int)Math.Pow(2.0, i * 3 + j);
                    }
                    else if (_board[i,j] == XOPlayer.O)
                    {
                        o |= (int)Math.Pow(2.0, i * 3 + j);
                    }
                }
            }
            foreach (var solution in _solutions)
            {
                if ((x & solution) == solution)
                {
                    _winner = XOPlayer.X;
                    _winningSolution = solution;
                    return _winner;
                }
                if (((o & solution) == solution))
                {
                    _winner = XOPlayer.O;
                    _winningSolution = solution;
                    return _winner;
                }
            }
            return _winner;
        }
        public XOPlayer GetValueInSpot(int row, int column)
        {
            return _board[row, column];
        }
        public bool IsGameOver
        {
            get
            {
                return (_winner != XOPlayer.NotSet) || (_moves >= 9);
            }
        }

    }
}
