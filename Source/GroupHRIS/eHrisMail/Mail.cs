using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Common;

namespace HrisMail
{
    public class Mail
    {
        public static void SendEmail(string sSender, string sPassword, string sReceiver, string sCCAddress, string sMailSubject, string sEmailBody)
        {
            try
            {
                MailAddress mailAddressTo;
                MailAddress mailAddressFrom;
                MailAddress mailAddressCC;

                mailAddressFrom = new MailAddress(sSender);
                mailAddressTo = new MailAddress(sReceiver);

                MailMessage mailMessage = new MailMessage(mailAddressFrom, mailAddressTo);



                if (sCCAddress.Trim() != "")
                {
                    mailAddressCC = new MailAddress(sCCAddress);
                    mailMessage.CC.Add(mailAddressCC);
                }

                mailMessage.Subject = sMailSubject;
                mailMessage.Body = sEmailBody;

                SmtpClient smtpClient = new SmtpClient("10.100.101.28", 25);

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = sSender,
                    Password = sPassword
                };

                smtpClient.EnableSsl = true;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }






        public static void SendDefaultEmail(string sSenderName, string sReceiver, string sCCAddress, string sMailSubject, StringBuilder sEmailBody)
        {
            try
            {
                MailAddress mailAddressTo;
                MailAddress mailAddressFrom;
                MailAddress mailAddressCC;

                mailAddressFrom = new MailAddress("testuser@eapholdings.lk", sSenderName);
                mailAddressTo = new MailAddress(sReceiver);

                MailMessage mailMessage = new MailMessage(mailAddressFrom, mailAddressTo);

                // if you want to get a reply and it is to a different address set it to the add() in following statement
                // mailMessage.ReplyToList.Add(mailAddressFrom);
                // reply will come to the mailaddress(es) in above list

                if ((sCCAddress.Trim() != "") && (sCCAddress.Trim() != null))
                {
                    mailAddressCC = new MailAddress(sCCAddress);
                    mailMessage.CC.Add(mailAddressCC);
                }

                mailMessage.Subject = sMailSubject;
                mailMessage.Body = sEmailBody.ToString();

                SmtpClient smtpClient = new SmtpClient("10.100.101.28", 25);

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "testuser@eapholdings.lk",
                    Password = "123456"
                };

                smtpClient.EnableSsl = true;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
