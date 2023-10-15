using System.Net.Mail;

namespace RamsonDevelopers.UtilEmail;

/// <summary>
/// DTO for Sending an Email
/// </summary>
public class SendEmailRequest
{
    /// <summary>
    /// Email address of the Sender <see cref="EmailAddress"/>
    /// </summary>
    public EmailAddress? FromAddress { get; set; }

    /// <summary>
    /// List of email addresses to send email to. <see cref="EmailAddress"/>
    /// </summary>
    public List<EmailAddress> ToAddresses { get; set; }

    /// <summary>
    /// List of email addresses to include in CC <see cref="EmailAddress"/>
    /// </summary>
    public List<EmailAddress> CcAddresses { get; set; }


    /// <summary>
    /// List of email addresses to include in BCC <see cref="EmailAddress"/>
    /// </summary>
    public List<EmailAddress> BccAddresses { get; set; }

    /// <summary>
    /// The subject of the Email
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Body of the email message
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// State weather the body is in HTML format
    /// </summary>
    public bool IsHtml { get; set; } = true;

    /// <summary>
    /// Set the Email's Priority. <see cref="EMailPriority"/>
    /// </summary>
    public EMailPriority Priority { get; set; } = EMailPriority.High;

    /// <summary>
    /// State Id
    /// </summary>
    public string EmailStateId { get; set; } = string.Empty;

    /// <summary>
    /// List of attachments to be shared with the email
    /// </summary>
    public List<string>? Attachments { get; set; }

    /// <summary>
    /// Linked resources for the attachments. <see cref="LinkedResource"/>
    /// </summary>
    public List<LinkedResource> LinkedResources { get; set; } = new();

    /// <summary>
    /// List of values for all the variables present in the HTML template or message body.
    /// <see cref="EmailTemplateVariables"/>
    /// </summary>
    public List<EmailTemplateVariables> Variables { get; set; }

    /// <summary>
    /// Default constructor
    /// <param name="variables">A List of all the template variables</param>
    /// </summary>
    public SendEmailRequest(params EmailTemplateVariables[] variables)
    {
        ToAddresses = new List<EmailAddress>();
        CcAddresses = new List<EmailAddress>();
        BccAddresses = new List<EmailAddress>();
        Variables = variables.ToList();
    }

    /// <summary>
    /// Absolute Path for HTML Template 
    /// </summary>
    public string? TemplatePath { get; set; }

}

/// <summary>
/// Object for providing values to the variables present in the message body or HTML Template.
/// </summary>
public class EmailTemplateVariables
{
    /// <summary>
    /// Name of the variable
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Value to replace the variable placeholder with
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Set Values of <see cref="Name"/> and <see cref="Value"/> property through constructor to reduce code.
    /// </summary>
    /// <param name="name">Name of the template variable</param>
    /// <param name="value">Value for the template variable</param>
    public EmailTemplateVariables(string name, string value)
    {
        Name = name;
        Value = value;

    }
}



/// <summary>
/// The template or format of the Email address
/// </summary>
public class EmailAddress
{
    private string _name = string.Empty;


    /// <summary>
    /// Name of the person whose email address is provided
    /// </summary>
    public string Name
    {
        get => string.IsNullOrEmpty(_name) ? Address : _name;
        set => _name = value;
    }

    /// <summary>
    /// Email address of the person
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Set the name and email address
    /// </summary>
    /// <param name="name">Name of the person</param>
    /// <param name="address">Email Address of that person</param>
    public EmailAddress(string name, string address)
    {
        Name = name;
        Address = address;
    }
}

/// <summary>
/// Enums for EmailPriority
/// </summary>
public enum EMailPriority
{
    /// <summary>
    /// set email priority to low
    /// </summary>
    Low,

    /// <summary>
    /// set email priority to Medium
    /// </summary>
    Medium,

    /// <summary>
    /// set email priority to HIGH
    /// </summary>
    High,
}