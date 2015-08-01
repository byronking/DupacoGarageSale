using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class ItineraryWaypoint
    {
        public int WaypointId { get; set; }
        public string WaypointAddress { get; set; }
        public string ItineraryNote { get; set; }
    }
}
