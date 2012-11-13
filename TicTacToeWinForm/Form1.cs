using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XOGame;
namespace TicTacToeWinForm
{
    public partial class Form1 : Form
    {
        private const string movesFile = @"C:\users\raanan\desktop\moves.txt";
        Color buttonColor;
        int XWins, YWins;
        XOGame.XOGame _theGame;
        XOGame.XOPlayer _currentPlayer;
        class ButtonTag
        {
            public int Row { get; set; }
            public int Column { get; set; }
            public override string ToString()
            {
                return "Row: " + Row + ", Column: " + Column;
            }
        }
        private void LogMove(XOPlayer player, int row, int column)
        {
            using (StreamWriter sw = File.AppendText(movesFile))
            {
                sw.WriteLine("_testGame.MakeAMove(XOPlayer.{0},{1},{2});",player,row,column);
            }
        }
        public Form1()
        {

            InitializeComponent();
            buttonColor = button9.BackColor;
            InitializeButtons();
            _currentPlayer = XOPlayer.X;
            XWins = YWins = 0;
            CurrentPlayerLabel.Text = _currentPlayer.ToString();
            _theGame = new XOGame.XOGame();
            if (File.Exists(movesFile))
            {
                File.Delete(movesFile);
            }
        }

        private void InitializeButtons()
        {
            ((Button)tableLayoutPanel1.Controls["button1"]).Tag = new ButtonTag() { Row = 0, Column = 0 };
            ((Button)tableLayoutPanel1.Controls["button2"]).Tag = new ButtonTag() { Row = 0, Column = 1 };
            ((Button)tableLayoutPanel1.Controls["button3"]).Tag = new ButtonTag() { Row = 0, Column = 2 };
            ((Button)tableLayoutPanel1.Controls["button4"]).Tag = new ButtonTag() { Row = 1, Column = 0 };
            ((Button)tableLayoutPanel1.Controls["button5"]).Tag = new ButtonTag() { Row = 1, Column = 1 };
            ((Button)tableLayoutPanel1.Controls["button6"]).Tag = new ButtonTag() { Row = 1, Column = 2 };
            ((Button)tableLayoutPanel1.Controls["button7"]).Tag = new ButtonTag() { Row = 2, Column = 0 };
            ((Button)tableLayoutPanel1.Controls["button8"]).Tag = new ButtonTag() { Row = 2, Column = 1 };
            ((Button)tableLayoutPanel1.Controls["button9"]).Tag = new ButtonTag() { Row = 2, Column = 2 };
            foreach (Control item in tableLayoutPanel1.Controls)
            {
                Button i = (Button)item;
                item.Click += button_Click;
                i.Text = "";
                i.Font = new Font(i.Font.FontFamily, 30);
                i.BackColor = buttonColor;
            }
        }

        void button_Click(object sender, EventArgs e)
        {
            if (_theGame.IsGameOver)
            {
                return;
            }
            Button sender_button = (Button)sender;
            ButtonTag tag = (ButtonTag)sender_button.Tag;
            if (sender_button.Text != "")
            {
                return;
            }
            sender_button.Text = _currentPlayer.ToString();
            LogMove(_currentPlayer, tag.Row, tag.Column);
            _theGame.MakeAMove(_currentPlayer, tag.Row, tag.Column);
            if (_theGame.IsGameOver)
            {
                XOPlayer winner = _theGame.GetWinner();
                switch (winner)
                {
                    case XOPlayer.X:
                        MessageBox.Show("X is the champion!");
                        ShowWinner(_theGame.WinningSolution);
                        XWins++;
                        break;
                    case XOPlayer.O:
                        MessageBox.Show("O is the champion!");
                        ShowWinner(_theGame.WinningSolution);
                        YWins++;
                        break;
                    case XOPlayer.NotSet:
                        MessageBox.Show("Looks like a draw...");
                        break;
                    default:
                        break;
                }
                Timer t = new Timer();
                t.Interval = 3000;
                t.Tick += (s, ea) =>
                {
                    _theGame.ResetGame();
                    _currentPlayer = XOPlayer.X;
                    CurrentPlayerLabel.Text = _currentPlayer.ToString();
                    InitializeButtons();
                    t.Stop();
                };
                t.Start();
                return;
            }
            SwitchTurns();
            CurrentPlayerLabel.Text = _currentPlayer.ToString();
            
        }

        
        private void ShowWinner(List<Point> solution)
        {
            foreach (Point item in solution)
            {
                int num = (item.X * 3 + item.Y) + 1;
                string buttonName = "button" + num.ToString();
                Button curButton = (Button)tableLayoutPanel1.Controls[buttonName];
                curButton.BackColor = Color.White;
            }
        }
        private void SwitchTurns()
        {
            int t = (int)_currentPlayer;
            t *= -1;
            _currentPlayer = (XOPlayer)t;
        }

    }
}
