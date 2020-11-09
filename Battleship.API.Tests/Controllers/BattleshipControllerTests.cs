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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Battleship.Api.Tests.Controllers
{
    public class BattleshipControllerTests
    {
        [TestClass]
        public class Get
        {
            // Set up test initialise class
            private static Mock<IBattleshipUtility> _mockBattleshipUtility;

            [ClassInitialize]
            public static void Initialise(TestContext context)
            {
                _mockBattleshipUtility = new Mock<IBattleshipUtility>();
            }

            [TestMethod]
            public void ReturnsValidPlayerBoard()
            {
                // Arrange
                var playerId = 1;
                var defaultBoard = BattleshipUtilityTestHelpers.CreateDefaultBoard();
                _mockBattleshipUtility.Setup(m => m.CreateDefaultBoard()).Returns(defaultBoard);
                var memoryCache = new MemoryCache(new MemoryCacheOptions());
                var battleshipController = new BattleshipController(memoryCache, _mockBattleshipUtility.Object);

                // Act
                var subject = battleshipController.Get();

                // Assert
                Assert.AreEqual(subject.PlayerId, playerId);
                Assert.IsNotNull(subject.PlayerBoard);
            }

        }

        [TestClass]
        public class Add
        {
            // Set up test initialise class
            private static Mock<IBattleshipUtility> _mockBattleshipUtility;

            [ClassInitialize]
            public static void Initialise(TestContext context)
            {
                _mockBattleshipUtility = new Mock<IBattleshipUtility>();
            }

            [TestMethod]
            public void ReturnsValidBattleship()
            {
                // Arrange
                var battleshipOptions = new BattleshipOptions { Alignment = BattleShip.Horizontal, Column = 1, PlayerId = 1, Row = 1, ShipSize = 4, OpponentId = 2 };
                _mockBattleshipUtility.Setup(
                    m => m.AddBattleship(
                            It.IsAny<Cell[][]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                        .Returns(new BattleshipUtilityResult("Battleship added", BattleshipResultType.Added));

                var memoryCache = MockMemoryCacheService.GetMemoryCache(BattleshipUtilityTestHelpers.CreateDefaultBoard());
                var battleshipController = new BattleshipController(memoryCache, _mockBattleshipUtility.Object);

                // Act
                var subject = battleshipController.Add(battleshipOptions);

                // Assert
                Assert.IsNotNull(subject);
                Assert.AreEqual(subject.Value.ResultType, BattleshipResultType.Added);
            }

            [TestMethod]
            public void ReturnsNotFoundWhenPlayerboardIsNull()
            {
                // Arrange
                var battleshipOptions = new BattleshipOptions { Alignment = BattleShip.Horizontal, Column = 1, PlayerId = 1, Row = 1, ShipSize = 4, OpponentId = 2 };
                _mockBattleshipUtility.Setup(m => m.AddBattleship(It.IsAny<Cell[][]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns((BattleshipUtilityResult)null);

                Cell[][] playerBoard = null;
                var memoryCache = MockMemoryCacheService.GetMockedMemoryCache();
                memoryCache.Set(0, playerBoard);
                var battleshipController = new BattleshipController(memoryCache, _mockBattleshipUtility.Object);

                // Act
                var subject = battleshipController.Add(battleshipOptions);

                // Assert
                var notFoundObjectResult = subject.Result as Microsoft.AspNetCore.Mvc.NotFoundObjectResult;
                Assert.IsNull(subject.Value);
                Assert.AreEqual(notFoundObjectResult.StatusCode.Value, StatusCodes.Status404NotFound);
            }

            [TestMethod]
            public void ReturnsBadRequestWhenModelStateIsInvalid()
            {
                // Arrange
                var battleshipOptions = new BattleshipOptions { Alignment = BattleShip.Horizontal, Column = 1, Row = 0, ShipSize = 4, OpponentId = 2 };
                _mockBattleshipUtility.Setup(m => m.AddBattleship(It.IsAny<Cell[][]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns((BattleshipUtilityResult)null);

                Cell[][] playerBoard = null;
                var memoryCache = MockMemoryCacheService.GetMockedMemoryCache();
                memoryCache.Set(0, playerBoard);
                var battleshipController = new BattleshipController(memoryCache, _mockBattleshipUtility.Object);

                // Act
                battleshipController.ModelState.AddModelError("Bad", "Request");
                var subject = battleshipController.Add(battleshipOptions);

                // Assert
                var badRequestObjectResult = subject.Result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult;
                Assert.IsNull(subject.Value);
                Assert.AreEqual(badRequestObjectResult.StatusCode.Value, StatusCodes.Status400BadRequest);
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

            public static IMemoryCache GetMockedMemoryCache()
            {
                var mockMemoryCache = new Mock<IMemoryCache>();
                var cachEntry = new Mock<ICacheEntry>();

                mockMemoryCache
                    .Setup(m => m.CreateEntry(It.IsAny<object>()))
                    .Returns(cachEntry.Object);

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
