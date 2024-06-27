using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using NuGet.Common;
using Dialogix.AppCode;
using Dialogix.Hubs;
using Dialogix.Models;
using System.Drawing;
using System.Text;

namespace Dialogix.Controllers
{
    [ServiceFilter(typeof(SessionAdmin))]
    public class ChatController : Controller
    {
        private readonly HttpClient _httpClient;
        IHttpContextAccessor _httpContextAccessor;
        private readonly dynamic baseUrl;
        private readonly IChatHub _signalRHub;

        private readonly ISessionService _sessionService;
        public ChatController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ISessionService sessionService, IChatHub signalRHub)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _sessionService = sessionService;
            _signalRHub = signalRHub;
            var request = _httpContextAccessor.HttpContext.Request;
            baseUrl = $"{request.Scheme}://{request.Host.Value}/"; _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public IActionResult Index()
        {
            return View();
        }



        #region "ChatBox"
        [HttpGet]
        public async Task<IActionResult> ChatBox()
        {

            var actionPath = HttpContext.Request.Path;
            _sessionService.SetString("ActionPath", actionPath);
            string message = "";
            try
            {
                var UserId = _sessionService.GetInt32("UserID");

                string url = baseUrl + "api/ChatAPI/LoadChatHistory";
                ChatMessage pChatMessage = new ChatMessage();

                pChatMessage.ChatMessageID = UserId;
                string Json = JsonConvert.SerializeObject(pChatMessage);
                StringContent content = new StringContent(Json, Encoding.UTF8, "application/json");

                HttpResponseMessage res = await _httpClient.PostAsync(url, content);

                if (res.IsSuccessStatusCode)
                {
                    dynamic resBody = await res.Content.ReadAsStringAsync();
                    List<ChatMessage> lstChatMessage = JsonConvert.DeserializeObject<List<ChatMessage>>(resBody);
                    ViewBag.lstChatMessage = lstChatMessage;
                    return View();
                }
                else
                {
                    return RedirectToAction("", "");
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return RedirectToAction("", "");
            }
        }


        [HttpGet]
        public async Task<IActionResult> ChatMessage(int Id = 0)
        {
            string message = "";
            bool response = false;
            var actionPath = HttpContext.Request.Path;
            _sessionService.SetString("ActionPath", actionPath);

            try
            {
                if (Id == 0)
                {
                    TempData["errorMessage"] = "Please select the User to Send Message";
                    return RedirectToAction("ChatBox", "Chat");
                }

                string url = baseUrl + $"api/ChatAPI/GetProfile";
                string fullUrl = url + $"?Id={Id}";
                HttpResponseMessage res = await _httpClient.GetAsync(fullUrl);
                if (res.IsSuccessStatusCode)
                {
                    dynamic resBody = await res.Content.ReadAsStringAsync();
                    ChatMessage obj = JsonConvert.DeserializeObject<ChatMessage>(resBody);


                    string chatRecordURL = baseUrl + "api/ChatAPI/ChatHistoryRecord";

                    ChatMessage pChatMessage = new ChatMessage();
                    var chatUserId = _sessionService.GetInt32("UserID");
                    pChatMessage.ChatMessageID = chatUserId;
                    pChatMessage.ChatReceiverID = Id;
                    string JSON = JsonConvert.SerializeObject(pChatMessage);
                    StringContent content = new StringContent(JSON, Encoding.UTF8, "application/json");

                    HttpResponseMessage chatResponse = await _httpClient.PostAsync(chatRecordURL, content);

                    if (chatResponse.IsSuccessStatusCode)
                    {
                        dynamic chatResData = await chatResponse.Content.ReadAsStringAsync();
                        List<ChatMessage> lstChatRecord = JsonConvert.DeserializeObject<List<ChatMessage>>(chatResData);
                        ViewBag.lstChatRecordConnections = lstChatRecord;
                        ViewBag.UserLoginID = chatUserId;
                    }

                    return View(obj);
                }
                else
                {
                    message = "Error in the API Call or to Fetch the Data";
                }
            }
            catch (Exception ex)
            {
                response = false;
                message = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchConnections(ChatMessage pChatMessage)
        {
            string message = "";
            try
            {
                if (pChatMessage.SearchConnection == null)
                {
                    TempData["errorMessage"] = "Invalid, Please enter the UserName";
                    TempData.Keep("errorMessage");
                    return RedirectToAction("ChatBox");
                }
                string url = baseUrl + "api/ChatAPI/SearchConnections";
                string Json = JsonConvert.SerializeObject(pChatMessage);
                StringContent content = new StringContent(Json, Encoding.UTF8, "application/json");
                HttpResponseMessage res = await _httpClient.PostAsync(url, content);
                if (res.IsSuccessStatusCode)
                {
                    dynamic resBody = await res.Content.ReadAsStringAsync();
                    dynamic resData = JsonConvert.DeserializeObject(resBody);
                    if (resData != null)
                    {
                        List<ChatMessage> lstChat = JsonConvert.DeserializeObject<List<ChatMessage>>(resBody);
                        return Ok(new { lstChat });
                    }
                    else
                    {
                        return Ok(new { message = "No data available" });
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return RedirectToAction("ChatBox", "Chat");

        }

        [HttpGet]
        public async Task<IActionResult> SendRequest(int Id = 0)
        {
            bool res = false;
            string msg = "";
            string FCMToken = "";
            string resMessage = "";
            try
            {
                if (Id == 0)
                {
                    TempData["errorMessage"] = "Please select a user to send the connection request.";
                    return RedirectToAction("ChatBox", "Chat");
                }

                var loginUserId = _sessionService.GetInt32("UserID");
                string url = baseUrl + "api/ChatAPI/RequestConnection";

                UserConnection obj = new UserConnection
                {
                    RequestID = loginUserId, //Current Login
                    AcceptID = Id
                };

                string JSON = JsonConvert.SerializeObject(obj);
                StringContent content = new StringContent(JSON, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    dynamic resBody = await response.Content.ReadAsStringAsync();
                    dynamic resData = JsonConvert.DeserializeObject<dynamic>(resBody);

                    if (resData.fcmToken != null)
                    {
                        FCMToken = resData.fcmToken;
                    }

                    if (resData.res == false)
                    {
                        msg = resBody.msg;
                        return RedirectToAction("ChatBox", "Chat");
                    }
                    string dbSaveUrl = baseUrl + "api/Firebase/SendNotificationAsync";
                    UserPendingNotification pUserPendingNotification = new UserPendingNotification
                    {
                        LoginUserID = Id,
                        Message = $"{loginUserId} has sent you a connection request.",
                        CreatedDate = DateTime.UtcNow,
                        FCMToken = FCMToken
                    };

                    string jsonContent = JsonConvert.SerializeObject(pUserPendingNotification);
                    StringContent contentData = new StringContent(jsonContent, Encoding.UTF8, "application/json");


                    HttpResponseMessage resDB = await _httpClient.PostAsync(dbSaveUrl, contentData);

                    if (resDB.IsSuccessStatusCode)
                    {
                        dynamic resNotification = await resDB.Content.ReadAsStringAsync();
                        dynamic resNotificationData = JsonConvert.DeserializeObject(resNotification);

                        string strSaveNotificationURL = baseUrl + "api/ChatAPI/NotificationMessage";


                        SendNotificationMessage pSendNotificationMessage = new SendNotificationMessage
                        {
                            SenderID = Convert.ToInt32(loginUserId),
                            ReceiverID = Id,
                            MessageData = resNotificationData.msg,
                            IsNotificationSend = resNotificationData.res,
                        };

                        string saveNotification = JsonConvert.SerializeObject(pSendNotificationMessage);
                        StringContent saveNotificationContent = new StringContent(saveNotification, Encoding.UTF8, "application/json");


                        HttpResponseMessage resSaveNotification = await _httpClient.PostAsync(strSaveNotificationURL, saveNotificationContent);

                        if (resSaveNotification.IsSuccessStatusCode)
                        {

                            dynamic SaveNotificationResponse = await resSaveNotification.Content.ReadAsStringAsync();
                            dynamic SaveNotificationBody = JsonConvert.DeserializeObject(SaveNotificationResponse);

                            if (resNotificationData.res == true)
                            {
                                resMessage = resNotificationData.msg;
                                TempData["successmessage"] = resMessage;
                            }
                            if (resNotificationData.res == false)
                            {
                                resMessage = resNotificationData.msg;
                                TempData["errorMessage"] = resMessage;
                            }

                            return RedirectToAction("ChatBox", "Chat");
                        }

                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to send the connection request. Please try again.";
                }

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("ChatBox", "Chat");
        }

        #endregion
    }
    public class SessionAdmin : ActionFilterAttribute
    {
        private readonly ISessionService _sessionService;

        public SessionAdmin(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int? UserId = _sessionService.GetInt32("UserID");
            string Username = _sessionService.GetString("UserName");
            if (string.IsNullOrEmpty(Username) || UserId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
