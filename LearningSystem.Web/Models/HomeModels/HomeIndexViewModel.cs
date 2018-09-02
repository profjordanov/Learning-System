using System.Collections.Generic;
using LearningSystem.Services.Models;

namespace LearningSystem.Web.Models.HomeModels
{
    public class HomeIndexViewModel : SearchFormModel
    {
        public IEnumerable<CourseListingServiceModel> Courses { get; set; }
    }
}