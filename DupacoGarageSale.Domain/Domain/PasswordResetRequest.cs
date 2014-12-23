using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class PasswordResetRequest
    {
        public int PasswordResetId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ResetToken { get; set; }
        public DateTime RequestDateTime { get; set; }
    }
}
