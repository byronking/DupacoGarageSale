using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class AdminMessage
    {
        public int MessageId { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageCreateDate { get; set; }
        public DateTime MessagePublishDate { get; set; }
        public string MessageType { get; set; }
    }
}
