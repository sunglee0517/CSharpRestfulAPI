using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using company.Services; // 추가
using company.Models; // 추가 (Member 클래스가 정의된 곳)

namespace company.Controllers
{
    [Route("company/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly MemberService _memberService;

        public MemberController(MemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet("getMemberList")]
        public async Task<ActionResult<List<Member>>> GetAllMembers()
        {
            var members = await _memberService.GetAllMembersAsync();
            return Ok(members);
        }

        [HttpGet("getMember/{id}")]
        public async Task<ActionResult<Member>> GetMemberById(string id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        [HttpPost("join")]
        public async Task<ActionResult<Member>> JoinMember(Member member)
        {
            var insertedMember = await _memberService.InsertMemberAsync(member);
            return CreatedAtAction(nameof(GetMemberById), new { id = insertedMember.Id }, insertedMember);
        }

        [HttpPost("myInfoEdit")]
        public async Task<IActionResult> EditMyInfo(Member member)
        {
            var success = await _memberService.UpdateMemberAsync(member);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult<Member>> Login(Member member)
        {
            var existingMember = await _memberService.GetMemberByIdAsync(member.Id);
            if (existingMember == null || existingMember.Pw != member.Pw)
            {
                return NotFound("Invalid credentials");
            }
            return Ok(existingMember);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Implement logout logic here
            // For example, invalidate session or token

            return NoContent();
        }
    }
}
