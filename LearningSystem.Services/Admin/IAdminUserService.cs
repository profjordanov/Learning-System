using System.Collections.Generic;
using System.Threading.Tasks;
using LearningSystem.Services.Admin.Models;

namespace LearningSystem.Services.Admin
{
    public interface IAdminUserService
    {
        Task<IEnumerable<AdminUserListingServiceModel>> AllAsync();
    }
}