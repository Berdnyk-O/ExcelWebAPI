namespace ExcelWebAPI.Models
{
    public class SheetDTO
    {
        public Dictionary<string, CellDTO> Cells { get; set; } = new();
    }
}
