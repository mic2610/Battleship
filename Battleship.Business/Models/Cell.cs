namespace Battleship.Business.Models
{
    public class Cell
    {
        public Cell(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}