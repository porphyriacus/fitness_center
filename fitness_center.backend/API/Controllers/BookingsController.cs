using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BookingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
