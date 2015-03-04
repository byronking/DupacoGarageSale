using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class MessageReply
    {
        public int ReplyId { get; set; }
        public string ReplyFrom { get; set; }
        public DateTime ReplyDateTime { get; set; }
        public string ReplyText { get; set; }
        public int MessageId { get; set; }        
    }
}
