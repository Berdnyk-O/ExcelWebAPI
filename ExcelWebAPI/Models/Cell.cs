using System.Reflection.Metadata;

namespace ExcelWebAPI.Models
{
    public class Cell
    {
        public string Id { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string SheetId { get; set; } = null!;
        public Sheet Sheet { get; set; } = null!;
    }
}
