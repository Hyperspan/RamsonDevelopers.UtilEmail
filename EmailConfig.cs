namespace RamsonDevelopers.UtilEmail;

/// <summary>
/// Configuration Class for email services
/// </summary>
public class EmailConfig
{
    /// <summary>
    /// Default label of the section in appSettings.json where the configuration settings lie.
    /// </summary>
    public static string SectionLabel { get; set; } = "MailConfiguration";

    /// <summary>
    /// Name of the SMTP Server the credentials belong to.
    /// </summary>
    public string Server { get; set; } = string.Empty;

    /// <summary>
    /// Port which the Server uses For SMTP
    /// </summary>
    public int Port { get; set; } = 25;

    /// <summary>
    /// Username of the sender 
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Password to send emails on behalf of the Sender
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// To use SSL or TCP
    /// </summary>
    public bool UseSsl { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    public string TargetName { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string FromName { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string FromAddress { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string ReplyToAddress { get; set; } = string.Empty;

}