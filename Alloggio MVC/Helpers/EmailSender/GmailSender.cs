using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Alloggio_MVC.Helpers.EmailSender
{
    public class GmailSender : IEmailSender
    {
        private string _host;
        private int _port;
        private bool _enabledSSl;
        private string _username;
        private string _password;
        public GmailSender(string host, int port, bool enabledSSl, string username, string password)
        {
            _host = host;
            _port = port;
            _enabledSSl = enabledSSl;
            _username = username;
            _password = password;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = _enabledSSl
            };

            return client.SendMailAsync(
                new MailMessage(_username, email, subject, message)
                {
                    IsBodyHtml = true
                }
            );
        }
    }
}
