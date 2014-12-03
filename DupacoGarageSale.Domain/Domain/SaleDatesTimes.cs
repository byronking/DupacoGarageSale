using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class SaleDatesTimes
    {
        public int SaleDateTimeId { get; set; }
        public DateTime SaleDateOne { get; set; }
        public string DayOneStart { get; set; }
        public string DayOneEnd { get; set; }
        public DateTime SaleDateTwo { get; set; }
        public string DayTwoStart { get; set; }
        public string DayTwoEnd { get; set; }
        public DateTime SaleDateThree { get; set; }
        public string DayThreeStart { get; set; }
        public string DayThreeEnd { get; set; }
        public DateTime SaleDateFour { get; set; }
        public string DayFourStart { get; set; }
        public string DayFourEnd { get; set; }
    }
}
