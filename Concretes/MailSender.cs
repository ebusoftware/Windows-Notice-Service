using SgkService.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SgkService.Concretes
{
    class MailSender : IMailService
    {

        public async Task SendMessage(string konu, string icerik, string aliciMail)
        {
            try
            {
                Duyurular duyurular = new Duyurular();

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["gondericiMail"], System.Configuration.ConfigurationManager.AppSettings["gondericiAdi"]);
                mail.To.Add(aliciMail.ToString());
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                mail.IsBodyHtml = true;
                //Mail konusu.
                mail.Subject = konu.ToString();
                //Mail açıklaması
                mail.Body = icerik.ToString();

                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["gondericiMail"], System.Configuration.ConfigurationManager.AppSettings["gondericiSifre"]);
                smtp.EnableSsl = true;
                smtp.Port =Convert.ToInt32( System.Configuration.ConfigurationManager.AppSettings["port"]);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Host = System.Configuration.ConfigurationManager.AppSettings["host"];

              await  smtp.SendMailAsync(mail);
               
            }
            catch (Exception)
            {
                Duyurular duyurular = new Duyurular();
                duyurular.Stop();
                duyurular.Start();
                //Console.WriteLine("Mail Gönderilemedi.");
                throw;
            }
        }
    }
}
