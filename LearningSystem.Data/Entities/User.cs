using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static LearningSystem.Data.DataConstants;

namespace LearningSystem.Data.Entities
{
    public class User : IdentityUser<string>
    {
        [Required]
        [MinLength(UserNameMinLength)]
        [MaxLength(UserNameMaxLength)]
        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public ICollection<StudentCourse> Courses { get; set; } = new List<StudentCourse>();

        public ICollection<Course> Trainings { get; set; } = new List<Course>();

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
