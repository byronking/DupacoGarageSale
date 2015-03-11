using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class GarageSale
    {
        public int GarageSaleId { get; set; }

        [Required(ErrorMessage = "Enter a name for your garage sale")]
        public string GarageSaleName { get; set; }

        [Required(ErrorMessage="Enter a description of your garage sale")]
        public string SaleDescription { get; set; }

        public string GargeSalePicLink { get; set; }

        [Required(ErrorMessage="Enter an address")]
        public string SaleAddress1 { get; set; }
        public string SaleAddress2 { get; set; }

        [Required(ErrorMessage="Enter a city")]
        public string SaleCity { get; set; }
        
        public string SaleState { get; set; }

        [Required(ErrorMessage = "Enter a state")]
        public int SaleStateId { get; set; }

        [Required(ErrorMessage = "Enter a zip code")]
        public string SaleZip { get; set; }
        public SaleDatesTimes DatesTimes { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyUser { get; set; }
        public string GarageSaleEmail { get; set; }
        public List<GarageSaleItem> GarageSaleItems { get; set; }
    }
}
