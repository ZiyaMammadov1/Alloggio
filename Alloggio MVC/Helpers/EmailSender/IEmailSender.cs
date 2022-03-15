using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Helpers.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync (string email, string subject, string message);
    }
}
