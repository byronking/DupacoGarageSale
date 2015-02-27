using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class GarageSaleItinerary
    {
        public int ItineraryId { get; set; }
        public string ItineraryName { get; set; }
        public DateTime ItineraryCreatedDate { get; set; }
        public int ItineraryLegId { get; set; }
        public int ItineraryLegOrder { get; set; }
        public int ItineraryLegsCount { get; set; }
        public int SaleId { get; set; }
        public string SaleAddress1 { get; set; }
        public string SaleAddress2 { get; set; }
        public string SaleCity{ get; set; }
        public string SaleState { get; set; }
        public string SaleZipCode { get; set; }
    }
}
