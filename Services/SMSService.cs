﻿using PhoneVerification.Exceptions;
using PhoneVerification.Models;
using PhoneVerification.Repositories.Interfaces;
using PhoneVerification.Services.Interfaces;
using System.Diagnostics;
using System.Text.Json;
using Twilio.Rest.Api.V2010.Account;

namespace PhoneVerification.Services
{
  public class SMSService : ISmsService
  {
    private IVerificationRepository<string> _verificationRepository;

    public SMSService(IVerificationRepository<string> verificationRepository)
    {
      _verificationRepository = verificationRepository;
    }

    public SmsVerification? Get(string identifier)
    {
      var result = _verificationRepository.GetByIdentifier(identifier.Contains("+") ? identifier : $"+{identifier}");

      if (result != null)
      {
        var deserialized = JsonSerializer.Deserialize<SmsVerification>(result);

        return deserialized;
      }

      return null;
    }

    public async Task<SmsVerification?> GetAsync(string identifier)
    {
      var result = await _verificationRepository.GetByIdentifierAsync(identifier.Contains("+") ? identifier : $"+{identifier}");

      if (result != null)
      {
        var deserialized = JsonSerializer.Deserialize<SmsVerification>(result);

        return deserialized;
      }

      return null;
    }

    public SendMessageResponse Send(SendMessageOptions options)
    {
      SmsVerification verification;

      var hasExistantVerification = _verificationRepository.Exists(options.To);

      if (hasExistantVerification)
      {
        verification = Get(options.To)!;
        if (verification.IsVerified) throw new AlreadyVerifiedException("This resource is already verified");
      }
      else
      {
        verification = new SmsVerification
        {
          Identifier = options.To
        };

        _verificationRepository.Save(options.To, JsonSerializer.Serialize(verification));
      }

      var result = MessageResource.Create(
        body: options.Body.Replace("{{code}}", verification.Code),
        from: new Twilio.Types.PhoneNumber(options.From),
        to: new Twilio.Types.PhoneNumber(options.To)
      );

      return new SendMessageResponse
      {
        Code = verification.Code,
        ErrorMessage = result.ErrorMessage
      };
    }

    public async Task<SendMessageResponse> SendAsync(SendMessageOptions options)
    {
      SmsVerification verification;

      var hasExistantVerification = await _verificationRepository.ExistsAsync(options.To);

      if (hasExistantVerification)
      {
        verification = (await GetAsync(options.To))!;

        if (verification.IsVerified) throw new AlreadyVerifiedException("This resource is already verified");
      } else
      {
        verification = new SmsVerification
        {
          Identifier = options.To
        };

        await _verificationRepository.SaveAsync(options.To, JsonSerializer.Serialize(verification));
      }

      var result = await MessageResource.CreateAsync(
        body: options.Body.Replace("{{code}}", verification.Code),
        from: new Twilio.Types.PhoneNumber(options.From),
        to: new Twilio.Types.PhoneNumber(options.To)
      );

      return new SendMessageResponse
      {
        Code = verification.Code,
        ErrorMessage = result.ErrorMessage
      };
    }

    public bool Verify(string identifier, string code)
    {
      var result = Get(identifier);


      if (result != null)
      {
        if (result.IsVerified) throw new AlreadyVerifiedException("This resource is already verified");
        if (result.Code != code) throw new InvalidCodeException();

        result.IsVerified = true;

        _verificationRepository.Save(identifier, JsonSerializer.Serialize(result));

        return true;
      }

      return false;
    }

    public async Task<bool> VerifyAsync(string identifier, string code)
    {
      var result = await GetAsync(identifier);


      if (result != null)
      {
        if (result.IsVerified) throw new AlreadyVerifiedException("This resource is already verified");
        if (result.Code != code) throw new InvalidCodeException();

        result.IsVerified = true;

        await _verificationRepository.SaveAsync(identifier.Contains("+") ? identifier : $"+{identifier}", JsonSerializer.Serialize(result));

        return true;
      }

      return false;
    }
  }
}
