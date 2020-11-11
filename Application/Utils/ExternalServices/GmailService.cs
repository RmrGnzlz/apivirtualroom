using System.Net.Mail;

namespace Utils.ExternalServices
{
    public class GmailService : IEmailService
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 25;
        public GmailService(string email, string password)
        {
            Email = email;
            Password = password;
        }
        public string EnviarCorreo(string to, string title, string body)
        {
            try
            {
                MailMessage mailMessage = new MailMessage(Email, to, title, body);
                mailMessage.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient(Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = Port;
                smtpClient.Credentials = new System.Net.NetworkCredential(Email, Password);
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();
                return "OK";
            }
            catch (System.Exception e)
            {
                return e.Message;
            }
        }
    }
}