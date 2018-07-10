using LearningSystem.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Data.EF
{
    public class LearningSystemDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public LearningSystemDbContext(DbContextOptions<LearningSystemDbContext> options)
            :base(options)
        {
        }
    }
}
