using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SendGrid;

namespace MoBot
{
    public class SendEmail : IDisposable
    {
        // Create network credentials to access your SendGrid account.
        private string username ="azure_a7395febf4500dc1161f80a873436fdb@azure.com";
        private string pswd = "Sw4p-CertZ@";
        public const string systemEmail = "MoBot@Envisage.au";

        public void Dispose()
        {
            // just incase I learn that Sendgrid needs to be disposed of later
            //Dispose();
        }

        public async Task<bool> SendRequestMail(string description, string phone, string email)
        {
            var sendGridMessage = new SendGridMessage
            {
                From = new MailAddress(systemEmail),
                To = new[] { new MailAddress("nathan@envisagesoftware.biz") },
                Subject = "App development Quote request for Toby",
                Html = $"Requirements {description} <br/> Contact {email} {phone}",
            };

            return await Send(sendGridMessage);
        }

        private async Task<bool> Send(SendGridMessage msg)
        {
            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential(username, pswd);

            // Create an SMTP transport for sending email.
            //var transportSMTP = SMTP.GetInstance(credentials);
            var transportWeb = new Web(credentials);

            // Send the email.
            //transportSMTP.Deliver(msg);
            await transportWeb.DeliverAsync(msg);

            return true;
        }

    }
}