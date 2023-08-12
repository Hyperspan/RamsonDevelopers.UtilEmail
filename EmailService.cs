using System.ComponentModel;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RamsonDevelopers.UtilEmail;

internal class EmailService : IEmailService
{
    public EmailService(
        ILogger<EmailService> logger,
        IOptions<EmailConfig> appConfig)
    {
        _mailConfig = appConfig.Value;
        _logger = logger;
    }

    private EmailConfig _mailConfig { get; }
    private ILogger<EmailService> _logger { get; }

    /// <summary>
    /// Send the Email to users as per the parameters passed
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<MailMessage> SendEMailAsync(SendEmailRequest request)
    {
        try
        {
            _logger.LogDebug("Initialing EMail '" + request.EMailStateID + "'");
            var mailMessage = new MailMessage();
            var eMailId = Guid.NewGuid();

            GenerateEMail(request, eMailId, ref mailMessage);

            var smtpClient = new SmtpClient(_mailConfig.Server, _mailConfig.Port);
            smtpClient.EnableSsl = _mailConfig.UseSsl;

            if (string.IsNullOrEmpty(_mailConfig.UserName))
            {
                smtpClient.UseDefaultCredentials = true;
            }
            else
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_mailConfig.UserName, _mailConfig.Password);
            }

            if (!string.IsNullOrEmpty(_mailConfig.TargetName)) smtpClient.TargetName = _mailConfig.TargetName;

            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Timeout = 600000;
            smtpClient.SendCompleted += SendCompletedCallback;

            await Task.Factory.StartNew(() =>
            {
                smtpClient.SendAsync(mailMessage, request.EMailStateID);
            });

            _logger.LogDebug("Sending EMail '" + request.EMailStateID + "' has been attempted");

            return mailMessage;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw ex;
        }
    }

    /// <summary>
    /// Generate the structure and the main body of the Email
    /// </summary>
    /// <param name="request"></param>
    /// <param name="eMailId"></param>
    /// <param name="mailMessage"></param>
    private void GenerateEMail(SendEmailRequest request, Guid eMailId, ref MailMessage mailMessage)
    {
        mailMessage.Headers.Add("Message-ID", eMailId.ToString());

        if (!string.IsNullOrEmpty(_mailConfig.ReplyToAddress))
            mailMessage.Headers.Add("Disposition-Notification-To", _mailConfig.ReplyToAddress);

        mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

        if (!string.IsNullOrEmpty(_mailConfig.ReplyToAddress))
            mailMessage.ReplyTo = new MailAddress(_mailConfig.ReplyToAddress);

        mailMessage.From = new MailAddress(request.FromAddress?.Address ??
                                           _mailConfig.FromAddress, request.FromAddress?.Name ??
                                                                    _mailConfig.FromName, Encoding.UTF8);

        // Configure the priority of the Email
        var mailMessage1 = mailMessage;
        MailPriority mailPriority;
        switch (request.Priority)
        {
            case EMailPriority.Low:
                mailPriority = MailPriority.Low;
                break;
            case EMailPriority.Medium:
                mailPriority = MailPriority.Normal;
                break;
            case EMailPriority.High:
                mailPriority = MailPriority.High;
                break;
            default:
                mailPriority = MailPriority.Normal;
                break;
        }

        mailMessage1.Priority = mailPriority;
        // Add recipients to the email
        if (request.ToAddresses.Count > 0)
        {
            foreach (var toAddress in request.ToAddresses)
                mailMessage.To.Add(new MailAddress(toAddress.Address, toAddress.Name, Encoding.UTF8));
        }
        // Add CC to the email
        if (request.CcAddresses.Count > 0)
        {
            foreach (var ccAddress in request.CcAddresses)
                mailMessage.CC.Add(new MailAddress(ccAddress.Address, ccAddress.Name, Encoding.UTF8));
        }

        // Add BCC to the email
        if (request.BccAddresses.Count > 0)
        {
            foreach (var bccAddress in request.BccAddresses)
                mailMessage.Bcc.Add(new MailAddress(bccAddress.Address, bccAddress.Name, Encoding.UTF8));
        }


        // Set up Subject
        mailMessage.SubjectEncoding = Encoding.UTF8;
        mailMessage.Subject = request.Subject;

        // If there are any attachments attach them
        if (request.Attachments != null)
        {
            foreach (var attachment in request.Attachments)
                mailMessage.Attachments.Add(new Attachment(attachment));
        }

        // Set if the body is in html format
        mailMessage.IsBodyHtml = request.IsHtml;
        mailMessage.BodyEncoding = Encoding.UTF8;

        if (!request.UseTemplate)
        {
            var alternateViewFromString = AlternateView.CreateAlternateViewFromString(request.Body, Encoding.UTF8, "text/html");
            mailMessage.AlternateViews.Add(alternateViewFromString);
        }
        else
        {
            mailMessage.Body = request.Body;
            mailMessage.IsBodyHtml = true;
        }

        foreach (var altView in request.AlternateViews)
            mailMessage.AlternateViews.Add(altView);

        mailMessage.SubjectEncoding = Encoding.UTF8;
        mailMessage.BodyEncoding = Encoding.UTF8;


        foreach (var variable in request.Variables)
            request.Template += request.Template.Replace($"{{{variable.Name}}}", variable.Value);


    }

    /// <summary>
    /// To Check and update the email's Status
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>Returns the status of the Email</returns>
    private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        var userState = (string)e.UserState;
        if (e.Cancelled)
            _logger.LogWarning("[{0}] EMail Send canceled.", userState);
        if (e.Error != null)
            _logger.LogError("[{0}] Send EMail Failed : {1}", userState, e.Error.ToString());
        else
            _logger.LogInformation("[{0}] Message sent.", userState);
    }
}
