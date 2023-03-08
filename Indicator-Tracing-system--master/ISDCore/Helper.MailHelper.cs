using System.Net;
using System.Net.Mail;
namespace ISDCore
{
    public class MailHelper
    {
    private static string ClientEmail = "isfm@idsc.net.eg";
    public static void sendEmail(string Email, string Body)
    {
      MailMessage mailMessage = new MailMessage();
      mailMessage.From = new MailAddress(ClientEmail);
      mailMessage.To.Add(new MailAddress(Email));
      mailMessage.Subject = "Account Information";
      mailMessage.Body = Body;
      SmtpClient smtpClient = new SmtpClient("163.121.6.2");
      smtpClient.Credentials = new System.Net.NetworkCredential("isfm", "IDSC@2020");
      smtpClient.Send(mailMessage);
    }
  }
}
