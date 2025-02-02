using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using OnlineExam.Domain.Helpers;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Service.Services
{
    public class EmailService : IEmailServices
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            this.emailSettings = emailSettings.Value;
        }

        public async Task<EmailServiceResult> SendEmailAsync(string email, string message)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(emailSettings.FromEmail, emailSettings.Password);

                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = $"{message}",
                        TextBody = "Welcome"
                    };

                    var Msg = new MimeMessage
                    {
                        Body = bodyBuilder.ToMessageBody()
                    };

                    Msg.From.Add(new MailboxAddress("School tech team", emailSettings.FromEmail));
                    Msg.To.Add(new MailboxAddress("user", email));
                    await client.SendAsync(Msg);
                    await client.DisconnectAsync(true);

                    return EmailServiceResult.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n+++++++++++++++++++++++++++++++++++++++++\n");
                Console.WriteLine(ex.InnerException!.ToString());

                return EmailServiceResult.FAILED;
            }
        }
    }
}
