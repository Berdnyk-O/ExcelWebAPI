namespace ExcelWebAPI.Models
{
    public class Sheet
    {
        public string id { get; set; } = null!;
        public List<Cell> Cells { get; set; } = new();
    }
}
