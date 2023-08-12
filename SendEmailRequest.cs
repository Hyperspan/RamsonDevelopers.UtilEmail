using System.Net.Http.Headers;
using System.Net.Mail;

namespace RamsonDevelopers.UtilEmail;

/// <summary>
/// DTO for Sending an Email
/// </summary>
public class SendEmailRequest
{
    public EMailAddress? FromAddress { get; set; }

    public List<EMailAddress> ToAddresses { get; set; }

    public List<EMailAddress> CcAddresses { get; set; }

    public List<EMailAddress> BccAddresses { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public bool IsHtml { get; set; } = true;

    public EMailPriority Priority { get; set; } = EMailPriority.High;

    public string EMailStateID { get; set; } = string.Empty;

    public List<string> Attachments { get; set; }

    public AlternateViewCollection AlternateViews { get; set; }

    public List<EmailTemplateVariables> Variables { get; set; } = new();

    public string Template { get; set; }
    public bool UseTemplate { get; set; }

    public SendEmailRequest()
    {
        ToAddresses = new List<EMailAddress>();
        CcAddresses = new List<EMailAddress>();
        BccAddresses = new List<EMailAddress>();
    }
}



public class EmailTemplateVariables
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}



/// <summary>
/// The template or format of the Email address
/// </summary>
public class EMailAddress
{
    private string _name;

    public string Name
    {
        get => string.IsNullOrEmpty(_name) ? Address : _name;
        set => _name = value;
    }

    public string Address { get; set; }

    public EMailAddress(string name, string address)
    {
        _name = name;
        Address = address;
    }
}

/// <summary>
/// Enums for EmailPriority
/// </summary>
public enum EMailPriority
{
    Low,
    Medium,
    High,
}