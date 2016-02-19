using Surat.Common.Data;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.Mail
{
    public interface IMailManager
    {
        void Send(MailMessage message);
    }
}
