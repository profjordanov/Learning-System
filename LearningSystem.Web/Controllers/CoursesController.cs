using System.Threading.Tasks;
using LearningSystem.Data;
using LearningSystem.Data.Entities;
using LearningSystem.Services;
using LearningSystem.Services.Models;
using LearningSystem.Web.Infrastructure.Extensions;
using LearningSystem.Web.Models.CoursesViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearningSystem.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly UserManager<User> _userManager;

        public CoursesController(ICourseService courseService, UserManager<User> userManager)
        {
            _courseService = courseService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = new CourseDetailsViewModel
            {
                Course = await _courseService.ByIdAsync<CourseDetailsServiceModel>(id)
            };

            if (model.Course == null)
                return NotFound();

            if (this.User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);

                model.IsUserEnrolledInCourse = await _courseService.IsUserSignedInCourseAsync(id, userId);
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignUpStudent(int id)
        {
            var userId =  _userManager.GetUserId(User);
            var success = await _courseService.SignUpStudentAsync(id, userId);

            if (!success)
                return BadRequest();

            TempData.AddSuccessMessage("You successfully enrolled in this course.");

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignOutStudent(int id)
        {
            var userId = _userManager.GetUserId(User);
            var success = await _courseService.SignOutStudentAsync(id, userId);

            if (!success)
            {
                return BadRequest();
            }

            this.TempData.AddSuccessMessage("You successfully signed out of this course.");

            return this.RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitExam(int id, IFormFile exam)
        {
            if (!exam.FileName.EndsWith(".zip") ||
                exam.Length > DataConstants.MaxFileLength)
            {
                TempData.AddErrorMessage("Exam submission should be a zip file with a max lendth of 2 MB!");
                return RedirectToAction(nameof(Details), new { id });
            }

            var fileContents = await exam.ToByteArrayAsync();
            var userId = _userManager.GetUserId(User);

            var success = await _courseService.SaveExamSubmission(id, userId, fileContents);
            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Exam submission saved successfully");
            return this.RedirectToAction(nameof(Details), new { id });
        }
    }
}