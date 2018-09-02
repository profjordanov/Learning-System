using System.Collections.Generic;
using System.Threading.Tasks;
using LearningSystem.Services.Models;

namespace LearningSystem.Services
{
    public interface IUserService : IService
    {
        Task<UserProfileServiceModel> ProfileAsync(string id);

        Task<IEnumerable<UserListingServiceModel>> FindAsync(string searchText);

        Task<byte[]> GetPdfCertificate(int courseId, string studentId);
    }
}