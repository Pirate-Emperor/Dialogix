using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Dialogix.Models;
using System.Text;

namespace Dialogix.Controllers.Firebase
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : ControllerBase
    {

        private readonly HttpClient _httpClient;
        private readonly string _serviceAccountKeyPath = "D:/.NET/Dialogix/Dialogix/wwwroot/Firebase Secret File/dialogix-pirate-emperor-firebase-adminsdk-elzcb-c844b026a9.json";


        public FirebaseController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Test()
        {
            string success = "";
            return Ok(new { success = "AiSuccess" });
        }


        [HttpPost("SendNotificationAsync")]
        public async Task<IActionResult> SendNotificationAsync([FromBody] UserPendingNotification notification)
        {
            string msg = "";
            bool res = false;
            try
            {


                var googleCredential = GoogleCredential.FromFile(_serviceAccountKeyPath).CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

                var accessToken = await googleCredential.UnderlyingCredential.GetAccessTokenForRequestAsync();


                var firebaseUrl = "https://fcm.googleapis.com/v1/projects/dialogix-pirate-emperor/messages:send";


                var fcmMessage = new
                {
                    message = new
                    {
                        token = notification.FCMToken,
                        notification = new
                        {
                            title = notification.Message,
                            body = notification.Message
                        }
                    }
                };


                string jsonMessage = JsonConvert.SerializeObject(fcmMessage);
                var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");


                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);


                var response = await _httpClient.PostAsync(firebaseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    res = true;
                    msg = "Notification sent successfully.";
                    Console.WriteLine("Notification sent successfully.");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    res = false;
                    msg = error;
                    Console.WriteLine($"Failed to send notification. Error: {error}");
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                res = false;
            }
            return Ok(new { msg = msg, res = res });
        }

    }
}


