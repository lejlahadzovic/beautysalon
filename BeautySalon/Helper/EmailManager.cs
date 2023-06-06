using System.Net.Mail;
using System.Net;

namespace BeautySalon.Helper
{
    public class EmailManager
    {
        public static void SendEmail(string Subject, string Body, string To)
        {
            if (To == "")
                return;

            String SendMailFrom = "neka.osobaa45@gmail.com";
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage email = new MailMessage();
            //START
            email.From = new MailAddress(SendMailFrom);
            email.To.Add(To);
            email.CC.Add(SendMailFrom);
            email.Subject = Subject;
            email.Body = Body;
            //END
            SmtpServer.Timeout = 5000;
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(SendMailFrom, "uyszrxxbciikinok");
            SmtpServer.Send(email);
        }
    }
}