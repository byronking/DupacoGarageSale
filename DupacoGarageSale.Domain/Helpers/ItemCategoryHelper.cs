using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Helpers
{
    public static class ItemCategoryHelper
    {
        public static List<int> GetItemCategoryIds(string searchCategory)
        {
            var categoryIdList = new List<int>();

            // Shameful, I know.
            switch (searchCategory)
            {
                case "all_baby":
                    {
                        categoryIdList = new List<int>(new int[] { 4, 6, 2, 7, 3, 8, 9, 5, 1 });
                    }
                    break;
                case "all_clothing_accessories":
                    {
                        categoryIdList = new List<int>(new int[] { 26, 28, 19, 12, 17, 10, 27, 13, 16, 22, 21, 24, 30, 25, 15, 29, 18, 11, 14, 23, 20 });
                    }
                    break;
                case "all_electronics":
                    {
                        categoryIdList = new List<int>(new int[] { 34, 36, 41, 42, 33, 37, 38, 35, 32, 40, 43, 31, 39 });
                    }
                    break;
                case "all_health_beauty":
                    {
                        categoryIdList = new List<int>(new int[] { 48, 47, 45, 44, 46 });
                    }
                    break;
                case "all_pets":
                    {
                        categoryIdList = new List<int>(new int[] { 51, 50, 49, 52, 55, 54, 53 });
                    }
                    break;
                case "all_home":
                    {
                        categoryIdList = new List<int>(new int[] { 79, 62, 61, 71, 57, 69, 56, 59, 58, 68, 70, 60, 66, 74, 77, 75, 78, 67, 72, 63, 65, 73, 76, 64 });
                    }
                    break;
                case "all_home_maintenance":
                    {
                        categoryIdList = new List<int>(new int[] { 80, 82, 81, 86, 87, 85, 83, 88, 84 });
                    }
                    break;
                case "all_media":
                    {
                        categoryIdList = new List<int>(new int[] { 89, 91, 90, 96, 94, 92, 93, 95, 97, 98 });
                    }
                    break;
                case "all_toys_games":
                    {
                        categoryIdList = new List<int>(new int[] { 99, 100, 112, 101, 114, 102, 103, 104, 105, 106, 108, 107, 111, 109, 110, 113 });
                    }
                    break;
                case "all_vehicles":
                    {
                        categoryIdList = new List<int>(new int[] { 120, 117, 115, 118, 122, 119, 121, 116 });
                    }
                    break;
                case "all_sports_fitness":
                    {
                        categoryIdList = new List<int>(new int[] { 135, 123, 125, 127, 137, 139, 132, 133, 130, 131, 128, 138, 129, 140, 126, 124, 134, 136 });
                    }
                    break;
                case "all_musical_instruments":
                    {
                        categoryIdList = new List<int>(new int[] { 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152 });
                    }
                    break;
            }

            return categoryIdList;
        }
    }
}
