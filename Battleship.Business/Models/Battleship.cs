namespace Battleship.Business.Models
{
    public class Battleship : Cell
    {
        public Battleship(string value) : base(value)
        {
        }

        public int StartRow { get; set; }

        public int StartColumn { get; set; }

        public int Length { get; set; }

        public string Alignment { get; set; }
    }
}