using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSystem.Data.Entities;
using LearningSystem.Services;
using LearningSystem.Services.Models;
using LearningSystem.Web.Models.Trainers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearningSystem.Web.Controllers
{
    public class TrainersController : Controller
    {
        private const string DateTimeFormat = "yyyy-MM-dd";
        private readonly ITrainerService _trainerService;
        private readonly ICourseService _courseService;
        private readonly UserManager<User> _userManager;

        public TrainersController(
            ITrainerService trainerService, 
            ICourseService courseService, 
            UserManager<User> userManager)
        {
            _trainerService = trainerService;
            _courseService = courseService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Courses()
        {
            var userId = _userManager.GetUserId(this.User);
            var courses = await _trainerService.CoursesAsync(userId);

            return View(courses);
        }

        public async Task<IActionResult> Students(int id)
        {
            var userId = _userManager.GetUserId(this.User);
            if (!await _trainerService.IsTrainer(id, userId))
            {
                return NotFound();
            }

            var studentsInCourseViewModel = new StudentsInCourseViewModel
            {
                Students = await _trainerService.StudentsInCourseAsync(id),
                Course = await _courseService.ByIdAsync<CourseListingServiceModel>(id)
            };

            return View(studentsInCourseViewModel);
        }

        public async Task<IActionResult> DownloadExam(int id, string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(this.User);
            if (!await _trainerService.IsTrainer(id, userId))
            {
                return BadRequest();
            }

            var studentInCourseNames = await _trainerService.StudentInCourseNamesAsync(id, studentId);
            if (studentInCourseNames == null)
            {
                return BadRequest();
            }

            var examContents = await _trainerService.GetExamSubmission(id, studentId);
            if (examContents == null)
            {
                return BadRequest("No exam submitted.");
            }

            return File(examContents, "application/zip", $"{studentInCourseNames.CourseName}-{studentInCourseNames.UserName}-{DateTime.UtcNow.ToString(DateTimeFormat)}.zip");
        }

        [HttpPost]
        public async Task<IActionResult> GradeStudent(int id, string studentId, Grade grade)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(this.User);
            if (!await _trainerService.IsTrainer(id, userId))
            {
                return BadRequest();
            }

            var success = await _trainerService.AddGradeAsync(id, studentId, grade);
            if (!success)
            {
                return BadRequest();
            }

            return this.RedirectToAction(nameof(Students), new { id });
        }
    }
}