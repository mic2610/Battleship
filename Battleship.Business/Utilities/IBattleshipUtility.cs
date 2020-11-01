using Battleship.Business.Models;

namespace Battleship.Business.Utilities
{
    public interface IBattleshipUtility
    {
        Cell[][] CreateDefaultBoard();

        BattleshipUtilityResult AddBattleship(Cell[][] board, int row, int column, int shipSize, string alignment);

        BattleshipUtilityResult Attack(Cell[][] board, int row, int column);
    }
}