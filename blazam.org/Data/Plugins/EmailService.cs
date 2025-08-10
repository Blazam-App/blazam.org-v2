using System.Text;
using MimeKit;

namespace blazam.org.Data.Plugins
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _senderEmail;
        private readonly bool _useSsl;

        public EmailService(IConfiguration configuration)
        {
            _smtpServer = configuration["EmailSettings:SmtpServer"]
                ?? throw new ArgumentNullException(nameof(configuration), "SMTP server configuration is missing");

            if (!int.TryParse(configuration["EmailSettings:SmtpPort"], out int port))
                throw new ArgumentException("Invalid SMTP port configuration");
            _smtpPort = port;

            _smtpUsername = configuration["EmailSettings:SmtpUsername"]
                ?? throw new ArgumentNullException(nameof(configuration), "SMTP username configuration is missing");
            _smtpPassword = configuration["EmailSettings:SmtpPassword"]
                ?? throw new ArgumentNullException(nameof(configuration), "SMTP password configuration is missing");
            _senderEmail = configuration["EmailSettings:SenderEmail"]
                ?? throw new ArgumentNullException(nameof(configuration), "Sender email configuration is missing");

            // Default to true if not specified
            _useSsl = configuration.GetValue<bool>("EmailSettings:UseSsl", true);
        }

        public async Task SendVerificationEmailAsync(string email, string token)
        {
            var verificationUrl = $"https://blazam.org/verify-email?token={token}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Blazam Plugins", _senderEmail));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Verify your Blazam Plugins account";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <html>
                    <body>
                        <h2>Welcome to Blazam Plugins!</h2>
                        <p>Thank you for registering. Please click the link below to verify your account:</p>
                        <p><a href='{verificationUrl}'>Verify my account</a></p>
                        <p>If you didn't create this account, you can safely ignore this email.</p>
                    </body>
                    </html>"
            };
            message.Body = bodyBuilder.ToMessageBody();

            // Explicitly specify MailKit.Net.Smtp.SmtpClient to avoid ambiguity
            using var client = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await client.ConnectAsync(_smtpServer, _smtpPort);

                if (!string.IsNullOrEmpty(_smtpUsername) && !string.IsNullOrEmpty(_smtpPassword))
                {
                    await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Failed to send email. Details:");
                sb.AppendLine($"SMTP Server: {_smtpServer}");
                sb.AppendLine($"Port: {_smtpPort}");
                sb.AppendLine($"SSL Enabled: {_useSsl}");
                sb.AppendLine($"Username: {_smtpUsername}");
                sb.AppendLine($"Error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    sb.AppendLine($"Inner error: {ex.InnerException.Message}");
                }

                throw new Exception(sb.ToString(), ex);
            }
        }
    }
}