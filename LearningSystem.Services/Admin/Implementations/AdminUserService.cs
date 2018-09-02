using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using LearningSystem.Data.EF;
using LearningSystem.Services.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Services.Admin.Implementations
{
    public class AdminUserService : IAdminUserService
    {
        private readonly LearningSystemDbContext _dbContext;

        public AdminUserService(LearningSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<AdminUserListingServiceModel>> AllAsync()
            => await _dbContext
                .Users
                .ProjectTo<AdminUserListingServiceModel>()
                .ToListAsync();
    }
}