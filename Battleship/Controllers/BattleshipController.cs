using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleship.API.Models;
using Battleship.Business.Models;
using Battleship.Business.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Battleship.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BattleshipController : ControllerBase
    {
        // Constants
        private const int PlayerId = 1;
        private const int OpponentId = 2;

        // Services
        private readonly IMemoryCache _memoryCache;
        private readonly IBattleshipUtility _battleshipUtility;

        public BattleshipController(IMemoryCache memoryCache, IBattleshipUtility battleshipUtility)
        {
            _memoryCache = memoryCache;
            _battleshipUtility = battleshipUtility;
        }

        [HttpGet]
        public BattleshipResult Get()
        {
            var playerBoard = _battleshipUtility.CreateDefaultBoard();
            var opponentBoard = _battleshipUtility.CreateDefaultBoard();

            // Add battle ships for opponent
            var battleship1Added = _battleshipUtility.AddBattleship(opponentBoard, 1, 1, 3, Business.Constants.BattleShip.Horizontal);
            var battleship2Added = _battleshipUtility.AddBattleship(opponentBoard, 9, 3, 5, Business.Constants.BattleShip.Horizontal);
            var battleship3Added = _battleshipUtility.AddBattleship(opponentBoard, 3, 3, 4, Business.Constants.BattleShip.Vertical);

            _memoryCache.GetOrCreate(PlayerId, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                return playerBoard;
            });

            _memoryCache.GetOrCreate(OpponentId, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                return opponentBoard;
            });

            return new BattleshipResult
            {
                PlayerBoard = DisplayBoard(playerBoard),
                OpponentBoard = DisplayBoard(opponentBoard),
                Results = new List<string> { battleship1Added, battleship2Added, battleship3Added },
                PlayerId = PlayerId,
                OpponentId = OpponentId
            };
        }

        [HttpPost("add")]
        public BattleshipResult Add([FromBody] BattleshipOptions battleshipOptions)
        {
            var playerBoard = _memoryCache.Get<Cell[][]>(battleshipOptions.PlayerId);
            var battleshipAdded = _battleshipUtility.AddBattleship(playerBoard, battleshipOptions.Row, battleshipOptions.Column, battleshipOptions.ShipSize.GetValueOrDefault(), battleshipOptions.Alignment);

            var opponentBoard = _memoryCache.Get<Cell[][]>(battleshipOptions.OpponentId);
            return new BattleshipResult
            {
                PlayerBoard = DisplayBoard(playerBoard),
                OpponentBoard = DisplayBoard(opponentBoard),
                Results = new List<string> { battleshipAdded },
                PlayerId = battleshipOptions.PlayerId,
                OpponentId = battleshipOptions.OpponentId
            };
        }

        [HttpPost("attackOpponent")]
        public BattleshipResult AttackOpponent([FromBody] BattleshipOptions battleshipOptions)
        {
            var playerBoard = _memoryCache.Get<Cell[][]>(battleshipOptions.PlayerId);
            var opponentBoard = _memoryCache.Get<Cell[][]>(battleshipOptions.OpponentId);
            var battleshipAttacked = _battleshipUtility.Attack(opponentBoard, battleshipOptions.Row, battleshipOptions.Column);
            return new BattleshipResult
            {
                PlayerBoard = DisplayBoard(playerBoard),
                OpponentBoard = DisplayBoard(opponentBoard),
                Results = new List<string> { battleshipAttacked },
                PlayerId = battleshipOptions.PlayerId,
                OpponentId = battleshipOptions.OpponentId
            };
        }

        [HttpPost("reset")]
        public string Reset([FromBody] BattleshipOptions battleshipOptions)
        {
            _memoryCache.Remove(battleshipOptions.PlayerId);
            _memoryCache.Remove(battleshipOptions.OpponentId);
            return "Board has been reset";
        }

        private static List<string> DisplayBoard(Cell[][] board)
        {
            var values = new List<string>();
            for (var row = 0; row < board.Length; row++)
            {
                var output = string.Empty;
                var selectedRow = board[row];
                for (var column = 0; column < selectedRow.Length; column++)
                {
                    var value = selectedRow[column].Value;
                    output += $"{value} ";
                }

                values.Add(output);
            }

            return values;
        }
    }
}
