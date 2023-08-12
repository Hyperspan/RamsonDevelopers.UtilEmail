## Note
I am still a newbie in the open source world and would appreciate if any on would help making this library better. Just by creating a new issue you are doing a lot. 

**Regards Ayush Aher.**

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
----
## Credits

**RamsonDevelopers.UtilEmail** was developed by [Ayush Aher](https://www.linkedin.com/in/ayushaher118/) and is maintained by **Ramson Developers**. We would like to acknowledge the contributions of the open-source community and express our gratitude to all the contributors who helped make this project possible.

----

## Feedback

If you have any feedback, please reach out to us at [developers.ramson@gmail.com](mailto:developers.ramson@gmail.com) 
or [Raise a Issue](https://github.com/AyushAher/RamsonDevelopers.UtilEmail/issues) in [Github Repository](https://github.com/AyushAher/RamsonDevelopers.UtilEmail)
