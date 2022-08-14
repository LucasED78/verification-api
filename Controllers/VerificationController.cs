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
    private readonly IMessageService _messageService;
    private readonly IConfiguration _configuration;

    public VerificationController(IMessageService messageService, IConfiguration configuration)
    {
      _messageService = messageService;
      _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Post(string phone)
    {
      try
      {
        var result = await _messageService.SendAsync(new SendMessageOptions
        {
          From = _configuration["phone"],
          Body = "Hi, this is your verification code: X-X-X-X-X-X",
          To = phone
        });

        if (result.ErrorMessage != null)
        {
          return BadRequest(result.ErrorMessage);
        }

        return new OkObjectResult(new { Status = result.Status });
      } catch (Exception ex)
      {
        return new BadRequestObjectResult(ex.Message);
      }
    }
  }
}
