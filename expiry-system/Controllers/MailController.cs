using Application.DTOs.Mail;
using Application.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace productExpiry_system.Controllers
{
    
        [Route("api/[controller]")]
        [ApiController]
        public class MailController : ControllerBase
        {
            private readonly IMailService mailService;
            public MailController(IMailService mailService)
            {
                this.mailService = mailService;
            }
            [HttpPost("send")]
            public async Task<IActionResult> SendMail([FromForm] MailRequestDTO request)
            {
                try
                {
                    await mailService.SendEmailAsync(request);
                    return Ok();
                }
                catch (System.Exception ex)
                {
                    throw;
                }

            }
        }
    
}
