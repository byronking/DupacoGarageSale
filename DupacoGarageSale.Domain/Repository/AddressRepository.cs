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
    public class AddressRepository
    {
        /// <summary>
        /// This returns the list of American States.
        /// </summary>
        /// <returns></returns>
        public List<State> GetStates()
        {
            var statesList = new List<State>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetStates", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var state = new State
                        {
                            StateId = Convert.ToInt16(reader["state_id"]),
                            StateName = reader["state_name"].ToString()
                        };

                        statesList.Add(state);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return statesList;
        }
    }
}