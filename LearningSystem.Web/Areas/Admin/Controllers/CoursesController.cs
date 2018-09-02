using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSystem.Data.Entities;
using LearningSystem.Services.Admin;
using LearningSystem.Web.Areas.Admin.Models.Courses;
using LearningSystem.Web.Controllers;
using LearningSystem.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearningSystem.Web.Areas.Admin.Controllers
{
    public class CoursesController : BaseAdminController
    {
        private readonly UserManager<User> _userManager;
        private readonly IAdminCourseService _adminCourseService;

        public CoursesController(UserManager<User> userManager, IAdminCourseService adminCourseService)
        {
            _userManager = userManager;
            _adminCourseService = adminCourseService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new AddCourseFormModel
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(20),
                Trainers = await GetTrainers()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddCourseFormModel model)
        {
            var user = await _userManager.FindByIdAsync(model.TrainerId);
            if (user == null)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid user.");
            }

            if (!ModelState.IsValid)
            {
                model.Trainers = await GetTrainers();
                return View(model);
            }

            await _adminCourseService.Create(
                model.Name,
                model.Description,
                model.StartDate,
                model.EndDate,
                model.TrainerId);

            this.TempData.AddSuccessMessage("Course created successfully.");

            return this.RedirectToAction(
                nameof(HomeController.Index),
                "Home",
                routeValues: new { area = string.Empty });
        }

        private async Task<IEnumerable<SelectListItem>> GetTrainers()
        {
            var trainers = await _userManager.GetUsersInRoleAsync(WebConstants.TrainerRole);
            return trainers
                .Select(t => new SelectListItem
                {
                    Text = $"{t.Name} ({t.UserName})",
                    Value = t.Id
                })
                .OrderBy(t => t.Text)
                .ToList();
        }
    }
}