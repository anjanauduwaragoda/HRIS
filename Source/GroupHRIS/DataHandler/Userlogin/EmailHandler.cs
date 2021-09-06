using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using Common;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace DataHandler.Userlogin
{
    public class EmailHandler
    {
        public Boolean SendEmailtoAbsence(string sReceiverEmail, string sIsabsentDate, string sEmpname)
        {
            Boolean blSend = false;

            try
            {
                MailAddress mailAddressTo;
                MailAddress mailAddressFrom;
                string sSenderEmail = "eHRM@eapholdings.lk";
                string sSenderName = "HRIS Administrator";
                string sMailSubject = "HRIS Time Record Reminder - Requires Action";
                string sEmailBody = "";

                sEmailBody = sEmailBody + "<br/>";
                sEmailBody = sEmailBody + "<br/>";
                sEmailBody = sEmailBody + "Dear " + sEmpname;
                sEmailBody = sEmailBody + "<br/>";
                sEmailBody = sEmailBody + "<br/>";
                sEmailBody = "EAP HRIS System indicates that you are absent on " + sIsabsentDate + " <br/>";
                sEmailBody = sEmailBody + " and will assume that you may have forgotten to clock IN / Clock OUT. " + "<br/>";
                sEmailBody = sEmailBody + "please contact HR Department personal for more details. " + "<br/>";
                sEmailBody = sEmailBody + "<br/>";
                sEmailBody = sEmailBody + "<br/>";
                sEmailBody = sEmailBody + "<br/>";
                sEmailBody = sEmailBody + "<br/>";
                sEmailBody = sEmailBody + "*** This is an automatically generated email, please do not reply. ***";
                sEmailBody = sEmailBody + "<br/>";

                mailAddressFrom = new MailAddress(sSenderEmail, sSenderName);
                mailAddressTo = new MailAddress(sReceiverEmail);

                MailMessage mailMessage = new MailMessage(mailAddressFrom, mailAddressTo);

                mailMessage.Subject = sMailSubject;
                mailMessage.Body = sEmailBody;

                SmtpClient smtpClient = new SmtpClient("10.100.101.28", 25);

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "eHRM@eapholdings.lk",
                    Password = "123#456"
                };

                smtpClient.EnableSsl = true;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
                blSend = false;
                throw;
            }

            return blSend;
        }

        public Boolean SendEmailresetPassword(string sSenderName, string sReceiverEmail, string sMailSubject, string sEmailBody)
        {
            Boolean blSend = false;

            try
            {
                MailAddress mailAddressTo;
                MailAddress mailAddressFrom;
                string sSenderEmail = CommonVariables.COMMON_MAIL_ADDRESS;

                mailAddressFrom = new MailAddress(sSenderEmail, sSenderName);
                mailAddressTo = new MailAddress(sReceiverEmail);

                MailMessage mailMessage = new MailMessage(mailAddressFrom, mailAddressTo);

                mailMessage.Subject = sMailSubject;
                mailMessage.Body = sEmailBody;

                SmtpClient smtpClient = new SmtpClient("10.100.101.28", 25);

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "eHRM@eapholdings.lk",
                    Password = "123#456"
                };

                smtpClient.EnableSsl = true;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
                blSend = false;
                throw;
            }

            return blSend;
        }

        public Boolean SendRegisterEmail(string sSenderName, string sReceiverEmail, string sMailSubject, string sEmailBody)
        {
            Boolean blSend = false;

            try
            {
                MailAddress mailAddressTo;
                MailAddress mailAddressFrom;
                string sSenderEmail = CommonVariables.COMMON_MAIL_ADDRESS;

                mailAddressFrom = new MailAddress(sSenderEmail, sSenderName);
                mailAddressTo = new MailAddress(sReceiverEmail);

                MailMessage mailMessage = new MailMessage(mailAddressFrom, mailAddressTo);

                mailMessage.Subject = sMailSubject;
                mailMessage.Body = sEmailBody;

                SmtpClient smtpClient = new SmtpClient("10.100.101.28", 25);

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "ehrm@eapholdings.lk",
                    Password = "123#456"
                };

                smtpClient.EnableSsl = true;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return blSend;
        }


        public Boolean IsValidEmailAddress(string sEmail)
        {
            if (string.IsNullOrEmpty(sEmail))
                return false;
            else
            {
                Boolean regex = Regex.IsMatch(sEmail, @"\A(?:[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?)\Z");
                if (!regex)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        // Anjana Uduwragoda
        public static void SendDefaultEmail(string sSenderName, string sReceiver, string sCCAddress, string sMailSubject, StringBuilder sEmailBody)
        {
            try
            {
                MailAddress mailAddressTo;
                MailAddress mailAddressFrom;
                MailAddress mailAddressCC;

                mailAddressFrom = new MailAddress("ehrm@eapholdings.lk", sSenderName);
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
                    UserName = "ehrm@eapholdings.lk",
                    Password = "123#456"
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


        public static void SendDefaultEmailWithHTML(string sSenderName, string sReceiver, string sCCAddress, string sMailSubject, StringBuilder sEmailBody)
        {
            try
            {
                MailAddress mailAddressTo;
                MailAddress mailAddressFrom;
                MailAddress mailAddressCC;

                mailAddressFrom = new MailAddress("ehrm@eapholdings.lk", sSenderName);
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
                mailMessage.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("10.100.101.28", 25);

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "ehrm@eapholdings.lk",
                    Password = "123#456"
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

        // Anjana Uduwragoda
        public static void SendDefaultEmailHtml(string sSenderName, string sReceiver, string sCCAddress, string sMailSubject, StringBuilder sEmailBody)
        {
            try
            {
                MailAddress mailAddressTo;
                MailAddress mailAddressFrom;
                MailAddress mailAddressCC;

                mailAddressFrom = new MailAddress("ehrm@eapholdings.lk", sSenderName);
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
                mailMessage.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("10.100.101.28", 25);

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "ehrm@eapholdings.lk",
                    Password = "123#456"
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

        /*
         * To Send String e-mail Body with Data Grid YASINTHA
         * 
         */
         
        public static void SendHTMLMail(string sSenderName, string sReceiver, string sMailSubject, string sEmailBody)
        {
            try
            {
                MailAddress mailAddressTo;
                MailAddress mailAddressFrom;

                mailAddressFrom = new MailAddress("ehrm@eapholdings.lk", sSenderName);
                mailAddressTo = new MailAddress(sReceiver);

                MailMessage mailMessage = new MailMessage(mailAddressFrom, mailAddressTo);

                mailMessage.Subject = sMailSubject;
                mailMessage.Body = sEmailBody.ToString();
                mailMessage.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("10.100.101.28", 25);

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "ehrm@eapholdings.lk",
                    Password = "123#456"
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
