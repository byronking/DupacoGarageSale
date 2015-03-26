using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class EmailMessage
    {
        public int MessageId { get; set; }
        public string MessageFrom { get; set; }
        public string MessageTo { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageSentDate { get; set; }
    }
}
