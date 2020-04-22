using System;
using System.Collections.Generic;
using System.Linq;
using Battleship.Business.Constants;
using Battleship.Business.Models;
using Battleship.Business.Utilities;

namespace Battleship.App
{
    class Program
    {
        //private string[][] _playerBoard;

        //private string[][] _opponentBoard;

        static void Main(string[] args)
        {
            /*
            BattleshipUtility = new BattleshipUtility();
            var playerBoard = BattleshipUtility.CreateDefaultBoard();

            PrintBoard(playerBoard);
            Console.WriteLine("Player Board created");
            Console.ReadLine();

            var battleship1Added = BattleshipUtility.AddBattleship(playerBoard, 1, 1, 3, Business.Constants.BattleShip.Horizontal);
            PrintBoard(playerBoard);
            Console.WriteLine(battleship1Added);
            Console.ReadLine();

            var battleship1Attacked1 = BattleshipUtility.Attack(playerBoard, 1, 1);
            PrintBoard(playerBoard);
            Console.WriteLine(battleship1Attacked1);
            Console.ReadLine();

            var battleship1Attacked2 = BattleshipUtility.Attack(playerBoard, 1, 2);
            PrintBoard(playerBoard);
            Console.WriteLine(battleship1Attacked2);
            Console.ReadLine();

            // Returns that the battleship was destroyed
            var battleship1Attacked3 = BattleshipUtility.Attack(playerBoard, 1, 3);
            PrintBoard(playerBoard);
            Console.WriteLine(battleship1Attacked3);
            Console.ReadLine();

            var battleship2Added = BattleshipUtility.AddBattleship(playerBoard, 3, 3, 4, Business.Constants.BattleShip.Vertical);
            PrintBoard(playerBoard);
            Console.WriteLine(battleship2Added);
            Console.ReadLine();

            var battleship2AddedAgain = BattleshipUtility.AddBattleship(playerBoard, 4, 3, 4, Business.Constants.BattleShip.Vertical);
            PrintBoard(playerBoard);
            Console.WriteLine(battleship2AddedAgain);
            Console.ReadLine();

            var battleship2hit = BattleshipUtility.Attack(playerBoard, 3, 3);
            PrintBoard(playerBoard);
            Console.WriteLine(battleship2hit);
            Console.ReadLine();

            var battleship2hitAgain = BattleshipUtility.Attack(playerBoard, 3, 3);
            PrintBoard(playerBoard);
            Console.WriteLine(battleship2hitAgain);
            Console.ReadLine();

            var battleshipMissed = BattleshipUtility.Attack(playerBoard, 8, 8);
            PrintBoard(playerBoard);
            Console.WriteLine(battleshipMissed);
            Console.ReadLine();

            Console.Read();
            */
        }

        private static void PrintBoard(Cell[][] playerBoard)
        {
            // Display board
            for (var row = 0; row < playerBoard.Length; row++)
            {
                var output = string.Empty;
                var innerArray = playerBoard[row];
                for (var col = 0; col < innerArray.Length; col++)
                {
                    var value = innerArray[col].Value;

                    // TODO: Use string builder
                    output += $"{value} ";
                }

                Console.WriteLine(output);
            }
        }

        // Create board based off of a jagged array, just a little bit easier to index values from instead of a 2d array

        public string Reset()
        {
            //_playerBoard = null;
            //_opponentBoard = null;
            return "Reset complete";
        }
    }
}
