using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace RamsonDevelopers.UtilEmail
{
    /// <inheritdoc />
    public class EmailService : IEmailService
    {
        /// <inheritdoc />
        public EmailService(IOptions<EmailConfig> appConfig)
        {
            _mailConfig = appConfig.Value;
        }

        /// <inheritdoc />
        public EmailService(EmailConfig appConfig)
        {
            _mailConfig = appConfig;
        }

        private readonly EmailConfig _mailConfig;

        /// <summary>
        /// Send Html Template message
        /// </summary>
        /// <param name="mailRequest"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public async Task<MailMessage> SendTemplateMessage(SendEmailRequest mailRequest)
        {
            try
            {
                if (!File.Exists(mailRequest.TemplatePath))
                {
                    throw new FileNotFoundException("Template not found.", mailRequest.TemplatePath);
                }

                var template = File.ReadAllText(mailRequest.TemplatePath);

                template = mailRequest.Variables.Aggregate(template, (current, variable)
                    => current.Replace("{{" + variable.Name + "}}", variable.Value));

                mailRequest.Body = template;

                return await SendEMailAsync(mailRequest);
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
                throw;
            }
        }


        /// <summary>
        /// Send the Email to users as per the parameters passed
        /// </summary>
        /// <param name="request">
        ///<see cref="SendEmailRequest"/>
        /// </param>
        /// <returns></returns>
        public async Task<MailMessage> SendEMailAsync(SendEmailRequest request)
        {
            try
            {
                Log.Debug("Initialing EMail '" + request.EmailStateId + "'");
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

                await Task.Factory.StartNew(() => { smtpClient.SendAsync(mailMessage, request.EmailStateId); });

                Log.Debug("Sending EMail '" + request.EmailStateId + "' has been attempted");

                return mailMessage;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
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
            mailMessage.Body = request.Body;
            mailMessage.Headers.Add("Message-ID", eMailId.ToString());

            if (!string.IsNullOrEmpty(_mailConfig.ReplyToAddress))
                mailMessage.Headers.Add("Disposition-Notification-To", _mailConfig.ReplyToAddress);

            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

            if (!string.IsNullOrEmpty(_mailConfig.ReplyToAddress))
                mailMessage.ReplyToList.Add(new MailAddress(_mailConfig.ReplyToAddress));

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

            var alternateViewFromString =
                AlternateView.CreateAlternateViewFromString(mailMessage.Body, Encoding.UTF8, "text/html");
            foreach (var resource in request.LinkedResources)
            {
                alternateViewFromString.LinkedResources.Add(resource);
            }

            mailMessage.AlternateViews.Add(alternateViewFromString);

            mailMessage.IsBodyHtml = true;

            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.BodyEncoding = Encoding.UTF8;
        }

        /// <summary>
        /// To Check and update the email's Status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns>Returns the status of the Email</returns>
        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.UserState == null) return;
            var userState = (string)e.UserState;
            if (e.Cancelled)
                Log.Warning("[{0}] EMail Send canceled.", userState);
            if (e.Error != null)
                Log.Error("[{0}] Send EMail Failed : {1}", userState, e.Error.ToString());
            else
                Log.Information("[{0}] Message sent.", userState);
        }
    }
}
