using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyMate.Models;

namespace EmailService
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task CreateAccountConfirmationEmail(AppUser user, string callbackUrl);
        Task CreatePasswordConfirmationEmail(AppUser user, string callbackUrl);

    }
}
