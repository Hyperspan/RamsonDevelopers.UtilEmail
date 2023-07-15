# EmailLib

EmailLib is a .NET class library that provides functionality to send emails using custom credentials. It utilizes the `EmailConfig` class to store and retrieve email configuration settings.

## Installation

To use EmailLib in your project, follow these steps:

1. Download the EmailLib source code or add it as a NuGet package to your solution.

2. Add a reference to the EmailLib project or the installed NuGet package in your target project.

3. Ensure that you have the required dependencies and references in your project to support email sending. You may need to install additional packages or configure your project accordingly.

## Usage

To send emails using EmailLib, follow these steps:

1. Instantiate an instance of the `EmailConfig` class.

   ```csharp
   EmailConfig emailConfig = new EmailConfig()
   {
       Server = "your-email-server",
       Port = 25,
       UserName = "your-username",
       Password = "your-password",
       UseSsl = true,
       TargetName = "your-target-name",
       FromName = "your-sender-name",
       FromAddress = "your-sender-address",
       ReplyToAddress = "your-reply-to-address"
   };
   ```

## Credits

EmailLib was developed by [Ayush Aher](https://www.linkedin.com/in/ayushaher118/) and is maintained by Ramson Developers. We would like to acknowledge the contributions of the open-source community and express our gratitude to all the contributors who helped make this project possible.


## Feedback
If you have any feedback, please reach out to us at developers.ramson@gmail.com