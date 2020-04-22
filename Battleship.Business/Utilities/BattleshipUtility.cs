using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Business.Models;

namespace Battleship.Business.Utilities
{
    public class BattleshipUtility
    {
        public const string Horizontal = nameof(Horizontal);
        public const string Vertical = nameof(Vertical);
        private const string Hit = "H";
        private const string Missed = "M";
        private const string Battleship = "B";
        private const string PlaceHolder = "-";

        public static Cell[][] CreateDefaultBoard()
        {
            return new Cell[][]
            {
                new []{ new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-") , new Cell("-") },
                new []{ new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-") , new Cell("-") },
                new []{ new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-") , new Cell("-") },
                new []{ new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-") , new Cell("-") },
                new []{ new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-") , new Cell("-") },
                new []{ new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-") , new Cell("-") },
                new []{ new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-") , new Cell("-") },
                new []{ new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-") , new Cell("-") },
                new []{ new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-") , new Cell("-") },
                new []{ new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-"), new Cell("-") , new Cell("-") }
            };
        }

        public static string AddBattleship(Cell[][] board, int row, int column, int shipSize, string alignment)
        {
            if (row <= 0 || row > 10)
                return $"Not a valid row of {++row}, cannot be less than 1 or more than 10";

            if (column <= 0 || column > 10)
                return $"Not a valid column of {++column}, cannot be less than 1 or more than 10";

            column = --column;
            row = --row;

            // 1) Add to playerBattleship
            var startingPointValue = board[row][column].Value;
            if (startingPointValue == Battleship)
                return $"You have already have a battleship at position of row: {++row}, column: {++column}";

            if (string.Equals(alignment, Horizontal, StringComparison.CurrentCultureIgnoreCase))
            {
                var selectedRow = board[row];
                var shipEndPosition = column + shipSize;
                var endCell = selectedRow.ElementAtOrDefault(shipEndPosition - 1);
                if (endCell == null)
                    return $"Cannot create ship at position of row: {++row}, column: {++column} as it will exceed the length of the board";

                for (var j = column; j < shipEndPosition; j++)
                {
                    var battleShip = new Models.Battleship(Battleship) { Alignment = alignment, StartX = column, StartY = row, Length = shipSize };
                    selectedRow[j] = battleShip;
                }
            }

            if (string.Equals(alignment, Vertical, StringComparison.CurrentCultureIgnoreCase))
            {
                var shipEndPosition = row + shipSize;
                var endCell = board.ElementAtOrDefault(shipEndPosition - 1);
                if (endCell == null)
                    return $"Cannot create ship at position of row: {++row}, column: {++column} as it will exceed the length of the board";

                for (var i = row; i < shipEndPosition; i++)
                {
                    var battleShip = new Models.Battleship(Battleship) { Alignment = alignment, StartX = column, StartY = row, Length = shipSize };
                    board[i][column] = battleShip;
                }
            }

            return $"Battleship created starting at position of row: {++row}, column: {++column}";
        }

        public static string Attack(Cell[][] board, int row, int column)
        {
            if (row <= 0 || row > 10)
                return $"Not a valid row of {++row}, cannot be less than 1 or more than 10";

            if (column <= 0 || column > 10)
                return $"Not a valid column of {++column}, cannot be less than 1 or more than 10";

            column = --column;
            row = --row;

            var selectedRow = board[row];
            var cell = selectedRow[column];
            var hit = false;
            if (cell.Value == PlaceHolder)
            {
                cell.Value = Missed;
                board[row][column] = cell;
                return $"You have missed at position of row: {++row}, column: {++column}";
            }
            else if (cell is Models.Battleship battleship)
            {
                if (battleship.Value == Hit)
                    return $"You have already hit at position of row: {++row}, column: {++column}";

                // Update current cell
                battleship.Value = Hit;
                board[row][column] = battleship;

                var hitBattleshipsCount = 0;

                // TODO: If all battleship cells are destroyed, set value as D
                if (battleship.Alignment == Horizontal)
                {
                    for (var j = battleship.StartX; j < battleship.Length; j++)
                    {
                        var selectedBattleship = board[row][j];
                        if (selectedBattleship.Value == Hit)
                            ++hitBattleshipsCount;
                    }
                }
                else if (battleship.Alignment == Vertical)
                {
                    for (var i = battleship.StartY; i < battleship.Length; i++)
                    {
                        var selectedBattleship = board[i][column];
                        if (selectedBattleship.Value == Hit)
                            ++hitBattleshipsCount;
                    }
                }

                var battleShipDestroyed = hitBattleshipsCount == battleship.Length;
                return $"You have {(battleShipDestroyed ? "destroyed a battleship starting" : "hit a battleship")} at position of row: {++row}, column: {++column}";
            }

            return $"You have {(hit ? "a hit" : "missed")} at row: {++row} and column: {++column}";
        }
    }
}