using Battleship.Business.Models;
using Battleship.Business.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battleship.Business.Tests.Utilities
{
    public class BattleshipUtilityTests
    {
        [TestClass]
        public class CreateDefaultBoard
        {
            [TestMethod]
            public void ReturnsValidSizedBoard()
            {
                // Arrange
                var rows = 10;
                var columns = 10;

                // Act
                var defaultBoard = BattleshipUtility.CreateDefaultBoard();

                // Asset
                Assert.AreEqual(rows, defaultBoard.Length);
                foreach (var row in defaultBoard)
                    Assert.AreEqual(columns, row.Length);
            }

            [TestMethod]
            public void ReturnsValidDefaultCells()
            {
                // Arrange
                var placeholder = Constants.BattleShip.PlaceHolder;

                // Act
                var defaultBoard = BattleshipUtility.CreateDefaultBoard();

                // Asset
                foreach (var row in defaultBoard)
                {
                    foreach (var cell in row)
                        Assert.AreEqual(placeholder, cell.Value);
                }
            }
        }

        [TestClass]
        public class AddBattleship
        {
            [TestMethod]
            public void ReturnsValidBattleship()
            {
                // Arrange
                var defaultBoard = BattleshipUtility.CreateDefaultBoard();
                var ship = Constants.BattleShip.Battleship;
                var row = 1;
                var column = 1;
                var shipSize = 3;
                var alignment = Constants.BattleShip.Horizontal;

                // Act
                BattleshipUtility.AddBattleship(defaultBoard, row, column, shipSize, alignment);

                // Assert
                var battleship = defaultBoard[row][column] as Models.Battleship;
                Assert.IsNotNull(battleship);
                Assert.AreEqual(ship, battleship.Value);
                Assert.AreEqual(row, battleship.StartRow);
                Assert.AreEqual(column, battleship.StartColumn);
                Assert.AreEqual(shipSize, battleship.Length);
                Assert.AreEqual(alignment, battleship.Alignment);
            }

            [TestMethod]
            public void HandlesExistingBattleship()
            {
            }
        }

        [TestClass]
        public class Attack
        {

        }
    }
}