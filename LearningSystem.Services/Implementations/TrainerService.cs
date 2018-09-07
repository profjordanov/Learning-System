using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using LearningSystem.Data.EF;
using LearningSystem.Data.Entities;
using LearningSystem.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Services.Implementations
{
    public class TrainerService : ITrainerService
    {
        private readonly LearningSystemDbContext _dbContext;

        public TrainerService(LearningSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CourseListingServiceModel>> CoursesAsync(string trainerId)
            => await _dbContext
                .Courses
                .Where(c => c.TrainerId == trainerId)
                .ProjectTo<CourseListingServiceModel>()
                .ToListAsync();

        public async Task<bool> IsTrainer(int courseId, string trainerId)
            => await _dbContext
                .Courses
                .AnyAsync(c => c.Id == courseId
                               && c.TrainerId == trainerId);

        public async Task<IEnumerable<StudentInCourseServiceModel>> StudentsInCourseAsync(int courseId)
            => await _dbContext
                .Courses
                .Where(c => c.Id == courseId)
                .SelectMany(c => c.Students.Select(s => s.Student))
                .ProjectTo<StudentInCourseServiceModel>(new { courseId })
                .ToListAsync();

        public async Task<bool> AddGradeAsync(int courseId, string studentId, Grade grade)
        {
            var studentInCourse = await _dbContext
                .FindAsync<StudentCourse>(studentId, courseId); // NB keys order!

            if (studentInCourse == null)
            {
                return false;
            }

            studentInCourse.Grade = grade;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<byte[]> GetExamSubmission(int courseId, string studentId)
        {
            var studentInCourse = await _dbContext
                .FindAsync<StudentCourse>(studentId, courseId); // NB keys order!

            return studentInCourse?.ExamSubmission;
        }

        public async Task<StudentInCourseNamesServiceModel> StudentInCourseNamesAsync(int courseId, string studentId)
        {
            var studentUsername = await _dbContext.Users
                .Where(u => u.Id == studentId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync();

            if (studentUsername == null)
            {
                return null;
            }

            var courseName = await _dbContext.Courses
                .Where(c => c.Id == courseId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

            if (courseName == null)
            {
                return null;
            }

            return new StudentInCourseNamesServiceModel
            {
                UserName = studentUsername,
                CourseName = courseName
            };
        }
    }
}