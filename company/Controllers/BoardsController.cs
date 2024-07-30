using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace company.Controllers
{
    [Route("company/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly BoardService _boardService;

        public BoardsController(BoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<Board>>> GetAllBoards()
        {
            var boards = await _boardService.GetAllBoardsAsync();
            return Ok(boards);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Board>> GetBoardById(int id)
        {
            var board = await _boardService.GetBoardByIdAsync(id);
            if (board == null)
            {
                return NotFound();
            }
            return Ok(board);
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Board>> InsertBoard(Board board)
        {
            var insertedBoard = await _boardService.InsertBoardAsync(board);
            return CreatedAtAction(nameof(GetBoardById), new { id = insertedBoard.No }, insertedBoard);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateBoard(Board board)
        {
            var success = await _boardService.UpdateBoardAsync(board);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            var success = await _boardService.DeleteBoardAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}