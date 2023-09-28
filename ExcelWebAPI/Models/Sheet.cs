namespace ExcelWebAPI.Models
{
    public class Sheet
    {
        public string Id { get; set; } = null!;
        public List<Cell> Cells { get; set; } = new();
    }
}
