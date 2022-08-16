# Verification API

A project using .NET, Twilio and Redis to create a sms and email verification flow

## Features

- Twilio integration to send SMS messages
- SendGrid integration to send emails
- Redis integration to store an control the verification flow

## How it works

There is a template in appsettings.json for the configuration needed in order to run the project.

```json
{
  "AccountSID": "",
  "AuthToken": "",
  "RedisConnection": "",
  "Phone": "",
  "SendGridApiKey": "",
  "Email": ""
}
```

## Installation

```sh
dotnet restore
```

## How to run

```sh
dotnet run
```

After this, just open your browser using one of the localhost urls (https://localhost:7051/swagger or https://localhost:5184/swagger) to see the available endpoints

## Tech

Dillinger uses a number of open source projects to work properly:

- [Twilio] - A platform that has a lot of integrations
- [SendGrid] - A platform (owned by Twili) that can be used to send emails
- [Redis] - An awesome in memory database, commonly used for cache
- [.NET Core] - Microsoft framework

[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)
   [Twilio]: <https://www.twilio.com/pt-br/sms>
   [SendGrid]: <https://sendgrid.com/go/email-brand-signup-sales-1>
   [.NET Core]: <https://docs.microsoft.com/pt-br/dotnet/core/introduction>
   [Redis]: <https://redis.io/>
