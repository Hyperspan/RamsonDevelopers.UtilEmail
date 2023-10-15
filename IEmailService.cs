using System.Net.Mail;
namespace RamsonDevelopers.UtilEmail;

/// <summary>
/// Interface for Dependency Injection
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send Email using the credentials passed.
    /// </summary>
    /// <param name="request">The request object of the Email Content</param>
    /// <returns>The main Mail message that was send using SMTP</returns>
    Task<MailMessage> SendEMailAsync(SendEmailRequest request);

    /// <summary>
    /// Send Html Template message
    /// </summary>
    /// <param name="mailRequest"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    Task<MailMessage> SendTemplateMessage(SendEmailRequest mailRequest);
}