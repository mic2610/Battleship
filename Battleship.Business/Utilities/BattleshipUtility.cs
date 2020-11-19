using System;
using System.Collections.Generic;
using System.Linq;
using Battleship.Business.Enums;
using Battleship.Business.Models;
using Battleship.Core.Extensions;

namespace Battleship.Business.Utilities
{
    public class BattleshipUtility : IBattleshipUtility
    {
        public Cell[][] CreateDefaultBoard()
        {
            return new Cell[10][]
            {
                new []{ new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder) , new Cell(Constants.BattleShip.PlaceHolder) },
                new []{ new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder) , new Cell(Constants.BattleShip.PlaceHolder) },
                new []{ new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder) , new Cell(Constants.BattleShip.PlaceHolder) },
                new []{ new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder) , new Cell(Constants.BattleShip.PlaceHolder) },
                new []{ new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder) , new Cell(Constants.BattleShip.PlaceHolder) },
                new []{ new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder) , new Cell(Constants.BattleShip.PlaceHolder) },
                new []{ new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder) , new Cell(Constants.BattleShip.PlaceHolder) },
                new []{ new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder) , new Cell(Constants.BattleShip.PlaceHolder) },
                new []{ new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder) , new Cell(Constants.BattleShip.PlaceHolder) },
                new []{ new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder), new Cell(Constants.BattleShip.PlaceHolder) , new Cell(Constants.BattleShip.PlaceHolder) }
            };
        }

        /// <summary>
        /// Add a battleship to a board
        /// </summary>
        public BattleshipUtilityResult AddBattleship(Cell[][] board, int row, int column, int shipSize, string alignment)
        {
            var error = ValidatePositions(row, column);
            if (error != null)
                return error;

            // Decrement the column and row as they are used within a jagged array
            column = --column;
            row = --row;

            // 1) Add to playerBattleship
            var startingPointValue = board[row][column].Value;
            if (startingPointValue == Constants.BattleShip.Battleship)
                return new BattleshipUtilityResult($"You have already have a battleship at position of row: {++row}, column: {++column}", BattleshipResultType.AlreadyExists);

            if (string.Equals(alignment, Constants.BattleShip.Horizontal, StringComparison.CurrentCultureIgnoreCase))
            {
                var selectedRow = board[row];
                var shipEndPosition = column + shipSize;
                var endCell = selectedRow.ElementAtOrDefault(shipEndPosition - 1);
                if (endCell == null)
                    return new BattleshipUtilityResult($"Cannot create ship at position of row: {++row}, column: {++column} as it will exceed the length of the board", BattleshipResultType.BoardOverflow);

                for (var c = column; c < shipEndPosition; c++)
                {
                    var battleShip = new Models.Battleship(Constants.BattleShip.Battleship) { Alignment = alignment, RowStart = column, ColumnStart = row, Length = shipSize };
                    selectedRow[c] = battleShip;
                }
            }

            if (string.Equals(alignment, Constants.BattleShip.Vertical, StringComparison.CurrentCultureIgnoreCase))
            {
                var shipEndPosition = row + shipSize;
                var endCell = board.ElementAtOrDefault(shipEndPosition - 1);
                if (endCell == null)
                    return new BattleshipUtilityResult($"Cannot create ship at position of row: {++row}, column: {++column} as it will exceed the length of the board", BattleshipResultType.BoardOverflow);

                for (var r = row; r < shipEndPosition; r++)
                {
                    var battleShip = new Models.Battleship(Constants.BattleShip.Battleship) { Alignment = alignment, RowStart = column, ColumnStart = row, Length = shipSize };
                    board[r][column] = battleShip;
                }
            }

            return new BattleshipUtilityResult($"Battleship created starting at position of row: {++row}, column: {++column}", BattleshipResultType.Added);
        }

        /// <summary>
        /// Attack a cell on a board, marked by a row and column
        /// </summary>
        /// <returns></returns>
        public BattleshipUtilityResult Attack(Cell[][] board, int row, int column)
        {
            var result = ValidatePositions(row, column);
            if (result != null)
                return result;

            // Decrement the column and row as they are used within a jagged array
            column = --column;
            row = --row;

            var selectedRow = board[row];
            var cell = selectedRow[column];

            // Check if the cell is destroyed, empty or a battleship
            if (cell.Value == Constants.BattleShip.Destroyed)
            {
                return new BattleshipUtilityResult("You have already destroyed this battleship", BattleshipResultType.AlreadyDestroyed);
            }
            else if (cell.Value == Constants.BattleShip.PlaceHolder)
            {
                cell.Value = Constants.BattleShip.Missed;
                board[row][column] = cell;
                return new BattleshipUtilityResult($"You have missed at position of row: {++row}, column: {++column}", BattleshipResultType.Missed);
            }
            else if (cell is Models.Battleship battleship)
            {
                if (battleship.Value == Constants.BattleShip.Hit)
                    return new BattleshipUtilityResult($"You have already hit at position of row: {++row}, column: {++column}", BattleshipResultType.AlreadyHit);

                // Update current cell
                battleship.Value = Constants.BattleShip.Hit;
                board[row][column] = battleship;

                // Determine whether a battleship has been destroyed
                var battleShipDestroyed = BattleshipDestroyed(board, battleship, row, column);
                return new BattleshipUtilityResult(
                    $"You have {(battleShipDestroyed ? "destroyed a battleship starting" : "hit a battleship")} at position of row: {++row}, column: {++column}",
                    battleShipDestroyed ? BattleshipResultType.Destroyed : BattleshipResultType.Hit);
            }

            return null;
        }

        /// <summary>
        /// Validate the row and column
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>BattleshipUtilityResult</returns>
        private BattleshipUtilityResult ValidatePositions(int row, int column)
        {
            if (row <= 0 || row > Constants.BattleShip.MaxRows)
                return new BattleshipUtilityResult($"Not a valid row of {++row}, cannot be less than 1 or more than {Constants.BattleShip.MaxRows}", BattleshipResultType.InvalidRow);
            else if (column <= 0 || column > Constants.BattleShip.MaxColumns)
                return new BattleshipUtilityResult($"Not a valid column of {++column}, cannot be less than 1 or more than {Constants.BattleShip.MaxColumns}", BattleshipResultType.InvalidColumn);

            return null;
        }

        /// <summary>
        /// Iterate through a battleship based on alignment to determine how many battleship cells have been hit and if a battleship has been destroyed
        /// </summary>
        /// <param name="board"></param>
        /// <param name="battleship"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>Bool indicating whether the battleship has been destroyed</returns>
        private bool BattleshipDestroyed(Cell[][] board, Models.Battleship battleship, int row, int column)
        {
            var hitBattleshipCellsCount = 0;

            // Determine alignment from the supplied battleship
            var horizontalNavigation = battleship.Alignment == Constants.BattleShip.Horizontal;

            // Create a dictionary with a string key of "{row},{column}" and value of battleship, then add each selected battleship to it and then if hitBattleshipCellsCount has reached the battleship length, navigate through it and mark as destroyed, use a .SafeSplit string extension to attain the row and index to add back to hitBattleshipsLookup 
            // Set initial count so resizing is not required, this is a little more performant friendly
            var hitBattleshipsLookup = new Dictionary<string, Models.Battleship>(battleship.Length);

            // Navigate the board horizontally by default using the supplied row, else use the supplied column
            for (var i = horizontalNavigation ? battleship.RowStart : battleship.ColumnStart; i < battleship.Length; i++)
            {
                var selectedRow = horizontalNavigation ? row : i;
                var selectedColumn = horizontalNavigation ? i : column;
                var selectedBattleship = board[selectedRow][selectedColumn];
                if (selectedBattleship.Value == Constants.BattleShip.Hit)
                {
                    ++hitBattleshipCellsCount;
                    hitBattleshipsLookup[$"{selectedRow},{selectedColumn}"] = selectedBattleship as Models.Battleship;
                }
            }

            var battleShipDestroyed = hitBattleshipCellsCount == battleship.Length;

            // Mark on board if destroyed
            if (battleShipDestroyed)
            {
                foreach (var hitBattleship in hitBattleshipsLookup)
                {
                    var keyParts = hitBattleship.Key.SafeSplit(new[] { "," });
                    var selectedRow = Convert.ToInt32(keyParts[0]);
                    var selectedColumn = Convert.ToInt32(keyParts[1]);

                    // Get the current battleship from the board
                    var selectedBattleship = board[selectedRow][selectedColumn];

                    // Mark the selected battleship as destroyed
                    selectedBattleship.Value = Constants.BattleShip.Destroyed;

                    // Reset the battleship cell onto the board
                    board[selectedRow][selectedColumn] = selectedBattleship;
                }
            }

            return battleShipDestroyed;
        }
    }
}