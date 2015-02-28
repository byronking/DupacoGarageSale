using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class ItinerarySpecialItem
    {
        public int SpecialItemsId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string PictureLink { get; set; }
        public decimal Price { get; set; }
        public int SaleId { get; set; }
        public int ItemCategoryId { get; set; }
        public int ItemSubcategoryId { get; set; }
        public GarageSaleAddress SpecialItemAddress { get; set; }
        public SaleDatesTimes SaleDatesTimes { get; set; }
    }
}
