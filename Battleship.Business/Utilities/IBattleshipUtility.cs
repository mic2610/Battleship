using Battleship.Business.Models;

namespace Battleship.Business.Utilities
{
    public interface IBattleshipUtility
    {
        Cell[][] CreateDefaultBoard();
        string AddBattleship(Cell[][] board, int row, int column, int shipSize, string alignment);
        string Attack(Cell[][] board, int row, int column);
    }
}