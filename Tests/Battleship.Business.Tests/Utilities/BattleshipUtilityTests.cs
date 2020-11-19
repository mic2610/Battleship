using System.Linq;
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
                var battleshipUtility = new BattleshipUtility();

                // Act
                var defaultBoard = battleshipUtility.CreateDefaultBoard();

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
                var battleshipUtility = new BattleshipUtility();

                // Act
                var defaultBoard = battleshipUtility.CreateDefaultBoard();

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
            public void AddsValidBattleship()
            {
                // Arrange
                var battleshipUtility = new BattleshipUtility();
                var defaultBoard = battleshipUtility.CreateDefaultBoard();
                var ship = Constants.BattleShip.Battleship;
                var row = 1;
                var column = 1;
                var shipSize = 3;
                var alignment = Constants.BattleShip.Horizontal;

                // Act
                var result = battleshipUtility.AddBattleship(defaultBoard, row, column, shipSize, alignment);

                // Assert
                var battleship = defaultBoard[--row][--column] as Models.Battleship;
                Assert.IsNotNull(battleship);
                Assert.AreEqual(ship, battleship.Value);
                Assert.AreEqual(row, battleship.RowStart);
                Assert.AreEqual(column, battleship.ColumnStart);
                Assert.AreEqual(shipSize, battleship.Length);
                Assert.AreEqual(alignment, battleship.Alignment);
                Assert.AreEqual(Enums.BattleshipResultType.Added, result.ResultType);
            }

            [TestMethod]
            public void HandlesOverlappingBattleship()
            {
                // Arrange
                var placeholder = Constants.BattleShip.PlaceHolder;
                var battleshipUtility = new BattleshipUtility();
                var defaultBoard = battleshipUtility.CreateDefaultBoard();
                var row = 8;
                var column = 8;
                var shipSize = 4;
                var alignment = Constants.BattleShip.Horizontal;

                // Act
                var result = battleshipUtility.AddBattleship(defaultBoard, row, column, shipSize, alignment);

                // Assert
                var cell = defaultBoard[--row][--column];
                Assert.AreEqual(placeholder, cell.Value);
                Assert.IsNull(cell as Models.Battleship);
                Assert.AreEqual(Enums.BattleshipResultType.BoardOverflow, result.ResultType);
            }
        }

        [TestClass]
        public class Attack
        {
            [TestMethod]
            public void ReturnsHitBattleship()
            {
                // Arrange
                var battleshipUtility = new BattleshipUtility();
                var defaultBoard = battleshipUtility.CreateDefaultBoard();
                var row = 1;
                var column = 1;
                var shipSize = 4;
                var alignment = Constants.BattleShip.Horizontal;
                var hit = Constants.BattleShip.Hit;

                // Act
                battleshipUtility.AddBattleship(defaultBoard, row, column, shipSize, alignment);
                var attackedResult = battleshipUtility.Attack(defaultBoard, row, column);

                // Assert
                var cell = defaultBoard[--row][--column];
                if (cell is Models.Battleship battleship)
                    Assert.AreEqual(hit, battleship.Value);

                Assert.AreEqual(Enums.BattleshipResultType.Hit, attackedResult.ResultType);
            }

            [TestMethod]
            public void ReturnsMissedAttack()
            {
                // Arrange
                var battleshipUtility = new BattleshipUtility();
                var defaultBoard = battleshipUtility.CreateDefaultBoard();
                var row = 1;
                var column = 1;
                var shipSize = 4;
                var alignment = Constants.BattleShip.Vertical;
                var missed = Constants.BattleShip.Missed;

                // Act
                battleshipUtility.AddBattleship(defaultBoard, row, column, shipSize, alignment);
                var attackedResult = battleshipUtility.Attack(defaultBoard, 2, 2);

                // Assert
                var cell = defaultBoard[1][1];
                Assert.AreEqual(missed, cell.Value);
                Assert.AreEqual(Enums.BattleshipResultType.Missed, attackedResult.ResultType);
            }

            [TestMethod]
            public void ReturnsDestroyedHorizontalBattleship()
            {
                // Arrange
                var battleshipUtility = new BattleshipUtility();
                var defaultBoard = battleshipUtility.CreateDefaultBoard();
                var row = 1;
                var column = 1;
                var shipSize = 4;
                var alignment = Constants.BattleShip.Horizontal;
                var destroyed = Constants.BattleShip.Destroyed;

                // Act
                battleshipUtility.AddBattleship(defaultBoard, row, column, shipSize, alignment);
                battleshipUtility.Attack(defaultBoard, row, column);
                battleshipUtility.Attack(defaultBoard, row, column + 1);
                battleshipUtility.Attack(defaultBoard, row, column + 2);
                var finalAttackedResult = battleshipUtility.Attack(defaultBoard, row, column + 3);

                // Assert
                var cell = defaultBoard[--row][--column];
                if (cell is Models.Battleship battleship)
                    Assert.AreEqual(destroyed, battleship.Value);

                Assert.AreEqual(Enums.BattleshipResultType.Destroyed, finalAttackedResult.ResultType);
            }

            [TestMethod]
            public void ReturnsDestroyedVerticalBattleship()
            {
                // Arrange
                var battleshipUtility = new BattleshipUtility();
                var defaultBoard = battleshipUtility.CreateDefaultBoard();
                var row = 1;
                var column = 1;
                var shipSize = 4;
                var alignment = Constants.BattleShip.Vertical;
                var destroyed = Constants.BattleShip.Destroyed;

                // Act
                battleshipUtility.AddBattleship(defaultBoard, row, column, shipSize, alignment);
                battleshipUtility.Attack(defaultBoard, row, column);
                battleshipUtility.Attack(defaultBoard, row + 1, column);
                battleshipUtility.Attack(defaultBoard, row + 2, column);
                var finalAttackedResult = battleshipUtility.Attack(defaultBoard, row + 3, column);

                // Assert
                var cell = defaultBoard[--row][--column];
                if (cell is Models.Battleship battleship)
                    Assert.AreEqual(destroyed, battleship.Value);

                Assert.AreEqual(Enums.BattleshipResultType.Destroyed, finalAttackedResult.ResultType);
            }
        }
    }
}