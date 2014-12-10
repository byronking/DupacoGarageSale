using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class GarageSaleItem
    {
        public int GarageSaleItemsId { get; set; }
        public int SaleId { get; set; }
        public int ItemCategoryId { get; set; }
        public string ItemCategoryName { get; set; }
        public int ItemSubcategoryId { get; set; }
        public string ItemSubcategoryName { get; set; }
    }
}
