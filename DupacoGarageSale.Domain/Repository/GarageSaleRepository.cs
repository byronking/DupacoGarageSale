using DupacoGarageSale.Data.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Repository
{
    public class GarageSaleRepository
    {
        /// <summary>
        /// This saves a new garage sale.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserSaveResult SaveGarageSale(GarageSale sale)
        {
            var saveResult = new UserSaveResult();            

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SaveGarageSale", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sale_name", SqlDbType.VarChar).Value = sale.GarageSaleName;
                    cmd.Parameters.Add("@sale_description", SqlDbType.VarChar).Value = sale.SaleDescription;
                    cmd.Parameters.Add("@sale_address1", SqlDbType.VarChar).Value = sale.SaleAddress1;
                    cmd.Parameters.Add("@sale_address2", SqlDbType.VarChar).Value = sale.SaleAddress2 ?? string.Empty;
                    cmd.Parameters.Add("@sale_city", SqlDbType.VarChar).Value = sale.SaleCity;
                    cmd.Parameters.Add("@sale_state", SqlDbType.VarChar).Value = sale.SaleState;
                    cmd.Parameters.Add("@sale_zip", SqlDbType.VarChar).Value = sale.SaleZip;
                    cmd.Parameters.Add("@create_date", SqlDbType.DateTime).Value = sale.CreateDate;
                    cmd.Parameters.Add("@modify_date", SqlDbType.DateTime).Value = sale.CreateDate;
                    cmd.Parameters.Add("@modify_user", SqlDbType.VarChar).Value = sale.ModifyUser;

                    cmd.Parameters.Add("@sale_date_one", SqlDbType.DateTime).Value = sale.DatesTimes.SaleDateOne;

                    DateTime? dayOneStart = null;

                    if (sale.DatesTimes.DayOneStart == null)
                    {
                        dayOneStart = null;
                    }
                    else
                    {
                        dayOneStart = Convert.ToDateTime(sale.DatesTimes.DayOneStart);
                    } 

                    cmd.Parameters.Add("@day_one_start", SqlDbType.DateTime).Value = dayOneStart;

                    DateTime? dayOneEnd = null;

                    if (sale.DatesTimes.DayOneEnd == null)
                    {
                        dayOneEnd = null;
                    }
                    else
                    {
                        dayOneEnd = Convert.ToDateTime(sale.DatesTimes.DayOneEnd);
                    } 

                    cmd.Parameters.Add("@day_one_end", SqlDbType.DateTime).Value = dayOneEnd;

                    cmd.Parameters.Add("@sale_date_two", SqlDbType.DateTime).Value = sale.DatesTimes.SaleDateTwo;
                    cmd.Parameters.Add("@day_two_start", SqlDbType.DateTime).Value = sale.DatesTimes.DayTwoStart;
                    cmd.Parameters.Add("@day_two_end", SqlDbType.DateTime).Value = sale.DatesTimes.DayTwoEnd;

                    cmd.Parameters.Add("@sale_date_three", SqlDbType.DateTime).Value = sale.DatesTimes.SaleDateThree;
                    cmd.Parameters.Add("@day_three_start", SqlDbType.DateTime).Value = sale.DatesTimes.DayThreeStart;
                    cmd.Parameters.Add("@day_three_end", SqlDbType.DateTime).Value = sale.DatesTimes.DayThreeEnd;

                    cmd.Parameters.Add("@sale_date_four", SqlDbType.DateTime).Value = sale.DatesTimes.SaleDateFour;
                    cmd.Parameters.Add("@day_four_start", SqlDbType.DateTime).Value = sale.DatesTimes.DayFourStart;
                    cmd.Parameters.Add("@day_four_end", SqlDbType.DateTime).Value = sale.DatesTimes.DayFourEnd;

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

        public GarageSale GetGarageSaleById(int id)
        {
            var sale = new GarageSale();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetGarageSaleById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sale_id", SqlDbType.VarChar).Value = id;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return sale;
        }
    }
}
