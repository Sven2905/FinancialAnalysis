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
    public class Message : BindableBase
    {
        public int MessageId { get; set; }
        public string Text { get; set; }
        public int RefSenderId { get; set; }
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }

    }
}
