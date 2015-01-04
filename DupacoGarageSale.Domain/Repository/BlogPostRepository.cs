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
    public class BlogPostRepository
    {
        public List<BlogPost> GetBlogPosts(int saleId)
        {
            var blogPosts = new List<BlogPost>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("GetBlogPostsBySaleId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sale_id", SqlDbType.Int).Value = saleId;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var post = new BlogPost
                        {
                            BlogPostId = Convert.ToInt32(reader["blog_post_id"]),
                            BlogPostTitle = reader["blog_post_title"].ToString(),
                            BlogPostUser = reader["blog_post_user_id"].ToString(),
                            ImageUri = reader["image_uri"].ToString(),
                            MediaTypeId = Convert.ToInt16(reader["media_type_id"]),
                            PostDateTime = Convert.ToDateTime(reader["blog_post_date_time"]),
                            PostMessage = reader["post_message"].ToString(),
                            SaleId = Convert.ToInt16(reader["sale_id"]),
                            VineUri = reader["vine_uri"].ToString(),
                            YouTubeUri = reader["youtube_uri"].ToString()
                        };

                        blogPosts.Add(post);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return blogPosts;
        }

        /// <summary>
        /// This saves a new blog post.
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public UserSaveResult SaveBlogPost(BlogPost post)
        {
            var saveResult = new UserSaveResult();
            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("SaveBlogPosts", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@blog_post_title", SqlDbType.VarChar).Value = post.BlogPostTitle;
                    cmd.Parameters.Add("@media_type_id", SqlDbType.Int).Value = post.MediaTypeId;
                    cmd.Parameters.Add("@image_uri", SqlDbType.VarChar).Value = post.ImageUri ?? string.Empty;
                    cmd.Parameters.Add("@youtube_uri", SqlDbType.VarChar).Value = post.YouTubeUri ?? string.Empty;
                    cmd.Parameters.Add("@vine_uri", SqlDbType.VarChar).Value = post.VineUri ?? string.Empty;
                    cmd.Parameters.Add("@post_message", SqlDbType.VarChar).Value = post.PostMessage;
                    cmd.Parameters.Add("@sale_id", SqlDbType.Int).Value = post.SaleId;
                    cmd.Parameters.Add("@blog_post_date_time", SqlDbType.DateTime).Value = DateTime.Now;

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
                Console.WriteLine(ex.ToString());
            }

            return saveResult;
        }

        /// <summary>
        /// This deletes a blog post.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteBlogPost(int id)
        {
            var saveSuccessful = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DupacoGarageSale"]))
                using (SqlCommand cmd = new SqlCommand("DeleteBlogPost", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@blog_post_id", SqlDbType.Int).Value = id;
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
    }
}
