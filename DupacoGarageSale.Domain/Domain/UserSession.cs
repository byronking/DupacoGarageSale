using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class UserSession
    {
        public Guid SessionKey { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime SessionStartDate { get; set; }
    }
}
