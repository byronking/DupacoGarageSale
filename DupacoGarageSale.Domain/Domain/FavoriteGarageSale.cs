using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class FavoriteGarageSale
    {
        public int FavoriteId { get; set; }
        public int GarageSaleId { get; set; }
        public int UserId { get; set; }
    }
}
