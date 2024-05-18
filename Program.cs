using Azure.Communication.Email;
using EmailProvider.Service;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<EmailClient>(new EmailClient(Environment.GetEnvironmentVariable("CommunicationServices")));
        services.AddSingleton<IEmailService, EmailService>();
    })
    .Build();

host.Run();
