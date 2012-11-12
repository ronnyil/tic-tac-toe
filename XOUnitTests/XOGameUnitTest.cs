using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XOGame;


namespace XOUnitTests
{
    [TestClass]
    public class XOGameUnitTest
    {
        private XOGame.XOGame _testGame;
        [TestInitialize]
        public void SetUpGame()
        {
            _testGame = new XOGame.XOGame();
        }
        [TestMethod]
        public void TestGameWinnerAfterNoMoves()
        {
            XOPlayer expectedWinner = XOPlayer.NotSet;
            XOPlayer actualWinner = _testGame.GetWinner();
            Assert.AreEqual<XOPlayer>(expectedWinner,actualWinner,
                String.Format("The winner is supposed to be 'NotSet'. The winner was '{0}'.",actualWinner));
        }
        [TestMethod]
        public void TestGameGetValueInSpotAfterNoMoves()
        {
            Random random = new Random();
            XOPlayer expectedValue = XOPlayer.NotSet;
            int x = random.Next(0, 2), y = random.Next(0, 2);
            XOPlayer actualValue = _testGame.GetValueInSpot(x, y);
            Assert.AreEqual<XOPlayer>(expectedValue,actualValue,
                String.Format("The winner in ({0},{1}) was {2}. Expected 'NotSet'.",x,y,actualValue));
        }
    }
}
