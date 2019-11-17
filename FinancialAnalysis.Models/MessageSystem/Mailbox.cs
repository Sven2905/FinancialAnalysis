using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.MessageSystem
{
        [JsonObject(MemberSerialization.OptOut)]
    public class Mailbox : BaseClass
    {
        public int MailboxId { get; set; }
        public int RefUserId { get; set; }
        public MailboxType MailboxType { get; set; }
    }
}
