using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Dialogix.AppCode;

namespace Dialogix.Controllers
{
    [ServiceFilter(typeof(SessionAdmin))]
    public class ConnectionsController : Controller
    {

        private readonly HttpClient _httpClient;
        IHttpContextAccessor _httpContextAccessor;
        private readonly dynamic baseUrl;

        private readonly ISessionService _sessionService;
        public ConnectionsController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ISessionService sessionService)
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

        [HttpGet]
        public async Task<IActionResult> ConnectionList()
        {
            bool res = false;
            string msg = "";
            try
            {
                //string url = baseUrl + "";
                //HttpResponseMessage apiRes = await _httpClient.GetAsync(url);

                //if (apiRes.IsSuccessStatusCode)
                //{

                //}


                return PartialView();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                res = false;
                return RedirectToAction("", "");
            }

        }


    }
}
