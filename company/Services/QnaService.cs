using System.Collections.Generic;
using System.Threading.Tasks;

public class QnaService
{
    private readonly QnaDAO _qnaDAO;

    public QnaService(QnaDAO qnaDAO)
    {
        _qnaDAO = qnaDAO;
    }

    public async Task<List<Qna>> GetAllQnasAsync()
    {
        return await _qnaDAO.GetAllQnasAsync();
    }

    public async Task<Qna> GetQnaByIdAsync(int id)
    {
        return await _qnaDAO.GetQnaByIdAsync(id);
    }

    public async Task<Qna> InsertQnaAsync(Qna qna)
    {
        return await _qnaDAO.InsertQnaAsync(qna);
    }

    public async Task<bool> UpdateQnaAsync(Qna qna)
    {
        return await _qnaDAO.UpdateQnaAsync(qna);
    }

    public async Task<bool> DeleteQnaAsync(int id)
    {
        return await _qnaDAO.DeleteQnaAsync(id);
    }
}