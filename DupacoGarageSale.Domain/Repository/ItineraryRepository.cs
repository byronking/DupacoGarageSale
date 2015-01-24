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
        /// This returns the user's itinerary with legs.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<GarageSaleItinerary> GetItineraryByUserId(int userId)
        {
            var itineraryList = new List<GarageSaleItinerary>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetItineraryByOwnerId", conn))
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
                            ItineraryLegId = Convert.ToInt32(reader["itinerary_leg_id"]),
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

        public UserSaveResult SaveItinerary(Itinerary itinerary)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SaveItinerary", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
                    cmd.Parameters.Add("@itinerary_id", SqlDbType.Int).Value = saleId;

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

        public UserSaveResult DeleteItineraryLeg(int itineraryLegId, int itineraryId, int saleId, int userId)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("DeleteItineraryLeg", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sale_id", SqlDbType.Int).Value = saleId;
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
    }
}
