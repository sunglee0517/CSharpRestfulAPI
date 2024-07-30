using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace company.Controllers
{
    [Route("company/[controller]")]
    [ApiController]
    public class QnaController : ControllerBase
    {
        private readonly QnaService _qnaService;

        public QnaController(QnaService qnaService)
        {
            _qnaService = qnaService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<Qna>>> GetAllQnas()
        {
            var qnas = await _qnaService.GetAllQnasAsync();
            return Ok(qnas);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Qna>> GetQnaById(int id)
        {
            var qna = await _qnaService.GetQnaByIdAsync(id);
            if (qna == null)
            {
                return NotFound();
            }
            return Ok(qna);
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Qna>> InsertQna(Qna qna)
        {
            var insertedQna = await _qnaService.InsertQnaAsync(qna);
            return CreatedAtAction(nameof(GetQnaById), new { id = insertedQna.Qno }, insertedQna);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateQna(Qna qna)
        {
            var success = await _qnaService.UpdateQnaAsync(qna);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteQna(int id)
        {
            var success = await _qnaService.DeleteQnaAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}