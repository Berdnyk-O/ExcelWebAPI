using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ExcelWebAPI.Models
{
    public class CellDTO
    {
        public CellDTO(string value, string result)
        {
            Value = value;
            Result = result;
        }
        public string Value { get; set; } = null!;
        public string Result { get; set; } = null!;
    }
}
