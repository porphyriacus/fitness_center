using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TrainersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
