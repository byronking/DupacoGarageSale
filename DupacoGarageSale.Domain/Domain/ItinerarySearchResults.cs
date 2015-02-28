using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class ItinerarySearchResults
    {
        public List<ItinerarySpecialItem> ItinerarySpecialItems { get; set; }
        public List<ItineraryGarageSaleItem> ItineraryGarageSaleItems { get; set; }
    }
}
