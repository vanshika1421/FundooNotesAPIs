using MessagingServiceBL.Interface;
using MessagingServiceModel;
using Microsoft.AspNetCore.Mvc;

namespace MessagingServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailBL _emailBL;

        public EmailController(IEmailBL emailBL)
        {
            _emailBL = emailBL;
        }

        [HttpPost("send")]
        public IActionResult SendEmail(EmailRequest request)
        {
            _emailBL.SendEmail(request);

            return Ok("Email sent successfully");
        }
    }
}