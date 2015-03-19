using DupacoGarageSale.Data.Domain;
using DupacoGarageSale.Data.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Repository
{
    public class ItineraryRepository
    {
        /// <summary>
        /// This checks for an existing itinerary for the user.
        /// </summary>
        /// <param name="itineraryOwner"></param>
        /// <returns></returns>
        public Itinerary CheckForItinerary(int itineraryOwner)
        {
            var itinerary = new Itinerary();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("CheckForItinerary", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@itinerary_owner", SqlDbType.Int).Value = itineraryOwner;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        itinerary.ItineraryId = Convert.ToInt32(reader["itinerary_id"]);
                        itinerary.SaleId = Convert.ToInt32(reader["sale_id"]);
                        itinerary.ItineraryCreateDate = Convert.ToDateTime(reader["itinerary_create_date"]);
                        itinerary.ItineraryModifyDate = Convert.ToDateTime(reader["itinerary_modify_date"]);
                        itinerary.ItineraryOwner = Convert.ToInt32(reader["itinerary_owner"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return itinerary;
        }

        /// <summary>
        /// This returns the user's itinerary with legs by itineraryId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<GarageSaleItinerary> GetItinerariesByUserId(int userId)
        {
            var itineraryList = new List<GarageSaleItinerary>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetItinerariesByOwnerId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@itinerary_owner", SqlDbType.Int).Value = userId;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var itinerary = new GarageSaleItinerary
                        {
                            ItineraryId = Convert.ToInt32(reader["itinerary_id"]),
                            ItineraryName = reader["itinerary_name"].ToString(),
                            ItineraryCreatedDate = Convert.ToDateTime(reader["itinerary_create_date"])
                        };

                        itineraryList.Add(itinerary);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return itineraryList;
        }

        /// <summary>
        /// This returns the user's itinerary with legs by itineraryId.
        /// </summary>
        /// <param name="itineraryId"></param>
        /// <returns></returns>
        public List<GarageSaleItinerary> GetItinerariesByItineraryId(int itineraryId)
        {
            var itineraryList = new List<GarageSaleItinerary>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetItinerariesByItineraryId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@itinerary_id", SqlDbType.Int).Value = itineraryId;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var itinerary = new GarageSaleItinerary
                        {
                            ItineraryId = Convert.ToInt32(reader["itinerary_id"]),
                            ItineraryName = reader["itinerary_name"].ToString(),
                            ItineraryCreatedDate = Convert.ToDateTime(reader["itinerary_create_date"]),
                            ItineraryLegId = Convert.ToInt32(reader["itinerary_leg_id"]),
                            ItineraryLegOrder = Convert.ToInt32(reader["leg_order"]),
                            ItineraryLegsCount = Convert.ToInt32(reader["legs_count"]),
                            SaleId = Convert.ToInt32(reader["sale_id"]),
                            SaleAddress1 = reader["sale_address1"].ToString(),
                            SaleAddress2 = reader["sale_address2"].ToString(),
                            SaleCity = reader["sale_city"].ToString(),
                            SaleState = reader["state_name"].ToString(),
                            SaleZipCode = reader["sale_zip"].ToString()
                        };

                        itineraryList.Add(itinerary);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return itineraryList;
        }

        /// <summary>
        /// This gets an itinerary by itinerary id.
        /// </summary>
        /// <param name="itineraryId"></param>
        /// <returns></returns>
        public GarageSaleItinerary GetItineraryByItineraryId(int itineraryId)
        {
            var itinerary = new GarageSaleItinerary();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetItineraryByItineraryId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@itinerary_id", SqlDbType.Int).Value = itineraryId;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        itinerary = new GarageSaleItinerary
                        {
                            ItineraryId = Convert.ToInt32(reader["itinerary_id"]),
                            ItineraryName = reader["itinerary_name"].ToString(),
                            ItineraryCreatedDate = Convert.ToDateTime(reader["itinerary_create_date"]),
                            ItineraryLegId = Convert.ToInt32(reader["itinerary_leg_id"]),
                            ItineraryLegOrder = Convert.ToInt32(reader["leg_order"]),
                            //ItineraryLegsCount = Convert.ToInt32(reader["legs_count"]),
                            SaleId = Convert.ToInt32(reader["sale_id"]),
                            SaleAddress1 = reader["sale_address1"].ToString(),
                            SaleAddress2 = reader["sale_address2"].ToString(),
                            SaleCity = reader["sale_city"].ToString(),
                            SaleState = reader["state_name"].ToString(),
                            SaleZipCode = reader["sale_zip"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return itinerary;
        }

        /// <summary>
        /// This saves a user's new itinerary nad first itinerary leg.
        /// </summary>
        /// <param name="itinerary"></param>
        /// <returns></returns>
        public UserSaveResult SaveItinerary(Itinerary itinerary)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SaveItinerary", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@itinerary_name", SqlDbType.VarChar).Value = itinerary.ItineraryName;
                    cmd.Parameters.Add("@sale_id", SqlDbType.Int).Value = itinerary.SaleId;
                    cmd.Parameters.Add("@itinerary_create_date", SqlDbType.DateTime).Value = itinerary.ItineraryCreateDate;
                    cmd.Parameters.Add("@itinerary_modify_date", SqlDbType.DateTime).Value = itinerary.ItineraryModifyDate;
                    cmd.Parameters.Add("@itinerary_owner", SqlDbType.Int).Value = itinerary.ItineraryOwner;

                    var returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    saveResult.SaveResultId = (int)returnParameter.Value;

                    if (saveResult.SaveResultId != 0)
                    {
                        saveResult.IsSaveSuccessful = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return saveResult;
        }

        public UserSaveResult SaveItineraryLeg(int itineraryId, int saleId)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SaveItineraryLeg", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sale_id", SqlDbType.Int).Value = saleId;
                    cmd.Parameters.Add("@itinerary_id", SqlDbType.Int).Value = itineraryId;

                    var returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    saveResult.SaveResultId = (int)returnParameter.Value;

                    if (saveResult.SaveResultId != 0)
                    {
                        saveResult.IsSaveSuccessful = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return saveResult;
        }

        /// <summary>
        /// This deletes a user's itinerary by id.
        /// </summary>
        /// <param name="itineraryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserSaveResult DeleteItinerary(int itineraryId, int userId)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("DeleteItinerary", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@itinerary_id", SqlDbType.Int).Value = itineraryId;
                    cmd.Parameters.Add("@itinerary_owner", SqlDbType.Int).Value = userId;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    saveResult.IsSaveSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return saveResult;
        }  

        public UserSaveResult DeleteFromItinerary(int itineraryId, int saleId)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("DeleteFromItinerary", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@itinerary_id", SqlDbType.Int).Value = itineraryId;
                    cmd.Parameters.Add("@sale_id", SqlDbType.Int).Value = saleId;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    saveResult.IsSaveSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return saveResult;
        }

        public UserSaveResult DeleteItineraryLeg(int itineraryLegId, int itineraryId, int saleId, int userId)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("DeleteItineraryLeg", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@itinerary_id", SqlDbType.Int).Value = itineraryId;
                    cmd.Parameters.Add("@itinerary_leg_id", SqlDbType.Int).Value = itineraryLegId;
                    cmd.Parameters.Add("@itinerary_owner", SqlDbType.Int).Value = userId;

                    var returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    saveResult.IsSaveSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return saveResult;
        }        

        /// <summary>
        /// This searches the garage sales based on search criteria
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public ItinerarySearchResults ItineraryPageSearch(string searchCriteria)
        {
            var results = new ItinerarySearchResults()
            {
                ItinerarySpecialItems = new List<ItinerarySpecialItem>(),
                ItineraryGarageSaleItems = new List<ItineraryGarageSaleItem>()
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SearchSpecialItemsByCriteriaForItinerary", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@search_criteria", SqlDbType.VarChar).Value = searchCriteria;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var garageSaleAddress = new GarageSaleAddress
                        {
                            Address1 = reader["sale_address1"].ToString(),
                            Address2 = reader["sale_address2"].ToString(),
                            City = reader["sale_city"].ToString(),
                            State = reader["state_name"].ToString(),
                            ZipCode = reader["sale_zip"].ToString()
                        };

                        var datesTimes = new SaleDatesTimes
                        {
                            SaleDateTimeId = Convert.ToInt32(reader["sale_date_time_id"]),
                            SaleDateOne = Convert.ToDateTime(reader["sale_date_one"]),
                            DayOneStart = reader["day_one_start"].ToString(),
                            DayOneEnd = reader["day_one_end"].ToString(),
                            SaleDateTwo = Convert.ToDateTime(reader["sale_date_two"]),
                            DayTwoStart = reader["day_two_start"].ToString(),
                            DayTwoEnd = reader["day_two_end"].ToString(),
                            SaleDateThree = Convert.ToDateTime(reader["sale_date_three"]),
                            DayThreeStart = reader["day_three_start"].ToString(),
                            DayThreeEnd = reader["day_three_end"].ToString(),
                            SaleDateFour = Convert.ToDateTime(reader["sale_date_four"]),
                            DayFourStart = reader["day_four_start"].ToString(),
                            DayFourEnd = reader["day_four_end"].ToString(),                        
                        };

                        var specialItem = new ItinerarySpecialItem
                        {
                            SpecialItemsId = Convert.ToInt32(reader["special_items_id"]),
                            Title = reader["title"].ToString(),
                            Description = reader["description"].ToString(),
                            PictureLink = reader["picture_link"].ToString(),
                            Price = Math.Round(Convert.ToDecimal(reader["price"]), 2),
                            SaleId = Convert.ToInt32(reader["sale_id"]),
                            ItemCategoryId = Convert.ToInt32(reader["item_category_id"]),
                            ItemSubcategoryId = Convert.ToInt32(reader["item_subcategory_id"]),
                            SpecialItemAddress = garageSaleAddress,
                            SaleDatesTimes = datesTimes
                        };

                        results.ItinerarySpecialItems.Add(specialItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SearchGarageSaleItemsByCriteriaForItinerary", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@search_criteria", SqlDbType.VarChar).Value = searchCriteria;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var datesTimes = new SaleDatesTimes
                        {
                            SaleDateTimeId = Convert.ToInt32(reader["sale_date_time_id"]),
                            SaleDateOne = Convert.ToDateTime(reader["sale_date_one"]),
                            DayOneStart = reader["day_one_start"].ToString(),
                            DayOneEnd = reader["day_one_end"].ToString(),
                            SaleDateTwo = Convert.ToDateTime(reader["sale_date_two"]),
                            DayTwoStart = reader["day_two_start"].ToString(),
                            DayTwoEnd = reader["day_two_end"].ToString(),
                            SaleDateThree = Convert.ToDateTime(reader["sale_date_three"]),
                            DayThreeStart = reader["day_three_start"].ToString(),
                            DayThreeEnd = reader["day_three_end"].ToString(),
                            SaleDateFour = Convert.ToDateTime(reader["sale_date_four"]),
                            DayFourStart = reader["day_four_start"].ToString(),
                            DayFourEnd = reader["day_four_end"].ToString(),
                        };

                        var item = new ItineraryGarageSaleItem
                        {
                            GarageSaleItemsId = Convert.ToInt32(reader["garage_sale_items_id"]),
                            ItemCategoryId = Convert.ToInt32(reader["item_category_id"]),
                            ItemCategoryName = reader["item_category_name"].ToString(),
                            ItemSubcategoryName = reader["item_subcategory_name"].ToString(),
                            ItemSubcategoryId = Convert.ToInt32(reader["item_subcategory_id"]),
                            Address1 = reader["sale_address1"].ToString(),
                            Address2 = reader["sale_address2"].ToString(),
                            City = reader["sale_city"].ToString(),
                            State = reader["state_name"].ToString(),
                            ZipCode = reader["sale_zip"].ToString(),
                            SaleId = Convert.ToInt32(reader["sale_id"]),
                            SaleDatesTimes = datesTimes
                        };

                        results.ItineraryGarageSaleItems.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return results;
        }

        /// <summary>
        /// This saves the user's selected waypoints.
        /// </summary>
        /// <param name="waypointAddress"></param>
        /// <param name="itineraryId"></param>
        /// <returns></returns>
        public UserSaveResult SaveItineraryWaypoints(string waypointAddress, int itineraryId)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SaveItineraryWaypoints", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@waypoint_address", SqlDbType.VarChar).Value = waypointAddress;
                    cmd.Parameters.Add("@itinerary_id", SqlDbType.Int).Value = itineraryId;

                    var returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    saveResult.SaveResultId = (int)returnParameter.Value;

                    if (saveResult.SaveResultId != 0)
                    {
                        saveResult.IsSaveSuccessful = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return saveResult;
        }

        /// <summary>
        /// This gets the waypoints saved by the user.
        /// </summary>
        /// <param name="itineraryId"></param>
        /// <returns></returns>
        public List<string> GetItineraryWaypoints(int itineraryId)
        {
            var itineraryWaypoints = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetWaypointsByItineraryId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@itinerary_id", SqlDbType.Int).Value = itineraryId;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var waypoint = reader["waypoint_address"].ToString();
                        itineraryWaypoints.Add(waypoint);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return itineraryWaypoints;
        }

        /// <summary>
        /// This deletes the user's waypoint.
        /// </summary>
        /// <param name="waypointAddress"></param>
        /// <param name="itineraryId"></param>
        /// <returns></returns>
        public UserSaveResult DeleteItineraryWaypoints(string waypointAddress, int itineraryId)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("DeleteItineraryWaypoints", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@waypoint_address", SqlDbType.VarChar).Value = waypointAddress;
                    cmd.Parameters.Add("@itinerary_id", SqlDbType.Int).Value = itineraryId;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    saveResult.IsSaveSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return saveResult;
        }
    }
}
