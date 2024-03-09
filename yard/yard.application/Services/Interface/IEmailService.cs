using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yard.application.Services.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(Message message);
    }
}
