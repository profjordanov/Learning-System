using System.Collections.Generic;
using LearningSystem.Services.Models;

namespace LearningSystem.Web.Models.HomeModels
{
    public class SearchViewModel
    {
        public IEnumerable<CourseListingServiceModel> Courses { get; set; }
            = new List<CourseListingServiceModel>();

        public IEnumerable<UserListingServiceModel> Users { get; set; }
            = new List<UserListingServiceModel>();

        public string SearchText { get; set; }
    }
}