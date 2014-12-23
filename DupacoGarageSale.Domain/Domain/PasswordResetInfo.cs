using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class PasswordResetInfo
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
