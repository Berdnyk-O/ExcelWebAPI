using ExcelWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelWebAPI.Managers
{
    public class DocumentManager : IDocumentManager
    {
        private readonly ExcelWebApiContext _context;

        public DocumentManager(ExcelWebApiContext context)
        {
            _context = context;
        }

        public async Task<Sheet> CreateSheetAsync(string sheetId)
        {
            Sheet newSheet = new()
            {
                Id = sheetId,
                Cells = new()
            };
            await _context.Sheets.AddAsync(newSheet);
            return newSheet;
        }

        public async Task<Cell> CreateCellAsync(string sheetId, string cellId)
        {
            Cell newCell = new()
            {
                Id = cellId,
                SheetId = sheetId
            };
            await _context.Cells.AddAsync(newCell);
            return newCell;
        }

        public async Task<Cell> SetSheetCellAsync(string sheetId, string cellId, string value)
        {
            Sheet sheet = await _context.Sheets.FirstOrDefaultAsync(x => x.Id == sheetId)
                           ?? await CreateSheetAsync(sheetId);
                 
            Cell cell = await _context.Cells.FirstOrDefaultAsync(x => x.Id == cellId)
                         ?? await CreateCellAsync(sheetId, cellId);

            cell.Value = value;

            await _context.SaveChangesAsync();

            return cell;
        }

        public async Task<Sheet?> GetSheetAsync(string sheetName)
        {
            return await _context.Sheets.Include(x=>x.Cells).FirstOrDefaultAsync(x => x.Id == sheetName) ?? null;
        }

        public async Task<Cell?> GetSheetCellAsync(string sheetName, string cellName)
        {
            Sheet? sheet = await GetSheetAsync(sheetName);
            if (sheet == null)
            {
                return null;
            }
            return await _context.Cells.FirstOrDefaultAsync(x => x.Id == cellName) ?? null;
        }
    }
}
