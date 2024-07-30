using Microsoft.AspNetCore.Mvc;
using company.Services; // 'YourNamespace'�� ���� ���ӽ����̽��� �����ϼ���
using System.Threading.Tasks; // Task Ÿ���� ����ϱ� ���� �߰�

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