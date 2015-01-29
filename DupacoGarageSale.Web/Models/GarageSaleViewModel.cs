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
        public List<ItemCategory> ItemCategories { get; set; }
        public List<int> SelectedCategories { get; set; }
        public SpecialItem GarageSaleSpecialItem { get; set; }
        public List<SpecialItem> GarageSaleSpecialItems { get; set; }
        public BlogPost GarageSaleBlogPost { get; set; }
        public List<BlogPost> BlogPosts { get; set; }
        public GarageSaleSearchResults SearchResults { get; set; }
        public List<MapAddress> MapAddresses { get; set; }
        public List<GarageSaleItinerary> GarageSaleItinerary { get; set; }
        public GarageSaleMessage GarageSaleMessage { get; set; }
        public List<GarageSaleMessage> GarageSaleMessages { get; set; }
        public Itinerary UserItinerary { get; set; }
    }
}