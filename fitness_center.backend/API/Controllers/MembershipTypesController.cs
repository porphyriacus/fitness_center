using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MembershipTypesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
