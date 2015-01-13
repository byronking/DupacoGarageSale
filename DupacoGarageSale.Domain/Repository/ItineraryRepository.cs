using DupacoGarageSale.Data.Domain;
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
                    cmd.Parameters.Add("@itinerary_create_date", SqlDbType.Int).Value = itinerary.ItineraryOwner;
                    cmd.Parameters.Add("@itinerary_modify_date", SqlDbType.Int).Value = itinerary.ItineraryOwner;
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
                Console.Write(ex.ToString());
            }

            return saveResult;
        }

        public int UpdateItinerary(Itinerary itinerary)
        {
            var itineraryId = itinerary.ItineraryId;

            return itineraryId;
        }
    }
}
