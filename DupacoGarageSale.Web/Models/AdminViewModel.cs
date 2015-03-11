using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DupacoGarageSale.Data.Domain;

namespace DupacoGarageSale.Web.Models
{
    public class AdminViewModel
    {
        public GarageSaleUser AdminUser { get; set; }
        public GarageSaleUser User { get; set; }
        public List<GarageSaleUser> Users { get; set; }
        public GarageSale GarageSale { get; set; }
        public List<GarageSale> GarageSales { get; set; }
        public List<GarageSale> UserGarageSales { get; set; }
        public GarageSaleItem GarageSaleItem { get; set; }
        public List<GarageSaleItem> GarageSaleItems { get; set; }
        public SpecialItem GarageSaleSpecialItem { get; set; }
        public List<SpecialItem> GarageSaleSpecialItems { get; set; }
        public List<ItemCategory> ItemCategories { get; set; }
        public List<int> SelectedCategories { get; set; }
        public BlogPost BlogPost { get; set; }
        public List<BlogPost> BlogPosts { get; set; }
        public GarageSaleMessage GarageSaleMessage { get; set; }
        public List<GarageSaleMessage> GarageSaleMessages { get; set; }
        public int UserCount { get; set; }
        public int GarageSaleCount { get; set; }
        public SpecialItemsCount SpecialItemsCount { get; set; }
        public string SearchCriteria { get; set; }
        public string HeadlineNews { get; set; }
        public List<AdminMessage> AdminMessages { get; set; }
        public List<ContactUsMessage> ContactUsMessages { get; set; }
    }
}