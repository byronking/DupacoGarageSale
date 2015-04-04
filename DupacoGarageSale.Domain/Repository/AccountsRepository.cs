using DupacoGarageSale.Data.Domain;
using DupacoGarageSale.Data.Services;
using DupacoGarageSale.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Repository
{
    public class AccountsRepository
    {
        public bool CheckForExistingAccount(string user_name, string email)
        {
            var isExistingAccount = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("CheckForExistingAccount", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@user_name", SqlDbType.VarChar).Value = user_name;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var userName = reader["user_name"].ToString();
                        var emailAddress = reader["email"].ToString();

                        isExistingAccount = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }
            return isExistingAccount;
        }

        /// <summary>
        /// This saves a new garage sale user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserSaveResult SaveGarageSaleUser(GarageSaleUser user)
        {
            var saveResult = new UserSaveResult();

            // Hash the password.
            var hashedPassword = AccountHelper.GetSHA1Hash(user.UserName, user.Password);

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SaveGarageSaleUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@first_name", SqlDbType.VarChar).Value = user.FirstName;
                    cmd.Parameters.Add("@last_name", SqlDbType.VarChar).Value = user.LastName;
                    cmd.Parameters.Add("@user_name", SqlDbType.VarChar).Value = user.UserName;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = user.Email;
                    cmd.Parameters.Add("@phone", SqlDbType.VarChar).Value = user.Phone;
                    cmd.Parameters.Add("@password", SqlDbType.VarBinary).Value = hashedPassword;
                    cmd.Parameters.Add("@create_date", SqlDbType.DateTime).Value = user.CreateDate;
                    cmd.Parameters.Add("@modify_user", SqlDbType.VarChar).Value = user.ModifyUser;
                    cmd.Parameters.Add("@active", SqlDbType.Bit).Value = user.Active;

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
        /// This gets an active user by username.
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public GarageSaleUser GetActiveGarageSaleUserByUserName(string user_name, byte[] password)
        {
            var user = new GarageSaleUser();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetActiveGarageSaleUserByUserName", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@user_name", SqlDbType.VarChar).Value = user_name;                   
                    cmd.Parameters.Add("@password", SqlDbType.VarBinary).Value = password;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
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
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }
            
            return user;
        }

        /// <summary>
        /// This deletes a garage sale user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteGarageSaleUser(int userId)
        {
            var saveSuccessful = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("DeleteGarageSaleUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@user_id", SqlDbType.VarChar).Value = userId;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }

                saveSuccessful = true;
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return saveSuccessful;
        }

        /// <summary>
        /// This gets an active user by username.
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public GarageSaleUser GetGarageSaleUserByUserName(string user_name)
        {
            var user = new GarageSaleUser();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetGarageSaleUserByUserName", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@user_name", SqlDbType.VarChar).Value = user_name;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DateTime? modifyDate = null;

                        if (reader["modify_date"] != DBNull.Value)
                        {
                            modifyDate = Convert.ToDateTime(reader["modify_date"]);
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
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return user;
        }

        /// <summary>
        /// This gets a user profile by id.
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public GarageSaleUser GetUserProfileInfoById(int user_id)
        {
            var user = new GarageSaleUser();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetUserProfileInfoById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@user_id", SqlDbType.VarChar).Value = user_id;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int addressId = 0;
                        if (reader["address_id"] != DBNull.Value)
                        {
                            addressId = Convert.ToInt32(reader["address_id"]);
                        }

                        var userType = 2;
                        if (reader["user_type_id"] != DBNull.Value)
                        {
                            userType = Convert.ToInt32(reader["user_type_id"]);
                        }

                        var stateId = 0;
                        if (reader["state_id"] != DBNull.Value)
                        {
                            stateId = Convert.ToInt32(reader["state_id"]);
                        }

                        var address = new UserAddress
                        {
                            AddressId = addressId,
                            Address1 = reader["address1"].ToString(),
                            Address2 = reader["address2"].ToString(),
                            City = reader["city"].ToString(),
                            State = reader["state"].ToString(),
                            StateId = stateId,
                            Zip = reader["zip"].ToString()
                        };

                        user.Active = Convert.ToBoolean(reader["active"]);
                        user.Address = address;
                        user.CreateDate = Convert.ToDateTime(reader["create_date"]);
                        user.Email = reader["email"].ToString();
                        user.FirstName = reader["first_name"].ToString();
                        user.LastName = reader["last_name"].ToString();
                        user.Phone = reader["phone"].ToString();
                        user.UserId = Convert.ToInt32(reader["user_id"]);
                        user.UserName = reader["user_name"].ToString();
                        user.ProfilePicLink = reader["profile_pic_link"].ToString();
                        user.UserTypeId = userType;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return user;
        }

        /// <summary>
        /// This saves updates to the garage sale user profile.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserSaveResult SaveGarageSaleUserProfile(GarageSaleUser user)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SaveGarageSaleUserProfile", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = user.UserId;
                    cmd.Parameters.Add("@first_name", SqlDbType.VarChar).Value = user.FirstName;
                    cmd.Parameters.Add("@last_name", SqlDbType.VarChar).Value = user.LastName;
                    cmd.Parameters.Add("@phone", SqlDbType.VarChar).Value = user.Phone;
                    cmd.Parameters.Add("@address1", SqlDbType.VarChar).Value = user.Address.Address1;
                    cmd.Parameters.Add("@address2", SqlDbType.VarChar).Value = user.Address.Address2 ?? string.Empty;
                    cmd.Parameters.Add("@city", SqlDbType.VarChar).Value = user.Address.City;
                    cmd.Parameters.Add("@state", SqlDbType.VarChar).Value = user.Address.StateId;
                    cmd.Parameters.Add("@zip", SqlDbType.VarChar).Value = user.Address.Zip;
                    if (user.ModifyDate != null)
                    {
                        cmd.Parameters.Add("@modify_date", SqlDbType.DateTime).Value = user.ModifyDate;
                    }
                    else
                    {
                        cmd.Parameters.Add("@modify_date", SqlDbType.DateTime).Value = user.CreateDate;
                    }
                    cmd.Parameters.Add("@modify_user", SqlDbType.VarChar).Value = user.ModifyUser;
                    cmd.Parameters.Add("@profile_pic_link", SqlDbType.VarChar).Value = user.ProfilePicLink;

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

        public UserSaveResult SavePasswordResetRequest(PasswordResetRequest request)
        {
            var saveResult = new UserSaveResult();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SavePasswordResetRequest", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@user_name", SqlDbType.VarChar).Value = request.UserName;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = request.Email;
                    cmd.Parameters.Add("@reset_token", SqlDbType.VarChar).Value = request.ResetToken;
                    cmd.Parameters.Add("@request_date_time", SqlDbType.DateTime).Value = request.RequestDateTime;

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

        public UserAddress GetUserAddressByUserId(int user_id)
        {
            var address = new UserAddress();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetUserAddressByUserId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@user_id", SqlDbType.VarChar).Value = user_id;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        address.AddressId = Convert.ToInt32(reader["address_id"]);
                        address.Address1 = reader["address1"].ToString();
                        address.Address2 = reader["address2"].ToString();
                        address.City = reader["city"].ToString();
                        address.State = reader["state_name"].ToString();
                        address.Zip = reader["zip"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return address;
        }

        public PasswordResetRequest GetUserByResetToken(string token)
        {
            var request = new PasswordResetRequest();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetUserByResetToken", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@reset_token", SqlDbType.VarChar).Value = token;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        request.PasswordResetId = Convert.ToInt32(reader["password_reset_id"]);
                        request.UserName = reader["user_name"].ToString();
                        request.Email = reader["email"].ToString();
                        request.ResetToken = reader["reset_token"].ToString();
                        request.RequestDateTime = Convert.ToDateTime(reader["request_date_time"]);

                        var user = new GarageSaleUser
                        {
                            UserId = Convert.ToInt32(reader["user_id"]),
                            UserName = reader["user_name"].ToString(),
                            Email = reader["email"].ToString(),
                            Active = Convert.ToBoolean(reader["active"]),
                            CreateDate = Convert.ToDateTime(reader["create_date"]),
                            FirstName = reader["first_name"].ToString(),
                            LastName = reader["last_name"].ToString(),
                            Phone = reader["phone"].ToString(),
                            ProfilePicLink = reader["profile_pic_link"].ToString()
                        };

                        request.User = user;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return request;
        }

        public bool UpdateUserPassword(string userName, byte[] password)
        {
            var saveSuccessful = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("UpdateUserPassword", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@user_name", SqlDbType.VarChar).Value = userName;
                    cmd.Parameters.Add("@password", SqlDbType.VarBinary).Value = password;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    saveSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return saveSuccessful;
        }
    }
}