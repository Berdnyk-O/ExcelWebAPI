using ExcelWebAPI.Managers;
using ExcelWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExcelWebAPI.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ExcelController : ControllerBase
    {
        private readonly IDocumentManager _manager;

        public ExcelController(IDocumentManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string sheetId, string sellId)
        {
            Cell? cell = await _manager.GetSheetCellAsync(sheetId, sellId);
            if (cell == null) 
            {
                return NotFound();
            }
            CellDTO cellDTO = new(cell.Value);
            return Ok(cellDTO);
        }

        [HttpPost]
        [Route("/{sheetId}/{sellId}")]
        public async Task<IActionResult> Post(string sheetId, string sellId, string value)
        {
            Cell cell =  await _manager.SetSheetCellAsync(sheetId, sellId, value);
            CellDTO cellDTO = new(cell.Value);
            return Ok(cellDTO);
        }
    }
}
