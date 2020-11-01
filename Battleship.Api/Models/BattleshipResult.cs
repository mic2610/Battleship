using Battleship.Business.Enums;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Battleship.Web.Models
{
    public class BattleshipResult
    {
        public List<string> PlayerBoard { get; set; }

        public List<string> OpponentBoard { get; set; }

        public string[] Results { get; set; }

        public int PlayerId { get; set; }

        public int OpponentId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BattleshipResultType ResultType { get; set; }
    }
}