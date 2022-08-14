using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneVerification.Models;
using PhoneVerification.Services.Interfaces;

namespace PhoneVerification.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class VerificationController : ControllerBase
  {
    private readonly IMessageService<SmsVerification> _messageService;
    private readonly IConfiguration _configuration;

    public VerificationController(IMessageService<SmsVerification> messageService, IConfiguration configuration)
    {
      _messageService = messageService;
      _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string phone)
    {
      var result = await _messageService.GetAsync(phone);

      if (result != null)
      {
        return new OkObjectResult(new { Verified = result.IsVerified, Code = result.Code });
      }

      return BadRequest("Phone not found");
    }

    [HttpPost]
    public async Task<IActionResult> Post(string phone)
    {
      try
      {
        var result = await _messageService.SendAsync(new SendMessageOptions
        {
          From = _configuration["phone"],
          Body = "Hi, this is your verification code: {{code}}",
          To = phone
        });

        if (result.ErrorMessage != null)
        {
          return BadRequest(result.ErrorMessage);
        }

        return new OkObjectResult(new { Code = result.Code });
      } catch (Exception ex)
      {
        return new BadRequestObjectResult(ex.Message);
      }
    }
  }
}
