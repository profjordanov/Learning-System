using Microsoft.AspNetCore.Mvc;

namespace LearningSystem.Web.Controllers
{
    public class UsersController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}