using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DupacoGarageSale.Data.Domain;

namespace DupacoGarageSale.Web.Models
{
    public class SearchViewModel
    {
        public GarageSaleUser User { get; set; }
        public GarageSaleSearchResults SearchResults { get; set; }
    }
}