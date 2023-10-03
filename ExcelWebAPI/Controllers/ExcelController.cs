using ExcelWebAPI.Managers;
using ExcelWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExcelWebAPI.Controllers
{
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IDocumentManager _manager;

        public ExcelController(IDocumentManager manager)
        {
            _manager = manager;
        }

        [HttpGet("api/v1/{sheetId}")]
        public async Task<IActionResult> Get([FromRoute] string sheetId)
        {
            sheetId = sheetId.Trim();

            sheetId = sheetId.ToLower();

            if (!IsIdValid(sheetId))
            {
                return BadRequest();
            }

            Sheet? sheet = await _manager.GetSheetAsync(sheetId);
            if (sheet == null)
            {
                return NotFound();
            }

            SheetDTO sheetDTO = new();
            foreach (var cell in sheet.Cells)
            {
                sheetDTO.Cells.Add(cell.Id, new(cell.Value, await _manager.GetResult(sheetId, cell.Id, cell.Value)));
            }

            return Ok(sheetDTO);
        }

        [HttpGet("api/v1/{sheetId}/{sellId}")]
        public async Task<IActionResult> Get([FromRoute] string sheetId, [FromRoute] string sellId)
        {
            sheetId = sheetId.Trim();
            sellId = sellId.Trim();

            sheetId = sheetId.ToLower();
            sellId = sellId.ToLower();

            if (!IsIdValid(sheetId) || !IsIdValid(sellId))
            {
                return BadRequest();
            }

            Cell? cell = await _manager.GetSheetCellAsync(sheetId, sellId);
            if (cell == null) 
            {
                return NotFound();
            }

            CellDTO cellDTO = new(cell.Value, await _manager.GetResult(sheetId, cell.Id, cell.Value));
            return Ok(cellDTO);
        }

        [HttpPost("api/v1/{sheetId}/{sellId}")]
        public async Task<IActionResult> Post([FromRoute] string sheetId, [FromRoute] string sellId, string value)
        {
            sheetId = sheetId.Trim();
            sellId = sellId.Trim();

            sheetId = sheetId.ToLower();
            sellId = sellId.ToLower();

            if (!IsIdValid(sheetId) || !IsIdValid(sellId)) 
            {
                return BadRequest();
            }
           
            CellDTO cellDTO = new(value, await _manager.GetResult(sheetId, sellId, value));
            if(cellDTO.Result=="ERROR")
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, cellDTO);
            }

            await _manager.SetSheetCellAsync(sheetId, sellId, value);
            return StatusCode(StatusCodes.Status201Created, cellDTO);
        }

        private bool IsIdValid(string id)
        {
            string specialChars = @" \|!#$%&/()=?»«@₴~{}.;'<>,^";
            foreach (var specialChar in specialChars)
            {
                if (id.Contains(specialChar)) return false;
            }
            return !double.TryParse(id, out double result);
        }
    }
}
