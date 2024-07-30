using System.Collections.Generic;
using System.Threading.Tasks;

public class BoardService
{
	private readonly BoardDAO _boardDAO;

	public BoardService(BoardDAO boardDAO)
	{
		_boardDAO = boardDAO;
	}

	public async Task<List<Board>> GetAllBoardsAsync()
	{
		return await _boardDAO.GetAllBoardsAsync();
	}

	public async Task<Board> GetBoardByIdAsync(int id)
	{
		return await _boardDAO.GetBoardByIdAsync(id);
	}

	public async Task<Board> InsertBoardAsync(Board board)
	{
		return await _boardDAO.InsertBoardAsync(board);
	}

	public async Task<bool> UpdateBoardAsync(Board board)
	{
		return await _boardDAO.UpdateBoardAsync(board);
	}

	public async Task<bool> DeleteBoardAsync(int id)
	{
		return await _boardDAO.DeleteBoardAsync(id);
	}
}