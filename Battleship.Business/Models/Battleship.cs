namespace Battleship.Business.Models
{
    public class Battleship : Cell
    {
        public Battleship() { }

        public Battleship(string value) : base(value)
        {
        }

        public int RowStart { get; set; }

        public int ColumnStart { get; set; }

        public int Length { get; set; }

        public string Alignment { get; set; }
    }
}