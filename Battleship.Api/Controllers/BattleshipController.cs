using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleship.API.Models;
using Battleship.Business.Enums;
using Battleship.Business.Models;
using Battleship.Business.Utilities;
using Battleship.Web.Models;
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
            // Cache both the player board
            var playerBoard = _memoryCache.GetOrCreate(PlayerId, entry =>
            {
                // TODO: Get or Add to a mongoDB and then cache
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return _battleshipUtility.CreateDefaultBoard();
            });

            var battleship1Added = string.Empty;
            var battleship2Added = string.Empty;
            var battleship3Added = string.Empty;

            // Cache the opponent board
            var opponentBoard = _memoryCache.GetOrCreate(OpponentId, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                var opponentBoard = _battleshipUtility.CreateDefaultBoard();

                // Add battle ships for opponent
                battleship1Added = _battleshipUtility.AddBattleship(opponentBoard, 1, 1, 3, Business.Constants.BattleShip.Horizontal)?.Message;
                battleship2Added = _battleshipUtility.AddBattleship(opponentBoard, 9, 3, 5, Business.Constants.BattleShip.Horizontal)?.Message;
                battleship3Added = _battleshipUtility.AddBattleship(opponentBoard, 3, 3, 4, Business.Constants.BattleShip.Vertical)?.Message;
                return opponentBoard;
            });

            return new BattleshipResult
            {
                PlayerBoard = DisplayBoard(playerBoard),
                OpponentBoard = DisplayBoard(opponentBoard),
                Results = new[] { battleship1Added, battleship2Added, battleship3Added },
                PlayerId = PlayerId,
                OpponentId = OpponentId
            };
        }

        [HttpPost("add")]
        public ActionResult<BattleshipResult> Add([FromBody] BattleshipOptions battleshipOptions)
        {
            // TODO: Get specific BadRequest message
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var playerBoard = _memoryCache.Get<Cell[][]>(battleshipOptions.PlayerId);
            if (playerBoard == null)
                return NotFound(nameof(playerBoard));

            var result = _battleshipUtility.AddBattleship(playerBoard, battleshipOptions.Row.GetValueOrDefault(), battleshipOptions.Column.GetValueOrDefault(), battleshipOptions.ShipSize.GetValueOrDefault(), battleshipOptions.Alignment);

            // TODO: Test NotFound result message
            if (result == null)
                return NotFound(nameof(result));

            // TODO: Try with an error message
            var opponentBoard = _memoryCache.Get<Cell[][]>(battleshipOptions.OpponentId);
            if (opponentBoard == null)
                return NotFound();

            return new BattleshipResult
            {
                PlayerBoard = DisplayBoard(playerBoard),
                OpponentBoard = DisplayBoard(opponentBoard),
                Results = new[] { result.Message },
                PlayerId = battleshipOptions.PlayerId.GetValueOrDefault(),
                OpponentId = battleshipOptions.OpponentId.GetValueOrDefault(),
                ResultType = result.ResultType
            };
        }

        [HttpPost("attackOpponent")]
        public ActionResult<BattleshipResult> AttackOpponent([FromBody] BattleshipOptions battleshipOptions)
        {
            var playerBoard = _memoryCache.Get<Cell[][]>(battleshipOptions.PlayerId);
            if (playerBoard == null)
                return NotFound(nameof(playerBoard));

            var opponentBoard = _memoryCache.Get<Cell[][]>(battleshipOptions.OpponentId);
            if (opponentBoard == null)
                return NotFound(nameof(opponentBoard));

            var battleshipAttacked = _battleshipUtility.Attack(opponentBoard, battleshipOptions.Row.GetValueOrDefault(), battleshipOptions.Column.GetValueOrDefault());
            return new BattleshipResult
            {
                PlayerBoard = DisplayBoard(playerBoard),
                OpponentBoard = DisplayBoard(opponentBoard),
                Results = new[] { battleshipAttacked.Message },
                PlayerId = battleshipOptions.PlayerId.GetValueOrDefault(),
                OpponentId = battleshipOptions.OpponentId.GetValueOrDefault()
            };
        }

        [HttpPost("reset")]
        public string Reset([FromBody] BattleshipOptions battleshipOptions)
        {
            _memoryCache.Remove(battleshipOptions.PlayerId);
            _memoryCache.Remove(battleshipOptions.OpponentId);
            return $"Player board: {battleshipOptions.PlayerId} and opponent board: {battleshipOptions.OpponentId} have been reset";
        }

        private List<string> DisplayBoard(Cell[][] board)
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
