using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dialogix.Models;
using System.Data.SqlClient;
using static Dialogix.Models.ChatUser;

namespace Dialogix.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginAPIController : ControllerBase
    {
        private readonly string _connectionString;

        public LoginAPIController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CustomConnection");
        }



        [HttpPost]
        public IActionResult Register([FromBody] ChatUser pChatUser)
        {
            bool res = false;
            string msg = "";
            int newUserId = 0;
            string Username = pChatUser.UserName;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_ChatUser", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", 1);
                        cmd.Parameters.AddWithValue("@UserName", pChatUser.UserName);
                        cmd.Parameters.AddWithValue("@Password", pChatUser.Password);
                        cmd.Parameters.AddWithValue("@Email", pChatUser.Email);
                        cmd.Parameters.AddWithValue("@Gender", pChatUser.Gender);
                        cmd.Parameters.AddWithValue("@ProfilePictureURL", pChatUser.HdnProfilePicture);
                        cmd.Parameters.AddWithValue("@FullName", pChatUser.FullName);
                        cmd.Parameters.AddWithValue("@FCMToken", pChatUser.FCMToken);

                        SqlParameter outputIdParam = new SqlParameter("@ChatUser", System.Data.SqlDbType.Int)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdParam);


                        cmd.ExecuteNonQuery();

                        newUserId = (int)outputIdParam.Value;
                    }
                }
                res = true;
                msg = "User Created Successfully";
            }
            catch (Exception ex)
            {
                res = false;
                msg = ex.Message;
            }
            return Ok(new { response = res, message = msg, userId = newUserId, username = Username });
        }

        #region "Login"
        [HttpPost]
        public IActionResult Login([FromBody] ChatUser pChatUser)
        {
            bool res = false;
            string msg = "";
            ChatUser obj = new ChatUser();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_ChatUser", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 2);
                    cmd.Parameters.AddWithValue("@UserName", pChatUser.UserName);
                    cmd.Parameters.AddWithValue("@Password", pChatUser.Password);
                    cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gender", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ProfilePictureURL", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ChatUser", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FullName", DBNull.Value);


                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            obj.ChatUserID = Convert.ToInt32(rdr["ChatUserID"]);
                            obj.UserName = Convert.ToString(rdr["UserName"]);
                            obj.HdnProfilePicture = Convert.ToString(rdr["ProfilePictureURL"]);
                        }
                        res = true;
                        msg = "User Login Successfully";

                        if (obj.ChatUserID == 0 || obj.ChatUserID == null)
                        {
                            res = false;
                            msg = "Please check your Username or Password";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res = false;
                msg = ex.Message;
            }
            return Ok(new { response = res, message = msg, obj = obj });
        }

        #endregion



    }
}
