using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class MappingData
    {
        public string StartingAddress { get; set; }
        public List<string> Addresses { get; set; }
        public string Radius { get; set; }
    }
}
