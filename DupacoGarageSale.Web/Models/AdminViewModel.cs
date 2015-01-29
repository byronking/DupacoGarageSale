using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DupacoGarageSale.Data.Domain;

namespace DupacoGarageSale.Web.Models
{
    public class AdminViewModel
    {
        public GarageSaleUser User { get; set; }
        public List<GarageSaleUser> Users { get; set; }
    }
}