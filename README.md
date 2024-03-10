----

# RamsonDevelopers.UtilEmail

RamsonDevelopers.UtilEmail is a .NET class library that provides functionality to send emails using custom credentials. It utilizes the `EmailConfig` class to store and retrieve email configuration settings.

----

## Installation

To use **RamsonDevelopers.UtilEmail** in your project, follow these steps:

1. Download the **RamsonDevelopers.UtilEmail** source code or add it as a NuGet package to your solution.

2. Add a reference to the **AddEmailService()** Method in **RamsonDevelopers.UtilEmail** project or the installed NuGet package in your target project's **program.cs**.

```csharp
    IConfiguration configuration = builder.Configuration;
    var emailConfig = configuration.GetSection(EmailConfig.SectionLabel);
    builder.Services.Configure<EmailConfig>(emailConfig);
    builder.Services.AddEmailService();
```

----

## Usage

To send emails using **RamsonDevelopers.UtilEmail**, follow these steps:

1. inside your `appsettings.json` add the following section with your actual credentials

   ```csharp
   "MailConfiguration": {
       "Server" : "your-email-server",
       "Port" : 25,
       "UserName" : "your-username",
       "Password" : "your-password",
       "UseSsl" : true,
       "TargetName" : "your-target-name",
       "FromName" : "your-sender-name",
       "FromAddress" : "your-sender-address",
       "ReplyToAddress" : "your-reply-to-address"
   }
   ```
2. Create an instance of [SendEmailRequest](https://github.com/Hyperspan/RamsonDevelopers.UtilEmail/blob/main/SendEmailRequest.cs) class and configure appropriate values

3. Create an instance of [IEmailService](https://github.com/Hyperspan/RamsonDevelopers.UtilEmail/blob/main/IEmailService.cs) through dependency injection.

4. Invoke ```SendEMailAsync``` Method inside the ```EmailService``` and pass the request object.

----
##  Credits

**RamsonDevelopers.UtilEmail** was developed by [Ayush Aher](https://ayush.ramson-developers.com) and is maintained by **Ramson Developers**. We would like to acknowledge the contributions of the open-source community and express our gratitude to all the contributors who helped make this project possible.

----

## Feedback

If you have any feedback, please reach out to us at [info@ramson-developers.com](mailto:info@ramson-developers.com)
or [Raise a Issue](https://github.com/Hyperspan/RamsonDevelopers.UtilEmail/issues) in [Github Repository](https://github.com/Hyperspan/RamsonDevelopers.UtilEmail)
