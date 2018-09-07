using System.Collections.Generic;
using LearningSystem.Services.Models;

namespace LearningSystem.Web.Models.Trainers
{
    public class StudentsInCourseViewModel
    {
        public IEnumerable<StudentInCourseServiceModel> Students { get; set; }

        public CourseListingServiceModel Course { get; set; }
    }
}