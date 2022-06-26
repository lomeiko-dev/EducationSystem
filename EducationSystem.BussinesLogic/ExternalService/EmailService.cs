
using EducationSystem.Helper.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace EducationSystem.BussinesLogic.ExternalService
{
    public class EmailService
    {
        private readonly OptionsEmailApp optionsEmailApp;
        private MimeMessage mimeMessage;

        public EmailService(IOptions<OptionsEmailApp> optionsEmailApp)
        {
            this.optionsEmailApp = optionsEmailApp.Value;
        }

        public void InitializeMime(string emailTo, string message)
        {
            mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress("Администрация", optionsEmailApp.Address));
            mimeMessage.To.Add(new MailboxAddress(string.Empty, emailTo));
            mimeMessage.Subject = optionsEmailApp.NameMessage;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html){ Text = message };
        }

        public async Task Send()
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(optionsEmailApp.Host, optionsEmailApp.Port, true);
                    await client.AuthenticateAsync(optionsEmailApp.Address, optionsEmailApp.Password);
                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
        }
    }
}
