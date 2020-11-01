using Battleship.API.Controllers;
using Battleship.Business.Models;
using Battleship.Business.Utilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Battleship.Business.Constants;
using Battleship.API.Models;
using Battleship.Business.Enums;

namespace Battleship.Api.Tests.Controllers
{
    public class BattleshipControllerTests
    {
        [TestClass]
        public class Get
        {
            // Set up test initialise class
            private static Mock<IBattleshipUtility> _battleshipUtility;

            [ClassInitialize]
            public static void Initialise(TestContext context)
            {
                _battleshipUtility = new Mock<IBattleshipUtility>();
            }

            [TestMethod]
            public void ReturnsValidPlayerBoard()
            {
                // Arrange
                var playerId = 1;
                var defaultBoard = BattleshipUtilityTestHelpers.CreateDefaultBoard();
                _battleshipUtility.Setup(m => m.CreateDefaultBoard()).Returns(defaultBoard);
                var memoryCache = new MemoryCache(new MemoryCacheOptions());
                var battleshipController = new BattleshipController(memoryCache, _battleshipUtility.Object);

                // Act
                var result = battleshipController.Get();

                // Assert
                Assert.AreEqual(result.PlayerId, playerId);
                Assert.IsNotNull(result.PlayerBoard);
            }

        }

        [TestClass]
        public class Post
        {
            // Set up test initialise class
            private static Mock<IBattleshipUtility> _battleshipUtility;

            [ClassInitialize]
            public static void Initialise(TestContext context)
            {
                _battleshipUtility = new Mock<IBattleshipUtility>();
            }

            [TestMethod]
            public void AddsValidBattleship()
            {
                // Arrange
                var battleshipOptions = new BattleshipOptions { Alignment = BattleShip.Horizontal, Column = 1, PlayerId = 1, Row = 1, ShipSize = 4, OpponentId = 2 };

                // TODO: Create a new BattleshipUtilityResult with an enum specifying the type of the result, string containing the return message and any other additional required properties
                _battleshipUtility.Setup(
                    m => m.AddBattleship(
                            It.IsAny<Cell[][]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                        .Returns(new BattleshipUtilityResult("Battleship added", BattleshipResultType.Added));

                var memoryCache = MockMemoryCacheService.GetMemoryCache(BattleshipUtilityTestHelpers.CreateDefaultBoard());
                var battleshipController = new BattleshipController(memoryCache, _battleshipUtility.Object);

                // Act
                var result = battleshipController.Add(battleshipOptions);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(result.ResultType, BattleshipResultType.Added);
            }
        }

        public static class MockMemoryCacheService
        {
            public static IMemoryCache GetMemoryCache(object expectedValue)
            {
                var mockMemoryCache = new Mock<IMemoryCache>();
                mockMemoryCache
                    .Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
                    .Returns(true);
                return mockMemoryCache.Object;
            }
        }

        public static class BattleshipUtilityTestHelpers
        {
            public static Cell[][] CreateDefaultBoard()
            {
                return new Cell[10][]
                {
                new []{ new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder) , new Cell(BattleShip.PlaceHolder) },
                new []{ new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder) , new Cell(BattleShip.PlaceHolder) },
                new []{ new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder) , new Cell(BattleShip.PlaceHolder) },
                new []{ new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder) , new Cell(BattleShip.PlaceHolder) },
                new []{ new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder) , new Cell(BattleShip.PlaceHolder) },
                new []{ new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder) , new Cell(BattleShip.PlaceHolder) },
                new []{ new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder) , new Cell(BattleShip.PlaceHolder) },
                new []{ new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder) , new Cell(BattleShip.PlaceHolder) },
                new []{ new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder) , new Cell(BattleShip.PlaceHolder) },
                new []{ new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder), new Cell(BattleShip.PlaceHolder) , new Cell(BattleShip.PlaceHolder) }
                };
            }
        }
    }
}
