using System.Threading.Tasks;
using LearningSystem.Data.Entities;
using LearningSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearningSystem.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public UsersController(IUserService userService, UserManager<User> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Profile(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            var profile = await _userService.ProfileAsync(user.Id);

            return View(profile);
        }

        public async Task<IActionResult> DownloadCertificate(int id)
        {
            var userId = _userManager.GetUserId(User);

            var certificateContents = await _userService.GetPdfCertificate(id, userId);
            if (certificateContents == null)
            {
                return BadRequest();
            }

            return File(certificateContents, "application/pdf", "Certificate.pdf");
        }

    }
}