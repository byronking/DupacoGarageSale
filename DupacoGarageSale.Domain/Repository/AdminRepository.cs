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
        /// This verifies whether the user is an admin or not.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool VerifyUserIsAdmin(string userName)
        {
            var userIsAdmin = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("VerifyUserIsAdmin", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@user_name", SqlDbType.VarChar).Value = userName;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var adminUser = reader["user_name"].ToString();
                    }

                    userIsAdmin = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return userIsAdmin;
        }

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
        public List<GarageSaleUser> GetAllUsersWithAddresses()
        {
            var usersList = new List<GarageSaleUser>();
            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetAllUsersWithAddresses", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var address = new UserAddress();
                        address.Address1 = reader["address1"].ToString();
                        address.Address2 = reader["address2"].ToString();
                        address.City = reader["city"].ToString();
                        address.State = reader["state_name"].ToString();
                        address.Zip = reader["zip"].ToString();

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
                        user.Address = address;
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
                        var address = new UserAddress();
                        address.Address1 = reader["address1"].ToString();
                        address.Address2 = reader["address2"].ToString();
                        address.City = reader["city"].ToString();
                        address.State = reader["state_name"].ToString();
                        address.Zip = reader["zip"].ToString();

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
                        user.Address = address;
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

        /// <summary>
        /// This gets the admin messages.
        /// </summary>
        /// <returns></returns>
        public List<AdminMessage> GetAdminMessages()
        {
            var adminMessages = new List<AdminMessage>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetAdminMessages", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var message = new AdminMessage
                        {
                            MessageText = reader["message_text"].ToString(),
                            MessageCreateDate = Convert.ToDateTime(reader["message_create_date"]),
                            MessagePublishDate = Convert.ToDateTime(reader["message_publish_date"]),
                            MessageType = reader["message_type"].ToString()
                        };

                        adminMessages.Add(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return adminMessages;
        }

        /// <summary>
        /// This gets the latest headline news message.
        /// </summary>
        /// <returns></returns>
        public List<AdminMessage> GetHeadlineNews()
        {
            var headlineNews = new List<AdminMessage>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetHeadlineNews", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var message = new AdminMessage
                        {
                            MessageText = reader["message_text"].ToString(),
                            MessageCreateDate = Convert.ToDateTime(reader["message_create_date"]),
                            MessagePublishDate = Convert.ToDateTime(reader["message_publish_date"]),
                            MessageType = reader["message_type"].ToString()
                        };

                        headlineNews.Add(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return headlineNews;
        }

        /// <summary>
        /// This gets the latest create sale instructions text.
        /// </summary>
        /// <returns></returns>
        public List<AdminMessage> GetCreateSaleInstructions()
        {
            var createSaleInstructions = new List<AdminMessage>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetCreateSaleInstructions", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var message = new AdminMessage
                        {
                            MessageText = reader["message_text"].ToString(),
                            MessageCreateDate = Convert.ToDateTime(reader["message_create_date"]),
                            MessagePublishDate = Convert.ToDateTime(reader["message_publish_date"]),
                            MessageType = reader["message_type"].ToString()
                        };

                        createSaleInstructions.Add(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return createSaleInstructions;
        }

        /// <summary>
        /// This gets the latest advanced sale instructions text.
        /// </summary>
        /// <returns></returns>
        public List<AdminMessage> GetAdvancedSaleInstructions()
        {
            var advancedSaleInstructions = new List<AdminMessage>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetAdvancedSaleInstructions", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var message = new AdminMessage
                        {
                            MessageText = reader["message_text"].ToString(),
                            MessageCreateDate = Convert.ToDateTime(reader["message_create_date"]),
                            MessagePublishDate = Convert.ToDateTime(reader["message_publish_date"]),
                            MessageType = reader["message_type"].ToString()
                        };

                        advancedSaleInstructions.Add(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return advancedSaleInstructions;
        }

        /// <summary>
        /// This saves admin messages, be they for the user home page or for the main home page.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This saves the contact us messages from the users.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public UserSaveResult SaveContactUsMessage(ContactUsMessage message)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SaveContactUsMessages", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@contact_name", SqlDbType.VarChar).Value = message.ContactName;
                    cmd.Parameters.Add("@contact_email", SqlDbType.VarChar).Value = message.ContactEmail;
                    cmd.Parameters.Add("@contact_phone", SqlDbType.VarChar).Value = message.ContactPhone;
                    cmd.Parameters.Add("@message_text", SqlDbType.VarChar).Value = message.MessageText;
                    cmd.Parameters.Add("@message_sent_date", SqlDbType.DateTime).Value = message.MessageSentDateTime;

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
        /// This gets the contact us messages.
        /// </summary>
        /// <returns></returns>
        public List<ContactUsMessage> GetContactUsMessages()
        {
            var contactUsMessages = new List<ContactUsMessage>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetContactUsMessages", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var message = new ContactUsMessage
                        {
                            MessageId = Convert.ToInt32(reader["message_id"]),
                            ContactName = reader["contact_name"].ToString(),
                            ContactEmail = reader["contact_email"].ToString(),
                            ContactPhone = reader["contact_phone"].ToString(),
                            MessageText = reader["message_text"].ToString(),
                            MessageSentDateTime = Convert.ToDateTime(reader["message_sent_date"])
                        };

                        message.MessageReplies = GetMessageReplies(message.MessageId);
                        contactUsMessages.Add(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return contactUsMessages;
        }

        /// <summary>
        /// This gets a contact us message by id.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public ContactUsMessage GetContactUsMessageById(int messageId)
        {
            var message = new ContactUsMessage();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetContactUsMessageById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@message_id", SqlDbType.Int).Value = messageId;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        message.ContactName = reader["contact_name"].ToString();
                        message.ContactEmail = reader["contact_email"].ToString();
                        message.ContactPhone = reader["contact_phone"].ToString();
                        message.MessageText = reader["message_text"].ToString();
                        message.MessageSentDateTime = Convert.ToDateTime(reader["message_sent_date"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return message;
        }

        /// <summary>
        /// This gets the replies to the contact us messages.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public List<MessageReply> GetMessageReplies(int messageId)
        {
            var replies = new List<MessageReply>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetContactUsReplies", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@message_id", SqlDbType.Int).Value = messageId;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var reply = new MessageReply
                        {
                            ReplyId = Convert.ToInt32(reader["reply_id"]),
                            MessageId = Convert.ToInt32(reader["message_id"]),
                            ReplyFrom = reader["reply_from"].ToString(),
                            ReplyDateTime = Convert.ToDateTime(reader["reply_date"]),
                            ReplyText = reader["reply_text"].ToString()
                        };

                        replies.Add(reply);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return replies;
        }

        /// <summary>
        /// This updates the contact us messages.
        /// </summary>
        /// <returns></returns>
        public UserSaveResult SaveContactUsReplies(MessageReply reply)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SaveContactUsReplies", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@reply_from", SqlDbType.VarChar).Value = reply.ReplyFrom;
                    cmd.Parameters.Add("@reply_to", SqlDbType.VarChar).Value = reply.ReplyTo;
                    cmd.Parameters.Add("@reply_date", SqlDbType.DateTime).Value = reply.ReplyDateTime;
                    cmd.Parameters.Add("@reply_text", SqlDbType.VarChar).Value = reply.ReplyText;
                    cmd.Parameters.Add("@message_id", SqlDbType.Int).Value = reply.MessageId;

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
        /// This gets all the email addresses for the autocomplete.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<EmailAddress> GetEmailAddressesByCriteria(string query)
        {
            var emailAddresses = new List<EmailAddress>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetEmailAddressesByQuery", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@query", SqlDbType.VarChar).Value = query;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var email = new EmailAddress
                        {
                            Email = reader["email"].ToString()
                        };

                        emailAddresses.Add(email);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return emailAddresses;
        }

        /// <summary>
        /// This gets the email addresses by community.
        /// </summary>
        /// <param name="community"></param>
        /// <returns></returns>
        public List<EmailAddress> GetEmailAddressesByCommunity(string community)
        {
            var emailAddresses = new List<EmailAddress>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetEmailAddressesByCommunity", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@community", SqlDbType.VarChar).Value = community;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var email = new EmailAddress
                        {
                            Email = reader["email"].ToString()
                        };

                        emailAddresses.Add(email);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return emailAddresses;
        }

        /// <summary>
        /// This gets the communities based on the existing garage sales.
        /// </summary>
        /// <returns></returns>
        public List<Community> GetCommunities()
        {
            var communityList = new List<Community>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetCommunities", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var community = new Community
                        {
                            Name = reader["sale_city"].ToString()
                        };
                            
                        communityList.Add(community);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return communityList;
        }
    }
}
