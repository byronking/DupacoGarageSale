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
    public class AdminRepository
    {
        /// <summary>
        /// This returns all the garage sale users.
        /// </summary>
        /// <returns></returns>
        public List<GarageSaleUser> GetAllGarageSaleUsers()
        {
            var usersList = new List<GarageSaleUser>();
            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetAllGarageSaleUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var user = new GarageSaleUser();

                        DateTime? modifyDate = null;
                        if (reader["modify_date"] != DBNull.Value)
                        {
                            modifyDate = Convert.ToDateTime(reader["modify_date"]);
                        }

                        int userTypeId = 0;
                        if (reader["user_type_id"] == DBNull.Value)
                        {
                            userTypeId = 2;
                        }
                        else
                        {
                            userTypeId = Convert.ToInt32(reader["user_type_id"]);
                        }

                        user.Active = Convert.ToBoolean(reader["active"]);
                        user.CreateDate = Convert.ToDateTime(reader["create_date"]);
                        user.Email = reader["email"].ToString();
                        user.FirstName = reader["first_name"].ToString();
                        user.LastName = reader["last_name"].ToString();
                        user.ModifyDate = modifyDate;
                        user.ModifyUser = reader["modify_user"].ToString();
                        user.BytePassword = (byte[])reader["password"];
                        user.Phone = reader["phone"].ToString();
                        user.UserId = Convert.ToInt32(reader["user_id"]);
                        user.UserName = reader["user_name"].ToString();
                        user.ProfilePicLink = reader["profile_pic_link"].ToString();
                        user.UserTypeId = userTypeId;

                        usersList.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return usersList;
        }

        /// <summary>
        /// This returns all the garage sale users.
        /// </summary>
        /// <returns></returns>
        public List<GarageSaleUser> GetGarageSaleUsersForSearch(string criteria)
        {
            var usersList = new List<GarageSaleUser>();
            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetGarageSaleUsersForSearch", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@search_criteria", SqlDbType.VarChar).Value = criteria;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var user = new GarageSaleUser();

                        DateTime? modifyDate = null;
                        if (reader["modify_date"] != DBNull.Value)
                        {
                            modifyDate = Convert.ToDateTime(reader["modify_date"]);
                        }

                        int userTypeId = 0;
                        if (reader["user_type_id"] == DBNull.Value)
                        {
                            userTypeId = 2;
                        }
                        else
                        {
                            userTypeId = Convert.ToInt32(reader["user_type_id"]);
                        }

                        user.Active = Convert.ToBoolean(reader["active"]);
                        user.CreateDate = Convert.ToDateTime(reader["create_date"]);
                        user.Email = reader["email"].ToString();
                        user.FirstName = reader["first_name"].ToString();
                        user.LastName = reader["last_name"].ToString();
                        user.ModifyDate = modifyDate;
                        user.ModifyUser = reader["modify_user"].ToString();
                        user.BytePassword = (byte[])reader["password"];
                        user.Phone = reader["phone"].ToString();
                        user.UserId = Convert.ToInt32(reader["user_id"]);
                        user.UserName = reader["user_name"].ToString();
                        user.ProfilePicLink = reader["profile_pic_link"].ToString();
                        user.UserTypeId = userTypeId;

                        usersList.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return usersList;
        }

        /// <summary>
        /// This returns all the garage sales.
        /// </summary>
        /// <returns></returns>
        public List<GarageSale> GetAllGarageSales()
        {
            var garageSaleList = new List<GarageSale>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetAllGarageSales", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var sale = new GarageSale
                        {
                            GarageSaleId = Convert.ToInt32(reader["sale_id"]),
                            GarageSaleName = reader["sale_name"].ToString(),
                            SaleDescription = reader["sale_description"].ToString(),
                            SaleAddress1 = reader["sale_address1"].ToString(),
                            SaleAddress2 = reader["sale_address2"].ToString(),
                            SaleCity = reader["sale_city"].ToString(),
                            SaleState = reader["state_name"].ToString(),
                            SaleZip = reader["sale_zip"].ToString()
                        };

                        garageSaleList.Add(sale);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return garageSaleList;
        }

        /// <summary>
        /// This returns all the garage sales.
        /// </summary>
        /// <returns></returns>
        public List<GarageSale> GetGarageSalesForSearch(string criteria)
        {
            var garageSaleList = new List<GarageSale>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetGarageSalesForSearch", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@search_criteria", SqlDbType.VarChar).Value = criteria;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var sale = new GarageSale
                        {
                            GarageSaleId = Convert.ToInt32(reader["sale_id"]),
                            GarageSaleName = reader["sale_name"].ToString(),
                            SaleDescription = reader["sale_description"].ToString(),
                            SaleAddress1 = reader["sale_address1"].ToString(),
                            SaleAddress2 = reader["sale_address2"].ToString(),
                            SaleCity = reader["sale_city"].ToString(),
                            SaleState = reader["state_name"].ToString(),
                            SaleZip = reader["sale_zip"].ToString()
                        };

                        garageSaleList.Add(sale);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return garageSaleList;
        }

        /// <summary>
        /// This gets the count of registered users.
        /// </summary>
        /// <returns></returns>
        public int GetCountOfRegisteredUsers()
        {
            var userCount = 0;

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
            using (SqlCommand cmd = new SqlCommand("GetCountOfRegisteredUsers", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    userCount = Convert.ToInt32(reader["count"]);
                }
            }

            return userCount;
        }

        /// <summary>
        /// This gets the count of garage sales.
        /// </summary>
        /// <returns></returns>
        public int GetCountOfGarageSales()
        {
            var saleCount = 0;

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
            using (SqlCommand cmd = new SqlCommand("GetCountOfGarageSales", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    saleCount = Convert.ToInt32(reader["count"]);
                }
            }

            return saleCount;
        }

        /// <summary>
        /// This gets the count of garage sales.
        /// </summary>
        /// <returns></returns>
        public SpecialItemsCount GetItemsAndCategoriesCount()
        {
            var itemsCount = new SpecialItemsCount();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
            using (SqlCommand cmd = new SqlCommand("GetCountOfItemsAndCategories", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    itemsCount.Count = Convert.ToInt32(reader["items"]);
                    itemsCount.Categories = Convert.ToInt32(reader["categories"]);
                }
            }

            return itemsCount;
        }

        /// <summary>
        /// This gets all the blog posts for the admin view.
        /// </summary>
        /// <returns></returns>
        public List<BlogPost> GetAllBlogPosts()
        {
            var blogPostsList = new List<BlogPost>();
            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetAllBlogPosts", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var blogPost = new BlogPost
                        {
                            BlogPostId = Convert.ToInt32(reader["blog_post_id"]),
                            BlogPostTitle = reader["blog_post_title"].ToString(),
                            MediaTypeId = Convert.ToInt32(reader["media_type_id"]),
                            ImageUri = reader["image_uri"].ToString(),
                            YouTubeUri = reader["youtube_uri"].ToString(),
                            VineUri = reader["vine_uri"].ToString(),
                            PostMessage = reader["post_message"].ToString(),
                            SaleId = Convert.ToInt32(reader["sale_id"]),
                            PostDateTime = Convert.ToDateTime(reader["blog_post_date_time"]),
                            BlogPostUser = reader["blog_post_user_id"].ToString()
                        };

                        blogPostsList.Add(blogPost);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return blogPostsList;
        }

        /// <summary>
        /// This gets an indivudal blog post by id.
        /// </summary>
        /// <param name="blog_post_id"></param>
        /// <returns></returns>
        public BlogPost GetBlogPost(int blog_post_id)
        {
            var blogPost = new BlogPost();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetBlogPost", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@blog_post_id", SqlDbType.Int).Value = blog_post_id;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        blogPost = new BlogPost
                        {
                            BlogPostId = Convert.ToInt32(reader["blog_post_id"]),
                            BlogPostTitle = reader["blog_post_title"].ToString(),
                            MediaTypeId = Convert.ToInt32(reader["media_type_id"]),
                            ImageUri = reader["image_uri"].ToString(),
                            YouTubeUri = reader["youtube_uri"].ToString(),
                            VineUri = reader["vine_uri"].ToString(),
                            PostMessage = reader["post_message"].ToString(),
                            SaleId = Convert.ToInt32(reader["sale_id"]),
                            PostDateTime = Convert.ToDateTime(reader["blog_post_date_time"]),
                            BlogPostUser = reader["blog_post_user"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return blogPost;
        }


        public UserSaveResult SaveAdminMessage(AdminMessage message)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SaveAdminMessage", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@message_text", SqlDbType.VarChar).Value = message.MessageText;
                    cmd.Parameters.Add("@message_create_date", SqlDbType.DateTime).Value = message.MessageCreateDate;
                    cmd.Parameters.Add("@message_publish_date", SqlDbType.DateTime).Value = message.MessagePublishDate;
                    cmd.Parameters.Add("@message_type", SqlDbType.VarChar).Value = message.MessageType;

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
    }
}
