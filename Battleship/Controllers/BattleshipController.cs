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
        private IMemoryCache _memoeryCache;

        public BattleshipController(IMemoryCache memoryCache)
        {
            _memoeryCache = memoryCache;
        }

        [HttpGet]
        public BattleshipResult Get()
        {
            var playerBoard = BattleshipUtility.CreateDefaultBoard();
            var opponentBoard = BattleshipUtility.CreateDefaultBoard();

            var battleship1Added = BattleshipUtility.AddBattleship(opponentBoard, 1, 1, 3, BattleshipUtility.Horizontal);
            var battleship2Added = BattleshipUtility.AddBattleship(opponentBoard, 9, 3, 5, BattleshipUtility.Horizontal);
            var battleship3Added = BattleshipUtility.AddBattleship(opponentBoard, 3, 3, 4, BattleshipUtility.Vertical);

            var playerId = Guid.NewGuid().ToString();
            _memoeryCache.GetOrCreate(playerId, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                return playerBoard;
            });

            var opponentId = Guid.NewGuid().ToString();
            _memoeryCache.GetOrCreate(opponentId, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10);
                return opponentBoard;
            });

            return new BattleshipResult
            {
                PlayerBoard = DisplayBoard(playerBoard),
                OpponentBoard = DisplayBoard(opponentBoard),
                Results = new List<string> { battleship1Added, battleship2Added, battleship3Added },
                PlayerId = playerId,
                OpponentId = opponentId
            };
        }

        [HttpPost("add")]
        public BattleshipResult Add([FromBody] BattleshipOptions battleshipOptions)
        {
            var playerBoard = _memoeryCache.Get<Cell[][]>(battleshipOptions.PlayerId);
            var battleshipAdded = BattleshipUtility.AddBattleship(playerBoard, battleshipOptions.Row, battleshipOptions.Column, battleshipOptions.ShipSize.GetValueOrDefault(), battleshipOptions.Alignment);

            var opponentBoard = _memoeryCache.Get<Cell[][]>(battleshipOptions.OpponentId);
            return new BattleshipResult
            {
                PlayerBoard = DisplayBoard(playerBoard),
                OpponentBoard = DisplayBoard(opponentBoard),
                Results = new List<string> { battleshipAdded },
                PlayerId = battleshipOptions.PlayerId,
                OpponentId = battleshipOptions.OpponentId
            };
        }

        [HttpPost("attack")]
        public BattleshipResult Attack([FromBody] BattleshipOptions battleshipOptions)
        {
            var playerBoard = _memoeryCache.Get<Cell[][]>(battleshipOptions.PlayerId);
            var opponentBoard = _memoeryCache.Get<Cell[][]>(battleshipOptions.OpponentId);
            var battleshipAttacked = BattleshipUtility.Attack(opponentBoard, battleshipOptions.Row, battleshipOptions.Column);
            return new BattleshipResult
            {
                PlayerBoard = DisplayBoard(playerBoard),
                OpponentBoard = DisplayBoard(opponentBoard),
                Results = new List<string> { battleshipAttacked },
                PlayerId = battleshipOptions.PlayerId,
                OpponentId = battleshipOptions.OpponentId
            };
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
