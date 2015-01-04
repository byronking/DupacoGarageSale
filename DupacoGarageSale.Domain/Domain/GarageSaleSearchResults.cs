using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class GarageSaleSearchResults
    {
        public List<SpecialItem> SpecialItems { get; set; }
        public List<GarageSaleSearchItem> GarageSaleItems { get; set; }
    }
}
