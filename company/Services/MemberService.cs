using System.Collections.Generic;
using System.Threading.Tasks;

public class MemberService
{
    private readonly MemberDAO _memberDAO;

    public MemberService(MemberDAO memberDAO)
    {
        _memberDAO = memberDAO;
    }

    public async Task<List<Member>> GetAllMembersAsync()
    {
        return await _memberDAO.GetAllMembersAsync();
    }

    public async Task<Member> GetMemberByIdAsync(string id)
    {
        return await _memberDAO.GetMemberByIdAsync(id);
    }

    public async Task<Member> InsertMemberAsync(Member member)
    {
        // Additional business logic can be added here, such as password hashing

        return await _memberDAO.InsertMemberAsync(member);
    }

    public async Task<bool> UpdateMemberAsync(Member member)
    {
        // Additional business logic can be added here

        return await _memberDAO.UpdateMemberAsync(member);
    }

    public async Task<bool> DeleteMemberAsync(string id)
    {
        return await _memberDAO.DeleteMemberAsync(id);
    }
}