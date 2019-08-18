using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TestWebservice.Services
{
    public static class MailService
    {
        public static bool SendEmailWithCsvTo(
            string cvsFile, 
            string fromMail, 
            string fromName,
            string fromPassword,
            string toMail,
            string toName
            )
        {
            if (string.IsNullOrEmpty(cvsFile) ||
                string.IsNullOrEmpty(fromMail) ||
                string.IsNullOrEmpty(fromName) ||
                string.IsNullOrEmpty(fromPassword) ||
                string.IsNullOrEmpty(toMail) ||
                string.IsNullOrEmpty(toName))
                return false;
            
            var fromAddress = new MailAddress(fromMail, fromName);
            var toAddress = new MailAddress(toMail, toName);
            
            var mailMessage = CreateMailMessage(cvsFile, fromAddress, toAddress);

            var client = CreateSmtpClient(fromPassword, fromAddress);
            
            client.Send(mailMessage);

            return true;
        }

        private static MailMessage CreateMailMessage(string cvsFile, MailAddress fromAddress, MailAddress toAddress)
        {
            var currentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(
                DateTime.Now.Month);
            var currentYear = DateTime.Now.Year.ToString();
            var currentDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = $"Оплата задолженностей за период: ({currentMonth})",
                Body = "«Сгенерирован отчет оплаты задолженностей за период: " +
                       $"({currentMonth}, {currentYear})" +
                       $"\n({currentDate})»",
                BodyEncoding = Encoding.UTF8,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            };
            mailMessage.Attachments.Add(new Attachment(cvsFile));
            return mailMessage;
        }

        private static SmtpClient CreateSmtpClient(string fromPassword, MailAddress fromAddress)
        {
            var client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            return client;
        }

    }
}