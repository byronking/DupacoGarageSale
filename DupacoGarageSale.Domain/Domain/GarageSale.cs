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
        public string GarageSaleName { get; set; }
        public string SaleDescription { get; set; }
        public string GargeSalePicLink { get; set; }
        public string SaleAddress1 { get; set; }
        public string SaleAddress2 { get; set; }
        public string SaleCity { get; set; }        
        public string SaleState { get; set; }
        public int SaleStateId { get; set; }
        public string SaleZip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public SaleDatesTimes DatesTimes { get; set; }
        public string SaleHost { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyUser { get; set; }
        public string GarageSaleEmail { get; set; }
        public List<GarageSaleItem> GarageSaleItems { get; set; }
    }
}
