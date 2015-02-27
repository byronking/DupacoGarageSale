using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class Itinerary
    {
        public int ItineraryId { get; set; }
        public string ItineraryName { get; set; }
        public int SaleId { get; set; }
        public DateTime ItineraryCreateDate { get; set; }
        public DateTime ItineraryModifyDate { get; set; }
        public int ItineraryOwner { get; set; }
    }
}
