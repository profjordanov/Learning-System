using System;
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
    public class CourseService : ICourseService
    {
        private readonly LearningSystemDbContext _dbContext;

        public CourseService(LearningSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CourseListingServiceModel>> AllActiveAsync()
            => await _dbContext
                .Courses
                .Where(c => c.StartDate >= DateTime.UtcNow.Date)
                .OrderByDescending(c => c.StartDate)
                .ThenBy(c => c.Name)
                .ProjectTo<CourseListingServiceModel>()
                .ToListAsync();

        public async Task<IEnumerable<CourseListingServiceModel>> FindAsync(string searchText)
            => await _dbContext
                .Courses
                .OrderByDescending(c => c.Id)
                .Where(c => c.Name.ToLower().Contains((searchText ?? string.Empty).ToLower()))
                .ProjectTo<CourseListingServiceModel>()
                .ToListAsync();

        public async Task<TModel> ByIdAsync<TModel>(int id) 
            where TModel : class
            => await _dbContext
                .Courses
                .Where(c => c.Id == id)
                .ProjectTo<TModel>()
                .FirstOrDefaultAsync();

        public async Task<bool> IsUserSignedInCourseAsync(int courseId, string userId)
            => await _dbContext
                .Courses
                .AnyAsync(c => c.Id == courseId &&
                               c.Students.Any(s => s.StudentId == userId));

        public async Task<bool> SignUpStudentAsync(int courseId, string userId)
        {
            var courseInfo = await GetCourseInfo(courseId, userId);

            if (courseInfo == null ||
                courseInfo.StartDate < DateTime.UtcNow.Date ||
                courseInfo.IsStudentEnrolledInCourse)
            {
                return false;
            }

            var studentInCourse = new StudentCourse
            {
                CourseId = courseId,
                StudentId = userId
            };

            await _dbContext.AddAsync(studentInCourse);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SignOutStudentAsync(int courseId, string userId)
        {
            var courseInfo = await GetCourseInfo(courseId, userId);

            if (courseInfo == null ||
                courseInfo.StartDate < DateTime.UtcNow.Date ||
                !courseInfo.IsStudentEnrolledInCourse)
            {
                return false;
            }

            var studentInCourse = await _dbContext.FindAsync<StudentCourse>(userId, courseId);
            _dbContext.Remove(studentInCourse);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SaveExamSubmission(int courseId, string studentId, byte[] examSubmission)
        {
            var course = await _dbContext.Courses.FindAsync(courseId);
            var student = await _dbContext.Users.FindAsync(studentId);
            var studentInCourse = await _dbContext
                .FindAsync<StudentCourse>(studentId, courseId);

            if (course == null || student == null || studentInCourse == null)
                return false;

            studentInCourse.ExamSubmission = examSubmission;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<CourseInfoServiceModel> GetCourseInfo(int courseId, string studentId)
            => await _dbContext
                .Courses
                .Where(c => c.Id == courseId)
                .Select(c => new CourseInfoServiceModel
                {
                    StartDate = c.StartDate,
                    IsStudentEnrolledInCourse = c.Students.Any(s => s.StudentId == studentId)
                })
                .FirstOrDefaultAsync();
    }
}