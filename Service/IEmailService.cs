using Azure.Messaging.ServiceBus;
using EmailProvider.Models;

namespace EmailProvider.Service;

public interface IEmailService
{
    bool SendEmail(EmailRequest emailRequest);
    EmailRequest UnpackEmailRequest(ServiceBusReceivedMessage message);
}