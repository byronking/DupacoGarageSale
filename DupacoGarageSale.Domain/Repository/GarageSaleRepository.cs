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
                    cmd.Parameters.Add("@sale_state_id", SqlDbType.Int).Value = sale.SaleStateId;
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

                    // Save the garage sale items.
                    foreach (var item in sale.GarageSaleItems)
                    {
                        item.SaleId = saveResult.SaveResultId;
                    }

                    var itemsSaveResult = SaveGarageSaleItems(sale.GarageSaleItems);

                    if (saveResult.SaveResultId != 0 && itemsSaveResult == true)
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
        /// This saves a new garage sale.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserSaveResult UpdateGarageSale(GarageSale sale)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("UpdateGarageSale", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sale_id", SqlDbType.Int).Value = sale.GarageSaleId;
                    cmd.Parameters.Add("@sale_name", SqlDbType.VarChar).Value = sale.GarageSaleName;
                    cmd.Parameters.Add("@sale_description", SqlDbType.VarChar).Value = sale.SaleDescription;
                    cmd.Parameters.Add("@sale_address1", SqlDbType.VarChar).Value = sale.SaleAddress1;
                    cmd.Parameters.Add("@sale_address2", SqlDbType.VarChar).Value = sale.SaleAddress2 ?? string.Empty;
                    cmd.Parameters.Add("@sale_city", SqlDbType.VarChar).Value = sale.SaleCity;
                    cmd.Parameters.Add("@sale_state_id", SqlDbType.Int).Value = sale.SaleStateId;
                    cmd.Parameters.Add("@sale_zip", SqlDbType.VarChar).Value = sale.SaleZip;
                    cmd.Parameters.Add("@create_date", SqlDbType.DateTime).Value = sale.CreateDate;
                    cmd.Parameters.Add("@modify_date", SqlDbType.DateTime).Value = sale.ModifyDate;
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

                    // Save the garage sale items.
                    var itemsSaveResult = SaveGarageSaleItems(sale.GarageSaleItems);

                    if (saveResult.SaveResultId != 0 && itemsSaveResult == true)
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
        public GarageSale GetGarageSaleAndItemsById(int id)
        {
            var garageSale = new GarageSale();
            var garageSaleItemsList = new List<GarageSaleItem>();

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
                        garageSale.DatesTimes = new SaleDatesTimes
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

                        garageSale = new GarageSale
                        {
                            CreateDate = Convert.ToDateTime(reader["create_date"]),
                            DatesTimes = garageSale.DatesTimes,
                            GarageSaleId = Convert.ToInt32(reader["sale_id"]),
                            GarageSaleName = reader["sale_name"].ToString(),
                            ModifyDate = Convert.ToDateTime(reader["modify_date"]),
                            ModifyUser = reader["modify_user"].ToString(),
                            SaleAddress1 = reader["sale_address1"].ToString(),
                            SaleAddress2 = reader["sale_address2"].ToString(),
                            SaleCity = reader["sale_city"].ToString(),                            
                            SaleDescription = reader["sale_description"].ToString(),
                            SaleState = reader["state_name"].ToString(),
                            SaleStateId = Convert.ToInt32(reader["state_id"]),
                            SaleZip = reader["sale_zip"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetGarageSaleItemsBySaleId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sale_id", SqlDbType.VarChar).Value = id;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var garageSaleItem = new GarageSaleItem
                        {
                            GarageSaleItemsId = Convert.ToInt32(reader["garage_sale_items_id"]),
                            SaleId = Convert.ToInt32(reader["sale_id"]),
                            ItemCategoryId = Convert.ToInt32(reader["item_category_id"]),
                            ItemCategoryName = reader["item_category_name"].ToString(),
                            ItemSubcategoryId = Convert.ToInt32(reader["item_subcategory_id"]),
                            ItemSubcategoryName = reader["item_subcategory_name"].ToString()
                        };

                        garageSaleItemsList.Add(garageSaleItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            garageSale.GarageSaleItems = garageSaleItemsList;

            return garageSale;
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
                            SaleStateId = Convert.ToInt32(reader["state_id"]),
                            SaleZip = reader["sale_zip"].ToString()
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

        /// <summary>
        /// This saves garage sale items.
        /// </summary>
        /// <param name="garageSaleItems"></param>
        /// <returns></returns>
        private bool SaveGarageSaleItems(List<GarageSaleItem> garageSaleItems)
        {
            var savesSuccessful = false;
            var itemsSaveResult = new UserSaveResult();
            var saveResultsList = new List<UserSaveResult>();

            try
            {
                // First, delete any existing garage sale items for this sale.
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("DeleteGarageSaleItems", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sale_id", SqlDbType.Int).Value = garageSaleItems[0].SaleId;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            // Now, add the newly chosen items.
            try
            {
                foreach (var garageSaleItem in garageSaleItems)
                {
                    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                    using (SqlCommand cmd = new SqlCommand("SaveGarageSaleItems", conn))
                    {
                    
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sale_id", SqlDbType.Int).Value = garageSaleItem.SaleId;
                        cmd.Parameters.Add("@item_subcategory_id", SqlDbType.Int).Value = garageSaleItem.ItemSubcategoryId;

                        var returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        itemsSaveResult.SaveResultId = (int)returnParameter.Value;
                        saveResultsList.Add(itemsSaveResult);

                        cmd.Connection.Close();                        
                    }
                }

                if (saveResultsList.Count == garageSaleItems.Count)
                {
                    savesSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return savesSuccessful;
        }

        /// <summary>
        /// This saves a special item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public UserSaveResult SaveGarageSaleSpecialItem(SpecialItem item)
        {
            var itemSaveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SaveGarageSaleSpecialItems", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sale_id", SqlDbType.Int).Value = item.SaleId;
                    cmd.Parameters.Add("@item_subcategory_id", SqlDbType.Int).Value = item.ItemSubcategoryId;
                    cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = item.Title;
                    cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = item.Description;
                    cmd.Parameters.Add("@picture_link", SqlDbType.VarChar).Value = item.PictureLink;
                    cmd.Parameters.Add("@price", SqlDbType.Money).Value = item.Price;

                    var returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    itemSaveResult.SaveResultId = (int)returnParameter.Value;

                    if (itemSaveResult.SaveResultId != 0)
                    {
                        itemSaveResult.IsSaveSuccessful = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return itemSaveResult;
        }

        /// <summary>
        /// This gets special items by garage sale id.
        /// </summary>
        /// <param name="sale_id"></param>
        /// <returns></returns>
        public List<SpecialItem> GetGarageSaleSpecialItems(int sale_id)
        {
            var specialItems = new List<SpecialItem>();

            try
            {
                // First, delete any existing garage sale items for this sale.
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetGarageSaleSpecialItems", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sale_id", SqlDbType.Int).Value = sale_id;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var specialItem = new SpecialItem
                        {
                            SpecialItemsId = Convert.ToInt32(reader["special_items_id"]),
                            Title = reader["title"].ToString(),
                            Description = reader["description"].ToString(),
                            PictureLink = reader["picture_link"].ToString(),
                            Price = Math.Round(Convert.ToDecimal(reader["price"]), 2),
                            SaleId = sale_id,
                            ItemCategoryId = Convert.ToInt32(reader["item_category_id"]),
                            ItemSubcategoryId = Convert.ToInt32(reader["item_subcategory_id"])
                        };

                        specialItems.Add(specialItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return specialItems;
        }

        /// <summary>
        /// This deletes a garage sale by id.
        /// </summary>
        /// <param name="sale_id"></param>
        /// <returns></returns>
        public bool DeleteGarageSale(int sale_id)
        {
            var saveSuccessful = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("DeleteGarageSale", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sale_id", SqlDbType.Int).Value = sale_id;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    saveSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return saveSuccessful;
        }

        /// <summary>
        /// This deletes individual special items by id.
        /// </summary>
        /// <param name="special_items_id"></param>
        /// <returns></returns>
        public bool DeleteGarageSpecialItems(int special_items_id)
        {
            var saveSuccessful = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("DeleteGarageSpecialItems", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@special_items_id", SqlDbType.Int).Value = special_items_id;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    saveSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return saveSuccessful;
        }

        /// <summary>
        /// This updates individual garage sale special items.
        /// </summary>
        /// <param name="specialItem"></param>
        /// <returns></returns>
        public bool UpdateGarageSaleSpecialItem(SpecialItem specialItem)
        {
            var updateSuccessful = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("UpdateGarageSaleSpecialItem", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@special_items_id", SqlDbType.Int).Value = specialItem.SpecialItemsId;
                    //cmd.Parameters.Add("@sale_id", SqlDbType.Int).Value = specialItem.SaleId;
                    cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = specialItem.Title;
                    cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = specialItem.Description;
                    cmd.Parameters.Add("@picture_link", SqlDbType.VarChar).Value = specialItem.PictureLink;
                    cmd.Parameters.Add("@price", SqlDbType.Money).Value = specialItem.Price;
                    cmd.Parameters.Add("@item_subcategory_id", SqlDbType.Int).Value = specialItem.ItemSubcategoryId;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    updateSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return updateSuccessful;
        }

        /// <summary>
        /// This searches the garage sales based on search criteria and item subcategory.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="itemSubcategory"></param>
        /// <returns></returns>
        public GarageSaleSearchResults SearchGarageSales(string searchCriteria, int itemSubcategory)
        {
            var results = new GarageSaleSearchResults()
            {
                 GarageSaleItems = new List<GarageSaleSearchItem>(),
                  SpecialItems = new List<SpecialItem>()
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SearchSpecialItems", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@search_criteria", SqlDbType.VarChar).Value = searchCriteria;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var specialItem = new SpecialItem
                        {
                            SpecialItemsId = Convert.ToInt32(reader["special_items_id"]),
                            Title = reader["title"].ToString(),
                            Description = reader["description"].ToString(),
                            PictureLink = reader["picture_link"].ToString(),
                            Price = Math.Round(Convert.ToDecimal(reader["price"]), 2),
                            SaleId = Convert.ToInt32(reader["sale_id"]),
                            ItemCategoryId = Convert.ToInt32(reader["item_category_id"]),
                            ItemSubcategoryId = Convert.ToInt32(reader["item_subcategory_id"])
                        };

                        results.SpecialItems.Add(specialItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SearchGarageSaleItems", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@item_subcategory_id", SqlDbType.Int).Value = itemSubcategory;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new GarageSaleSearchItem
                        {
                            GarageSaleItemsId = Convert.ToInt32(reader["garage_sale_items_id"]),
                            ItemCategoryId = Convert.ToInt32(reader["item_category_id"]),
                            ItemCategoryName = reader["item_category_name"].ToString(),
                            ItemSubcategoryName = reader["item_subcategory_name"].ToString(),
                            ItemSubcategoryId = Convert.ToInt32(reader["item_subcategory_id"]),
                            SaleId = Convert.ToInt32(reader["sale_id"]),
                            Address1 = reader["sale_address1"].ToString(),
                            Address2 = reader["sale_address2"].ToString(),
                            City = reader["sale_city"].ToString(),
                            State = reader["state_name"].ToString(),
                            ZipCode = reader["sale_zip"].ToString()
                        };

                        results.GarageSaleItems.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return results;
        }

        /// <summary>
        /// This gets all the garage sale addresses.
        /// </summary>
        /// <returns></returns>
        public List<MapAddress> GetGarageSaleAddresses()
        {
            var garageSalesAddresses = new List<MapAddress>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetGarageSaleAddresses", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var address = new MapAddress
                        {
                            Address = reader["address"].ToString()
                        };

                        garageSalesAddresses.Add(address);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return garageSalesAddresses;
        }

        /// <summary>
        /// This gets all the garage sale addresses.
        /// </summary>
        /// <returns></returns>
        public int GetGarageSaleIdByAddresses(string address)
        {
            var garageSalesId = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetGarageSaleIdByAddresses", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@address", SqlDbType.VarChar).Value = address+"%";
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        garageSalesId = Convert.ToInt32(reader["sale_id"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return garageSalesId;
        }
    }
}
