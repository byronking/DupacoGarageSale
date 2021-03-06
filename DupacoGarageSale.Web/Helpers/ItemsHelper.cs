﻿using DupacoGarageSale.Data.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DupacoGarageSale.Web.Helpers
{
    public static class ItemsHelper
    {
        public static List<int> GetRandomSpecialItems()
        {
            // Get 4 random special items.
            var randomSpecialItems = new List<int>();
            var minMax = GetMinAndMaxItemNumbers("specialItems");
            var min = 0;
            var max = 0;

            if (minMax.Count > 0)
            {
                min = minMax[0];
                max = minMax[1];

                Random rnd = new Random();
                for (int i = 0; i < 4; i++)
                {
                    var currentValue = rnd.Next(min, max);
                    while (randomSpecialItems.Exists(value => value == currentValue))
                    {
                        currentValue = rnd.Next(min, max);
                    }
                    randomSpecialItems.Add(currentValue);
                } 
            }            

            return randomSpecialItems;
        }

        // Get 4 random garage sale items.
        public static List<int> GetRandomGarageSaleItems()
        {
            // Get 4 random special items.
            var randomGarageSaleItems = new List<int>();
            var minMax = GetMinAndMaxItemNumbers("garageSaleItems");
            var min = 0;
            var max = 0;

            if (minMax.Count > 0)
            {
                min = minMax[0];
                max = minMax[1];

                Random rnd = new Random();
                for (int i = 0; i < 4; i++)
                {
                    var currentValue = rnd.Next(min, max);
                    randomGarageSaleItems.Add(currentValue);
                } 
            }            

            return randomGarageSaleItems;
        }

        public static List<int> GetMinAndMaxItemNumbers(string itemType)
        {
            var minAndMax = new List<int>();
            var sproc = string.Empty;

            if (itemType == "specialItems")
            {
                sproc = "GetMinAndMaxSpecialItems";
            }
            else
            {
                sproc = "GetMinAndMaxGarageSaleItems";
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand(sproc, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        minAndMax.Add(Convert.ToInt32(reader["min"]));
                        minAndMax.Add(Convert.ToInt32(reader["max"]));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return minAndMax;
        }
    }
}