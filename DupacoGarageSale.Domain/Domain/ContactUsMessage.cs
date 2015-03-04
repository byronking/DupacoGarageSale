using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class ContactUsMessage
    {
        public int MessageId { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageSentDateTime { get; set; }
        public List<MessageReply> MessageReplies { get; set; }
    }
}
