using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgkService.Abstracts
{
    public interface IMailService
    {
        Task SendMessage(string konu, string icerik,string aliciMail);
    }
}
