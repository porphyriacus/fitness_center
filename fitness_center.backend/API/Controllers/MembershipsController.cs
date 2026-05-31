using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MembershipsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
