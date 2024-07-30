using System.Collections.Generic;
using System.Threading.Tasks;

public class DataroomService
{
    private readonly DataroomDAO _dataroomDAO;

    public DataroomService(DataroomDAO dataroomDAO)
    {
        _dataroomDAO = dataroomDAO;
    }

    public async Task<List<Dataroom>> GetAllDataroomsAsync()
    {
        return await _dataroomDAO.GetAllDataroomsAsync();
    }

    public async Task<Dataroom> GetDataroomByIdAsync(int id)
    {
        return await _dataroomDAO.GetDataroomByIdAsync(id);
    }

    public async Task<Dataroom> InsertDataroomAsync(Dataroom dataroom)
    {
        return await _dataroomDAO.InsertDataroomAsync(dataroom);
    }

    public async Task<bool> UpdateDataroomAsync(Dataroom dataroom)
    {
        return await _dataroomDAO.UpdateDataroomAsync(dataroom);
    }

    public async Task<bool> DeleteDataroomAsync(int id)
    {
        return await _dataroomDAO.DeleteDataroomAsync(id);
    }
}