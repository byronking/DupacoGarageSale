﻿using DupacoGarageSale.Data.Domain;
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
                Console.Write(ex.ToString());
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
                Console.Write(ex.ToString());
            }
            
            return user;
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
                Console.Write(ex.ToString());
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

                        var address = new UserAddress
                        {
                            AddressId = addressId,
                            Address1 = reader["address1"].ToString(),
                            Address2 = reader["address2"].ToString(),
                            City = reader["city"].ToString(),
                            State = reader["state"].ToString(),
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
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
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
                    cmd.Parameters.Add("@state", SqlDbType.VarChar).Value = user.Address.State;
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
                Console.Write(ex.ToString());
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
                Console.Write(ex.ToString());
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
                Console.Write(ex.ToString());
            }

            return address;
        }
    }
}