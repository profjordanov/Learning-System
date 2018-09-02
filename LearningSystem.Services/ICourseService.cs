using System.Collections.Generic;
using System.Threading.Tasks;
using LearningSystem.Services.Models;

namespace LearningSystem.Services
{
    public interface ICourseService : IService
    {
        Task<IEnumerable<CourseListingServiceModel>> AllActiveAsync();

        Task<IEnumerable<CourseListingServiceModel>> FindAsync(string searchText);

        Task<TModel> ByIdAsync<TModel>(int id) where TModel : class;

        Task<bool> IsUserSignedInCourseAsync(int courseId, string userId);

        Task<bool> SignUpStudentAsync(int courseId, string userId);

        Task<bool> SignOutStudentAsync(int courseId, string userId);

        Task<bool> SaveExamSubmission(int courseId, string studentId, byte[] examSubmission);
    }
}