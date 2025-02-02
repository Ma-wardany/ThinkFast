using OnlineExam.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Service.Abstracts
{
    public interface IEmailServices
    {
        public Task<EmailServiceResult> SendEmailAsync(string email, string messag);
    }
}
