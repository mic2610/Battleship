namespace Battleship.Business.Models
{
    public class Battleship : Cell
    {
        public Battleship(string value) : base(value)
        {
        }

        public int StartX { get; set; }

        public int StartY { get; set; }

        public int Length { get; set; }

        public string Alignment { get; set; }
    }
}