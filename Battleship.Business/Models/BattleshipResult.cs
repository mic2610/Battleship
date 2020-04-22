using System.Collections.Generic;

namespace Battleship.Business.Models
{
    public class BattleshipResult
    {
        public List<string> PlayerBoard { get; set; }

        public List<string> OpponentBoard { get; set; }

        public List<string> Results { get; set; }

        public string PlayerId { get; set; }

        public string OpponentId { get; set; }
    }
}