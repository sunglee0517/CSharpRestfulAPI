using Microsoft.AspNetCore.Mvc;
using company.Services; // 'YourNamespace'를 실제 네임스페이스로 변경하세요
using System.Threading.Tasks; // Task 타입을 사용하기 위해 추가

namespace company.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(string to, string subject, string body)
        {
            await _emailService.SendEmailAsync(to, subject, body);
            return Ok();
        }
    }
}