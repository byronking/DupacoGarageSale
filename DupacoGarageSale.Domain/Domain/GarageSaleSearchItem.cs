using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class GarageSaleSearchItem
    {
        public int GarageSaleItemsId { get; set; }
        public int SaleId { get; set; }
        public int ItemCategoryId { get; set; }
        public string ItemCategoryName { get; set; }
        public int ItemSubcategoryId { get; set; }
        public string ItemSubcategoryName { get; set; }
        public string SaleDescription { get; set; }
        public string ProfilePic { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
