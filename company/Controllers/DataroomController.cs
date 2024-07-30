using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace company.Controllers
{
    [Route("company/[controller]")]
    [ApiController]
    public class DataroomController : ControllerBase
    {
        private readonly DataroomService _dataroomService;

        public DataroomController(DataroomService dataroomService)
        {
            _dataroomService = dataroomService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<Dataroom>>> GetAllDatarooms()
        {
            var datarooms = await _dataroomService.GetAllDataroomsAsync();
            return Ok(datarooms);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Dataroom>> GetDataroomById(int id)
        {
            var dataroom = await _dataroomService.GetDataroomByIdAsync(id);
            if (dataroom == null)
            {
                return NotFound();
            }
            return Ok(dataroom);
        }

        [HttpPost("upload")]
        public async Task<ActionResult<Dataroom>> UploadDataroom(Dataroom dataroom)
        {
            var insertedDataroom = await _dataroomService.InsertDataroomAsync(dataroom);
            return CreatedAtAction(nameof(GetDataroomById), new { id = insertedDataroom.Dno }, insertedDataroom);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateDataroom(Dataroom dataroom)
        {
            var success = await _dataroomService.UpdateDataroomAsync(dataroom);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteDataroom(int id)
        {
            var success = await _dataroomService.DeleteDataroomAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}