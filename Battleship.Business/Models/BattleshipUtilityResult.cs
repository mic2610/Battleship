using Battleship.Business.Enums;

namespace Battleship.Business.Models
{
    public class BattleshipUtilityResult
    {
        public BattleshipUtilityResult(string message, BattleshipResultType resultType)
        {
            Message = message;
            ResultType = resultType;
        }

        public string Message { get; set; }

        public BattleshipResultType ResultType { get; set; }
    }
}