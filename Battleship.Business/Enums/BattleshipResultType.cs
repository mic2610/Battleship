using System.Text.Json.Serialization;

namespace Battleship.Business.Enums
{
    public enum BattleshipResultType
    {
        InvalidRow = 1,
        InvalidColumn = 2,
        AlreadyExists = 3,
        BoardOverflow = 4,
        Added = 5,
        AlreadyDestroyed = 6,
        AttackMissed = 7,
        AlreadyHit = 8,
        Hit = 9,
        Destroyed = 10,
        Missed = 11,
    }
}