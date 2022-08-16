using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneVerification.Exceptions;
using PhoneVerification.Models;
using PhoneVerification.Services.Interfaces;
using System.Diagnostics;

namespace PhoneVerification.Controllers
{
  [Route("api/verification/phone")]
  [ApiController]
  public class PhoneVerificationController : ControllerBase
  {
    private readonly IMessageService<Verification, SendMessageOptions> _messageService;
    private readonly IConfiguration _configuration;
    private readonly ICodeVerificationService _codeVerificationService;

    public PhoneVerificationController(
      IEnumerable<IMessageService<Verification, SendMessageOptions>> messageServices,
      IConfiguration configuration,
      ICodeVerificationService codeVerificationService
    )
    {
      _messageService = messageServices.First(service => service.GetType().Name.Contains("SMS"));
      _configuration = configuration;
      _codeVerificationService = codeVerificationService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string identifier)
    {
      var result = await _messageService.GetAsync(identifier);

      if (result != null)
      {
        return new OkObjectResult(new { Verified = result.IsVerified, Code = result.Code });
      }

      return BadRequest("Phone not found");
    }

    [HttpPost]
    public async Task<IActionResult> Post(string identifier)
    {
      try
      {
        var result = await _messageService.SendAsync(new SendMessageOptions
        {
          From = _configuration["Phone"],
          Body = "Hi, this is your verification code: {{code}}",
          To = identifier
        });

        if (result.ErrorMessage != null)
        {
          return BadRequest(result.ErrorMessage);
        }

        return new OkObjectResult(new { Code = result.Code });
      } catch (AlreadyVerifiedException ex)
      {
        return new ConflictObjectResult(new { Error = ex.Message });
      } catch (Exception ex)
      {
        return new BadRequestObjectResult(ex.Message);
      }
    }

    [HttpPut]
    public async Task<IActionResult> Verify(string identifier, string code)
    {
      try
      {
        var existant = await _messageService.GetAsync(identifier);

        if (existant == null)
        {
          return BadRequest("Phone not found");
        }

        var result = await _codeVerificationService.VerifyAsync(new PhoneNumber(identifier).Value, code);;

        if (result)
        {
          return Ok();
        }

        return BadRequest();
      } catch (ResourceNotFoundException ex)
      {
        return new NotFoundObjectResult(new { Error = ex.Message });
      } catch (InvalidCodeException)
      {
        return new BadRequestObjectResult(new { Error = $"Code {code} is invalid" });
      } catch (AlreadyVerifiedException ex)
      {
        return new ConflictObjectResult(new { Error = ex.Message });
      } catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpPost("[action]")]
    public Task<IActionResult> Resend(string identifier) => Post(identifier);
  }
}
