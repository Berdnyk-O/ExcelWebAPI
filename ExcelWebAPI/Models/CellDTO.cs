using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ExcelWebAPI.Models
{
    public class CellDTO
    {
        public CellDTO(string value)
        {
            Value = value;
        }
        public string Value { get; set; } = null!;
        public string Result { get; set; } = null!;
    }
}
