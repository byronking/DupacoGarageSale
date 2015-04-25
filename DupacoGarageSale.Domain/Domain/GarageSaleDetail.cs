using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class GarageSaleDetail
    {
        public int SaleId { get; set; }
        public string DateField1 { get; set; }	
        public string DateField1Start { get; set; }	
        public string DateField1End	{ get; set; }
        public string DateField2 { get; set; }	
        public string DateField2Start { get; set; }	
        public string DateField2End { get; set; }	
        public string DateField3 { get; set; }	
        public string DateField3Start { get; set; }	
        public string DateField3End { get; set; }	
        public string DateField4 { get; set; }	
        public string DateField4Start { get; set; }	
        public string DateField4End { get; set; }	
        public string SaleName { get; set; }	
        public string HouseNumberStreet { get; set; }	
        public string AptUnitCondoNumber { get; set; }	
        public string City { get; set; }
        public string State	{ get; set; }
        public string Zip { get; set; }	
        public string Description { get; set; }
        public string CategoryBaby	{ get; set; }
        public string CategoryClothingAccessories { get; set; }	
        public string CategoryElectronics { get; set; }
        public string CategoryHealthBeauty { get; set; }
        public string CategoryPets { get; set; }
        public string CategoryHome { get; set; }	
        public string CategoryHomeMaintenance { get; set; }	
        public string CategoryMedia { get; set; }	
        public string CategoryToysGames { get; set; }	
        public string CategoryVehicles { get; set; }	
        public string CategorySportsFitnessOutdoors { get; set; }	
        public string CategoryMusicalInstruments { get; set; }	
        public string UserName { get; set; }
        public string FirstName { get; set; }	
        public string LastName { get; set; }	
        public string ContactPhone { get; set; }	
        public string ContactEmail { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
