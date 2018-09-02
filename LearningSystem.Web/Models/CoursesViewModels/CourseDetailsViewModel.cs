using LearningSystem.Services.Models;
namespace LearningSystem.Web.Models.CoursesViewModels
{
    public class CourseDetailsViewModel
    {
        public CourseDetailsServiceModel Course { get; set; }

        public bool IsUserEnrolledInCourse { get; set; }
    }
}