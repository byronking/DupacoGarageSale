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

        /// <summary>
        /// This gets an individual garage sale by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return sale;
        }

         /// <summary>
        /// This gets a list of garage sales by user name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List<GarageSale></returns>
        public List<GarageSale> GetGarageSaleByUserName(string username)
        {
            var garageSalesList = new List<GarageSale>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetGarageSalesByUserName", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@garage_sale_user_name", SqlDbType.VarChar).Value = username;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var saleDatesTimes = new SaleDatesTimes();
                        saleDatesTimes.SaleDateTimeId = Convert.ToInt32(reader["sale_date_time_id"]);
                        saleDatesTimes.SaleDateOne = Convert.ToDateTime(reader["sale_date_one"]);
                        saleDatesTimes.DayOneStart = reader["day_one_start"].ToString();
                        saleDatesTimes.DayOneEnd = reader["day_one_end"].ToString();
                        saleDatesTimes.SaleDateTwo = Convert.ToDateTime(reader["sale_date_two"]);
                        saleDatesTimes.DayTwoStart = reader["day_two_start"].ToString();
                        saleDatesTimes.DayTwoEnd = reader["day_two_end"].ToString();
                        saleDatesTimes.SaleDateThree = Convert.ToDateTime(reader["sale_date_three"]);
                        saleDatesTimes.DayThreeStart = reader["day_three_start"].ToString();
                        saleDatesTimes.DayThreeEnd = reader["day_three_end"].ToString();
                        saleDatesTimes.SaleDateFour = Convert.ToDateTime(reader["sale_date_four"]);
                        saleDatesTimes.DayFourStart = reader["day_four_start"].ToString();
                        saleDatesTimes.DayFourEnd = reader["day_four_end"].ToString();

                        var garageSale = new GarageSale
                        {
                            CreateDate = Convert.ToDateTime(reader["create_date"]),
                            DatesTimes = saleDatesTimes,
                            GarageSaleId = Convert.ToInt32(reader["sale_id"]),
                            GarageSaleName = reader["sale_name"].ToString(),
                            ModifyDate = Convert.ToDateTime(reader["modify_date"]),
                            ModifyUser = reader["modify_user"].ToString(),
                            SaleAddress1 = reader["sale_address1"].ToString(),
                            SaleAddress2 = reader["sale_address2"].ToString(),
                            SaleCity = reader["sale_city"].ToString(),
                            SaleDescription = reader["sale_description"].ToString(),
                            SaleState = reader["state_name"].ToString(),
                            SaleZip = reader["sale_zip"].ToString(),
                        };

                        garageSalesList.Add(garageSale);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return garageSalesList;
        }

        /// <summary>
        /// This gets all the subcategories
        /// </summary>
        /// <param name="category_id"></param>
        /// <returns></returns>
        public List<ItemCategory> GetCategoriesAndSubcategories()
        {
            var subcategoriesList = new List<ItemCategory>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetCategoriesAndSubcategories", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var subcategory = new ItemCategory
                        {
                            ItemCategoryId = Convert.ToInt32(reader["item_category_id"]),
                            ItemCategoryName = reader["item_category_name"].ToString(),
                            ItemSubcategoryId = Convert.ToInt32(reader["item_subcategory_id"]),
                            ItemSubcategoryName = reader["item_subcategory_name"].ToString()
                        };

                        subcategoriesList.Add(subcategory);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return subcategoriesList;
        }

        /// <summary>
        /// This gets the subcategories by category id.
        /// </summary>
        /// <param name="category_id"></param>
        /// <returns></returns>
        public List<ItemCategory> GetCategoriesByCategoryId(int category_id)
        {
            var categoriesList = new List<ItemCategory>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetSubcategoriesByCategoryId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@category_id", SqlDbType.Int).Value = category_id;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var category = new ItemCategory
                        {
                            ItemCategoryId = Convert.ToInt32(reader["item_category_id"]),
                            ItemCategoryName = reader["item_category_name"].ToString(),
                            ItemSubcategoryId = Convert.ToInt32(reader["item_subcategory_id"]),
                            ItemSubcategoryName = reader["item_subcategory_name"].ToString()
                        };

                        categoriesList.Add(category);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return categoriesList;
        }
    }
}
