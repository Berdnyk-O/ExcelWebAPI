using ExcelWebAPI.Managers;
using ExcelWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

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

        [HttpGet("/{sheetId}")]
        public async Task<IActionResult> Get([FromRoute] string sheetId)
        {
            Sheet? sheet = await _manager.GetSheetAsync(sheetId);
            if (sheet == null)
            {
                return NotFound();
            }
            SheetDTO sheetDTO = new();
            foreach (var cell in sheet.Cells)
            {
                sheetDTO.Cells.Add(cell.Id, new(cell.Value, await _manager.GetResult(sheetId, cell.Value)));
            }
            return Ok(sheetDTO);
        }

        [HttpGet("/{sheetId}/{sellId}")]
        public async Task<IActionResult> Get([FromRoute] string sheetId, [FromRoute] string sellId)
        {
            Cell? cell = await _manager.GetSheetCellAsync(sheetId, sellId);
            if (cell == null) 
            {
                return NotFound();
            }
            CellDTO cellDTO = new(cell.Value, await _manager.GetResult(sheetId, cell.Value));
            return Ok(cellDTO);
        }

        [HttpPost("/{sheetId}/{sellId}")]
        public async Task<IActionResult> Post([FromRoute] string sheetId, [FromRoute] string sellId, string value)
        {
            Cell cell =  await _manager.SetSheetCellAsync(sheetId, sellId, value);
            CellDTO cellDTO = new(cell.Value, await _manager.GetResult(sheetId, cell.Value));
            if(cellDTO.Value=="Error")
            {
                return StatusCode(StatusCodes.Status201Created, cellDTO);
            }
            return StatusCode(StatusCodes.Status422UnprocessableEntity, cellDTO);

        }
    }
}
