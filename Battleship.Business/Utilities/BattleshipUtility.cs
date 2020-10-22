using System;
using System.Linq;
using Battleship.Business.Models;

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

        public string AddBattleship(Cell[][] board, int row, int column, int shipSize, string alignment)
        {
            if (row <= 0 || row > Constants.BattleShip.MaxRows)
                return $"Not a valid row of {++row}, cannot be less than 1 or more than {Constants.BattleShip.MaxRows}";

            if (column <= 0 || column > Constants.BattleShip.MaxColumns)
                return $"Not a valid column of {++column}, cannot be less than 1 or more than {Constants.BattleShip.MaxColumns}";

            // Decrement the column and row as they are used within a jagged array
            column = --column;
            row = --row;

            // 1) Add to playerBattleship
            var startingPointValue = board[row][column].Value;
            if (startingPointValue == Constants.BattleShip.Battleship)
                return $"You have already have a battleship at position of row: {++row}, column: {++column}";

            if (string.Equals(alignment, Constants.BattleShip.Horizontal, StringComparison.CurrentCultureIgnoreCase))
            {
                var selectedRow = board[row];
                var shipEndPosition = column + shipSize;
                var endCell = selectedRow.ElementAtOrDefault(shipEndPosition - 1);
                if (endCell == null)
                    return $"Cannot create ship at position of row: {++row}, column: {++column} as it will exceed the length of the board";

                for (var j = column; j < shipEndPosition; j++)
                {
                    var battleShip = new Models.Battleship(Constants.BattleShip.Battleship) { Alignment = alignment, StartRow = column, StartColumn = row, Length = shipSize };
                    selectedRow[j] = battleShip;
                }
            }

            if (string.Equals(alignment, Constants.BattleShip.Vertical, StringComparison.CurrentCultureIgnoreCase))
            {
                var shipEndPosition = row + shipSize;
                var endCell = board.ElementAtOrDefault(shipEndPosition - 1);
                if (endCell == null)
                    return $"Cannot create ship at position of row: {++row}, column: {++column} as it will exceed the length of the board";

                for (var i = row; i < shipEndPosition; i++)
                {
                    var battleShip = new Models.Battleship(Constants.BattleShip.Battleship) { Alignment = alignment, StartRow = column, StartColumn = row, Length = shipSize };
                    board[i][column] = battleShip;
                }
            }

            return $"Battleship created starting at position of row: {++row}, column: {++column}";
        }

        public string Attack(Cell[][] board, int row, int column)
        {
            // TODO: Create custom exception
            if (row <= 0 || row > Constants.BattleShip.MaxRows)
                return $"Not a valid row of {++row}, cannot be less than 1 or more than {Constants.BattleShip.MaxRows}";

            if (column <= 0 || column > Constants.BattleShip.MaxColumns)
                return $"Not a valid column of {++column}, cannot be less than 1 or more than {Constants.BattleShip.MaxColumns}";

            // Decrement the column and row as they are used within a jagged array
            column = --column;
            row = --row;

            var selectedRow = board[row];
            var cell = selectedRow[column];
            var hit = false;

            // Check if the cell is destroyed, empty or a battleship
            if (cell.Value == Constants.BattleShip.Destroyed)
            {
                return "You have already destroyed this battleship";
            }
            else if (cell.Value == Constants.BattleShip.PlaceHolder)
            {
                cell.Value = Constants.BattleShip.Missed;
                board[row][column] = cell;
                return $"You have missed at position of row: {++row}, column: {++column}";
            }
            else if (cell is Models.Battleship battleship)
            {
                if (battleship.Value == Constants.BattleShip.Hit)
                    return $"You have already hit at position of row: {++row}, column: {++column}";

                // Update current cell
                battleship.Value = Constants.BattleShip.Hit;
                board[row][column] = battleship;

                // Determine whether a battleship has been destroyed
                var battleShipDestroyed = BattleshipDestroyed(board, battleship, row, column);
                return $"You have {(battleShipDestroyed ? "destroyed a battleship starting" : "hit a battleship")} at position of row: {++row}, column: {++column}";
            }

            return $"You have {(hit ? "a hit" : "missed")} at row: {++row} and column: {++column}";
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
            int hitBattleshipCellsCount = 0;

            // Determine alignment from the supplied battleship
            var horizontalNavigation = battleship.Alignment == Constants.BattleShip.Horizontal;

            // Navigate the board horizontally by default using the supplied row, else use the supplied column
            for (var i = horizontalNavigation ? battleship.StartRow : battleship.StartColumn; i < battleship.Length; i++)
            {
                var selectedBattleship = board[horizontalNavigation ? row : i][horizontalNavigation ? i : column];
                if (selectedBattleship.Value == Constants.BattleShip.Hit)
                    ++hitBattleshipCellsCount;
            }

            var battleShipDestroyed = hitBattleshipCellsCount == battleship.Length;

            // Mark on board if destroyed
            if (battleShipDestroyed)
            {
                for (var i = horizontalNavigation ? battleship.StartRow : battleship.StartColumn; i < battleship.Length; i++)
                {
                    // Mark the selected battleship as destroyed
                    var selectedBattleship = board[horizontalNavigation ? row : i][horizontalNavigation ? i : column];
                    selectedBattleship.Value = Constants.BattleShip.Destroyed;

                    // Reset the battleship cell onto the board
                    board[horizontalNavigation ? row : i][horizontalNavigation ? i : column] = selectedBattleship;
                }
            }

            return battleShipDestroyed;
        }
    }
}