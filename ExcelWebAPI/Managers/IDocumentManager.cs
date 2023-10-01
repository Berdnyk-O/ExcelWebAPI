using ExcelWebAPI.Models;

namespace ExcelWebAPI.Managers
{
    public interface IDocumentManager
    {
        Task<Cell> CreateCellAsync(string sheetId, string cellId);
        Task<Sheet> CreateSheetAsync(string sheetId);
        Task<Sheet?> GetSheetAsync(string sheetName);
        Task<Cell?> GetSheetCellAsync(string sheetName, string cellName);
        Task<Cell> SetSheetCellAsync(string sheetId, string cellId, string value);
        Task<string> GetResult(string sheetId, string cellId, string cellValue);
    }
}