using System;
using System.Text.Json.Serialization;
using Azure;
using Azure.Communication.Email;
using Azure.Messaging.ServiceBus;
using EmailProvider.Models;
using EmailProvider.Service;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EmailProvider.Functions;

public class EmailSender(ILogger<EmailSender> logger, IEmailService emailService)
{
    private readonly ILogger<EmailSender> _logger = logger;
    private readonly IEmailService _emailService = emailService;

    [Function(nameof(EmailSender))]
    public async Task Run([ServiceBusTrigger("email_request", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
    {
        try
        {
            var emailRequest = _emailService.UnpackEmailRequest(message);
            if (emailRequest != null && !string.IsNullOrEmpty(emailRequest.To))
            {
                if (_emailService.SendEmail(emailRequest))
                {
                    await messageActions.CompleteMessageAsync(message);
                }
            }
        }

        catch (Exception ex)
        {
            _logger.LogError($"ERROR : EmailSender.Run :: {ex.Message}");
        }
    }

    //public EmailRequest UnpackEmailRequest(ServiceBusReceivedMessage message)
    //{
    //    try
    //    {
    //        var emailRequest = JsonConvert.DeserializeObject<EmailRequest>(message.Body.ToString());
    //        if (emailRequest != null)
    //        {
    //            return emailRequest;
    //        }
    //    }

    //    catch (Exception ex)
    //    {
    //        _logger.LogError($"ERROR : EmailSender.UnpackEmailRequest() :: {ex.Message}");
    //    }

    //    return null!;
    //}

    //public bool SendEmail(EmailRequest emailRequest)
    //{
    //    try
    //    {
    //        var result =_emailClient.Send(
    //            WaitUntil.Completed,

    //            senderAddress: "DoNotReply@1ba5ed87-30aa-4c41-bd9a-1847ced55501.azurecomm.net",
    //            recipientAddress: "dude.silicon@gmail.com",
    //            subject: "Test Email",
    //            htmlContent: "<html><h1>Hello world via email.</h1l></html>",
    //            plainTextContent: "Hello world via email.");

    //        if (result.HasCompleted)
    //            return true;
    //    }

    //    catch (Exception ex)
    //    {
    //        _logger.LogError($"ERROR : EmailSender.SendEmailAsync() :: {ex.Message}");
    //    }

    //    return false;
    //}
}
