using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class BlogPost
    {
        public int BlogPostId { get; set; }

        [Required (ErrorMessage="Add a title to your post")]
        public string BlogPostTitle { get; set; }
        public DateTime PostDateTime { get; set; }
        public string BlogPostUser { get; set; }
        public int MediaTypeId { get; set; }
        public string ImageUri { get; set; }
        public string YouTubeUri { get; set; }
        public string VineUri { get; set; }
        public string PostMessage { get; set; }
        public int SaleId { get; set; }
    }
}
