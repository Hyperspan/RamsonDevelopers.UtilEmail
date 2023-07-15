using Microsoft.Extensions.DependencyInjection;

namespace RamsonDevelopers.UtilEmail;

public static class ServiceExtension
{
    /// <summary>
    /// Used to include this project to other projects via dependency injections
    /// </summary>
    /// <param name="service"></param>
    /// <returns></returns>
    public static IServiceCollection AddEmailService(this IServiceCollection service)
    {
        service.AddSingleton<IEmailService, EmailService>();
        return service;
    }
}

