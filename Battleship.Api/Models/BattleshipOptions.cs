namespace Battleship.API.Models
{
    public class BattleshipOptions
    {
        public int Row { get; set; }

        public int Column { get; set; }
        
        public int? ShipSize { get; set; }

        public string Alignment { get; set; }

        public int PlayerId { get; set; }

        public int OpponentId { get; set; }
    }
}