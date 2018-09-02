using System;
using System.Threading.Tasks;
using LearningSystem.Data.EF;
using LearningSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Services.Admin.Implementations
{
    public class AdminCourseService : IAdminCourseService
    {
        private readonly LearningSystemDbContext _dbContext;

        public AdminCourseService(LearningSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(string name, string description, DateTime startDate, DateTime endDate, string trainerId)
        {
            var trainerExists = await _dbContext.Users.AnyAsync(u => u.Id == trainerId);
            if (!trainerExists)
                return;

            var course = new Course
            {
                Name = name,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                TrainerId = trainerId
            };

            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();
        }
    }
}