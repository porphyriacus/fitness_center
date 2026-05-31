using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class WorkoutsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
