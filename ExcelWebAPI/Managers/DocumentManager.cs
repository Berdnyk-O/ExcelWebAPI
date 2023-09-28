using ExcelWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelWebAPI.Managers
{
    public class DocumentManager
    {
        private readonly ExcelWebApiContext _context;

        public DocumentManager(ExcelWebApiContext context)
        {
            _context = context;
        }

        public async Task<Sheet> CreateSheetAsync(string sheetId)
        {
            Sheet? newSheet = new()
            {
                Id = sheetId,
                Cells = new()
            };
            await _context.Sheets.AddAsync(newSheet);
            return newSheet;
        }

        public async Task<Cell> CreateCellAsync(string sheetId, string cellId)
        {
            Cell? newCell = new()
            {
                Id = sheetId
            };
            await _context.Cells.AddAsync(newCell);
            return newCell;
        }

        public async Task SetSheetCell(string sheetId, string cellId, string value)
        {
            Sheet? sheet = await _context.Sheets.FirstOrDefaultAsync(x => x.Id == sheetId);
            sheet ??= await CreateSheetAsync(sheetId);

            Cell? cell = await _context.Cells.FirstOrDefaultAsync(x => x.Id == cellId);
            cell ??= await CreateCellAsync(sheetId, cellId);

            cell.Value = value;

            await _context.SaveChangesAsync();
        }

        public Sheet? GetSheet(string sheetName)
        {
            return _context.Sheets.FirstOrDefault(x => x.Id == sheetName) ?? null;
        }

        public Cell? GetSheetCell(string sheetName, string cellName)
        {
            Sheet? sheet = GetSheet(sheetName);
            if (sheet == null)
            {
                return null;
            }
            return sheet.Cells.FirstOrDefault(x => x.Id == cellName) ?? null;
        }
    }
}
