using DupacoGarageSale.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DupacoGarageSale.Web.Models
{
    public class GarageSaleViewModel
    {
        public GarageSaleUser User { get; set; }
        public GarageSale Sale { get; set; }
        public List<GarageSale> GarageSales { get; set; }
    }
}