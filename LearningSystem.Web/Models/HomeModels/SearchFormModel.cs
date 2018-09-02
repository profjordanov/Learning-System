using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Web.Models.HomeModels
{
    public class SearchFormModel
    {
        public string SearchText { get; set; }

        [Display(Name = "Search In Users")]
        public bool SearchInUsers { get; set; } = true;

        [Display(Name = "Search In Courses")]
        public bool SearchInCourses { get; set; } = true;
    }
}