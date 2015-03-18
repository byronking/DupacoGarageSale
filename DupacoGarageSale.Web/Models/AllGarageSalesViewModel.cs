using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DupacoGarageSale.Data.Domain;

namespace DupacoGarageSale.Web.Models
{
    public class AllGarageSalesViewModel
    {
        public List<GarageSale> AllGarageSales { get; set; }
        public GarageSaleUser LoggedInUser { get; set; }
    }
}