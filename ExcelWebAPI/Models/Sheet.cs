namespace ExcelWebAPI.Models
{
    public class Sheet
    {
        public string Name { get; set; } = null!;
        public List<Cell> Cells { get; set; } = null!;
    }
}
