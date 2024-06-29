using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Dialogix.AppCode;
using Dialogix.Models;
using System.Data.SqlTypes;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using System.Text;

namespace Dialogix.Controllers
{
    [ServiceFilter(typeof(RedirectIfLoggedInAttribute))]
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;
        IHttpContextAccessor _httpContextAccessor;
        private readonly dynamic baseUrl;

        private readonly ISessionService _sessionService;
        public LoginController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ISessionService sessionService)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _sessionService = sessionService;
            var request = _httpContextAccessor.HttpContext.Request;
            baseUrl = $"{request.Scheme}://{request.Host.Value}/"; _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public IActionResult Index()
        {
            return View();
        }

        #region "Login"
        [HttpGet]
        public IActionResult Login()
        {
            var Username = _sessionService.GetString("UserName");
            var UserID = _sessionService.GetInt32("UserID");

            if (!string.IsNullOrWhiteSpace(Username) || UserID != null)
            {
                var actionPath = TempData["RequestPath"];

                return RedirectToAction("actionPath");
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Login(ChatUser pChatUser)
        {
            if (pChatUser.UserName == null)
            {
                ViewBag.ErrorMessage = "Please enter the UserName";
                return View();
            }
            if (pChatUser.Password == null)
            {
                ViewBag.ErrorMessage = "Please enter the Password";
                return View();
            }

            string url = baseUrl + "api/LoginAPI/Login";
            string Json = JsonConvert.SerializeObject(pChatUser);
            StringContent content = new StringContent(Json, Encoding.UTF8, "application/json");

            HttpResponseMessage res = await _httpClient.PostAsync(url, content);

            if (res.IsSuccessStatusCode)
            {
                dynamic resBody = await res.Content.ReadAsStringAsync();
                dynamic resData = JsonConvert.DeserializeObject(resBody);

                if (resData.response == true)
                {
                    string Message = resData.message;
                    dynamic data = resData.obj;


                    int UserId = Convert.ToInt32(data.chatUserID);
                    string Username = data.userName;
                    _sessionService.SetInt32("UserID", UserId);
                    _sessionService.SetString("UserName", Username);
                    TempData["successMessage"] = Message;
                    return RedirectToAction("ChatBox", "Chat");
                }
                else
                {
                    string Message = resData.message;
                    TempData["errorMessage"] = Message;
                    return View("Login");
                }
            }



            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register(ChatUser pChatUser)
        {
            return View(pChatUser);
        }
        [HttpPost]
        public async Task<IActionResult> Register(ChatUser pChatUser, IFormFile ProfilePictureURL, string ProfilePictureURLCamera)
        {
            if (!string.IsNullOrEmpty(ProfilePictureURLCamera))
            {
                string base64Data = ProfilePictureURLCamera.Replace("data:image/jpeg;base64,", "");

                byte[] imageBytes = Convert.FromBase64String(base64Data);

                string filePath = Path.Combine("wwwroot", "images", "uploads");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string DateTimeCam = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string fileName = "ProfileCam" + DateTimeCam + ".jpg";
                string directoryPath = Path.Combine(filePath, fileName);

                System.IO.File.WriteAllBytes(directoryPath, imageBytes);
                pChatUser.HdnProfilePicture = fileName;
            }

            if (ProfilePictureURL != null)
            {
                string FileName = Path.GetFileNameWithoutExtension(ProfilePictureURL.FileName);

                string FileData = Path.Combine("wwwroot", "images", "Uploads");

                if (!Directory.Exists(FileData))
                {
                    Directory.CreateDirectory(FileData);
                }
                string DateTimePhoto = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string fileNameWithDate = $"{FileName}_{DateTimePhoto}.jpg";
                string FilePath = Path.Combine(FileData, fileNameWithDate);
                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    await ProfilePictureURL.CopyToAsync(stream);
                }

                pChatUser.HdnProfilePicture = fileNameWithDate;

                pChatUser.ProfilePictureURL = null;
            }


            string url = baseUrl + "api/LoginAPI/Register";

            string Json = JsonConvert.SerializeObject(pChatUser);
            StringContent content = new StringContent(Json, Encoding.UTF8, "application/json");

            HttpResponseMessage res = await _httpClient.PostAsync(url, content);
            if (res.IsSuccessStatusCode)
            {

                dynamic resData = await res.Content.ReadAsStringAsync();
                dynamic resBody = JsonConvert.DeserializeObject(resData);
                if (resBody.response == true)
                {
                    string Message = resBody.message;

                    int UserId = Convert.ToInt32(resBody.userId);
                    string Username = resBody.username;
                    _sessionService.SetInt32("UserID", UserId);
                    _sessionService.SetString("UserName", Username);

                    TempData["successMessage"] = Message;
                    TempData.Keep("successMessage");
                    return RedirectToAction("ChatBox", "Chat");
                }
                else
                {
                    string Message = resBody.message;
                    TempData["errorMessage"] = Message;
                    return RedirectToAction("Register", pChatUser);
                }

            }
            return RedirectToAction("Register", "Login");
        }


        [HttpGet]
        public PartialViewResult _partialTermsCondition(string termsValueCheck = "")
        {

            ViewBag.HdnTermsCondition = termsValueCheck; ;
            return PartialView();
        }

        [HttpPost]
        public JsonResult TermsCondition(bool boolTerms)
        {
            if (boolTerms == true)
            {
                ChatUser obj = new ChatUser();
                obj.TermsCondition = boolTerms;
                return Json(new { boolTerms });
            }
            return Json(new { boolTerms });
        }



        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
        #endregion
    }

    //To Check User is Already Login or not
    public class RedirectIfLoggedInAttribute : ActionFilterAttribute
    {
        private readonly ISessionService _sessionService;
        public RedirectIfLoggedInAttribute(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var username = context.HttpContext.Session.GetString("UserName");
            var userId = context.HttpContext.Session.GetInt32("UserID");
            var requestedPath = _sessionService.GetString("ActionPath");

            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (actionDescriptor != null && actionDescriptor.ActionName == "LogOut" && !string.IsNullOrEmpty(username) && userId != null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(requestedPath))
            {
                context.Result = new RedirectResult(requestedPath);
            }
        }
    }
}


