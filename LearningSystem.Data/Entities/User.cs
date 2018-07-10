using System;
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
    }
}
