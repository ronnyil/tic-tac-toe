using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XOGame;


namespace XOUnitTests
{
    [TestClass]
    public class XOGameUnitTest
    {
        [TestMethod]
        public void TestGameAfterNoMoves()
        {
            //Testing GetWinner
            XOGame.XOGame _testGame = new XOGame.XOGame();
            XOPlayer expectedWinner = XOPlayer.NotSet;
            XOPlayer actualWinner = _testGame.GetWinner();
            Assert.AreEqual<XOPlayer>(expectedWinner,actualWinner,
                String.Format("The winner is supposed to be 'NotSet'. The winner was '{0}'.",actualWinner));

            //Testing GetValueInSpot
            Random random = new Random();
            XOPlayer expectedValue = XOPlayer.NotSet;
            int x = random.Next(0, 2), y = random.Next(0, 2);
            XOPlayer actualValue = _testGame.GetValueInSpot(x, y);
            Assert.AreEqual<XOPlayer>(expectedValue, actualValue,
                String.Format("The winner in ({0},{1}) was {2}. Expected 'NotSet'.", x, y, actualValue));


            //Testing game over
            bool expectedResult = false;
            bool actualResult = _testGame.IsGameOver;
            Assert.AreEqual<bool>(expectedResult, actualResult, "The game can't be over after no moves");
        }

        [TestMethod]
        public void TestGameAfterOneMove()
        {
            XOGame.XOGame _testGame = new XOGame.XOGame();
            
            Random random = new Random();
            int x = random.Next(0, 2), y = random.Next(0, 2);

            _testGame.MakeAMove(XOPlayer.X, x, y);

            XOPlayer expectedPlayer = XOPlayer.X;
            XOPlayer actualPlayer = _testGame.GetValueInSpot(x, y);
            Assert.AreEqual<XOPlayer>(expectedPlayer, actualPlayer,
                String.Format("The player in ({0},{1}) was '{2}'. It should have been '{3}'.",x,y,actualPlayer,expectedPlayer));

            XOPlayer expectedWinner = XOPlayer.NotSet;
            XOPlayer actualWinner = _testGame.GetWinner();
            Assert.AreEqual<XOPlayer>(expectedWinner, actualWinner,
                String.Format("The winner is supposed to be 'NotSet'. The winner was '{0}'.", actualWinner));
        }

        [TestMethod]
        [ExpectedException(typeof(XOInvalidMoveException))]
        public void TestTwoOfTheSamePlayerInARow()
        {
            XOGame.XOGame _testGame = new XOGame.XOGame();

            _testGame.MakeAMove(XOPlayer.X, 0, 0);
            _testGame.MakeAMove(XOPlayer.X, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidMove()
        {
            XOGame.XOGame _testGame = new XOGame.XOGame();

            _testGame.MakeAMove(XOPlayer.X, 0, 3);
        }

        [TestMethod]
        public void TestWholeGameToDraw()
        {
            XOGame.XOGame _testGame = new XOGame.XOGame();
            _testGame.MakeAMove(XOPlayer.X, 0, 0);
            _testGame.MakeAMove(XOPlayer.O, 1, 1);
            _testGame.MakeAMove(XOPlayer.X, 1, 0);
            _testGame.MakeAMove(XOPlayer.O, 2, 0);
            _testGame.MakeAMove(XOPlayer.X, 0, 2);
            _testGame.MakeAMove(XOPlayer.O, 0, 1);
            _testGame.MakeAMove(XOPlayer.X, 2, 1);
            _testGame.MakeAMove(XOPlayer.O, 1, 2);
            _testGame.MakeAMove(XOPlayer.X, 2, 2);

            bool expectedGameOver = true;
            bool actualGameOver = _testGame.IsGameOver;
            Assert.AreEqual<bool>(expectedGameOver, actualGameOver, "The game was supposed to be over by now");

            XOPlayer expectedWinner = XOPlayer.NotSet;
            XOPlayer actualWinner = _testGame.GetWinner();
            Assert.AreEqual<XOPlayer>(expectedWinner, actualWinner, "The game was supposed to be a draw, instead '" + actualWinner.ToString() + "' was the winner.");

            Assert.AreEqual<int>(0, _testGame.WinningSolution.Count, "The solution should be empty.");
        }

        [TestMethod]
        [ExpectedException(typeof(XOInvalidMoveException))]
        public void TestWholeGameToOWin()
        {
            XOGame.XOGame _testGame = new XOGame.XOGame();
            _testGame.MakeAMove(XOPlayer.X, 0, 0);
            _testGame.MakeAMove(XOPlayer.O, 1, 1);
            _testGame.MakeAMove(XOPlayer.X, 2, 1);
            _testGame.MakeAMove(XOPlayer.O, 2, 0);
            _testGame.MakeAMove(XOPlayer.X, 1, 2);
            _testGame.MakeAMove(XOPlayer.O, 0, 2);

            bool expectedGameOver = true;
            bool actualGameOver = _testGame.IsGameOver;
            Assert.AreEqual<bool>(expectedGameOver, actualGameOver, "The game was supposed to be over by now");

            XOPlayer expectedWinner = XOPlayer.O;
            XOPlayer actualWinner = _testGame.GetWinner();
            Assert.AreEqual<XOPlayer>(expectedWinner, actualWinner, "The winner was supposed to be 'O', instead '" + actualWinner.ToString() + "' was the winner.");

            _testGame.MakeAMove(XOPlayer.X, 2, 2);
        }
    }
}
