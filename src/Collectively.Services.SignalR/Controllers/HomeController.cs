using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace Collectively.Services.SignalR.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Get()
            => Content("Welcome to the Collectively.Services.SignalR API!");
    }
}