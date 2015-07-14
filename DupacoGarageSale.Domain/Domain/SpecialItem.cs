using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class SpecialItem
    {
        public int SpecialItemsId { get; set; }

        [Required (ErrorMessage="Enter a title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Enter a description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Upload a picture")]
        public string PictureLink { get; set; }

        [Required(ErrorMessage = "Enter a price")]
        [DataType(DataType.Currency, ErrorMessage="Enter a price without special characters.")]
        public decimal Price { get; set; }

        public int SaleId { get; set; }
        public int ItemCategoryId { get; set; }
        public int ItemSubcategoryId { get; set; }
        public GarageSaleAddress SpecialItemAddress { get; set; }
        public SaleDatesTimes DatesTimes { get; set; }
        public string SaleDescription { get; set; }
    }
}
