using ExcelWebAPI.Managers;
using ExcelWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Xml.Linq;

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
            sheetId = sheetId.Trim();

            if (sheetId.Any(Char.IsWhiteSpace))
            {
                return BadRequest();
            }

            sheetId = sheetId.ToLower();

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

        [HttpGet("/{sheetId}/{sellId}")]
        public async Task<IActionResult> Get([FromRoute] string sheetId, [FromRoute] string sellId)
        {
            sheetId = sheetId.Trim();
            sellId = sellId.Trim();

            if (sheetId.Any(Char.IsWhiteSpace) || sellId.Any(Char.IsWhiteSpace))
            {
                return BadRequest();
            }

            sheetId = sheetId.ToLower();
            sellId = sellId.ToLower();

            Cell? cell = await _manager.GetSheetCellAsync(sheetId, sellId);
            if (cell == null) 
            {
                return NotFound();
            }

            CellDTO cellDTO = new(cell.Value, await _manager.GetResult(sheetId, cell.Id, cell.Value));
            return Ok(cellDTO);
        }

        [HttpPost("/{sheetId}/{sellId}")]
        public async Task<IActionResult> Post([FromRoute] string sheetId, [FromRoute] string sellId, string value)
        {
            sheetId = sheetId.Trim();
            sellId = sellId.Trim();

            if (sheetId.Any(Char.IsWhiteSpace) || sellId.Any(Char.IsWhiteSpace)) 
            {
                return BadRequest();
            }

            sheetId = sheetId.ToLower();
            sellId =sellId.ToLower();

            Cell cell =  await _manager.SetSheetCellAsync(sheetId, sellId, value);
           
            CellDTO cellDTO = new(cell.Value, await _manager.GetResult(sheetId, cell.Id, cell.Value));
            if(cellDTO.Value=="Error")
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, cellDTO);
            }

            return StatusCode(StatusCodes.Status201Created, cellDTO);
        }
    }
}
