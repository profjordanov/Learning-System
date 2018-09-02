using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using LearningSystem.Data.EF;
using LearningSystem.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly LearningSystemDbContext _db;
        private readonly IPdfGenerator _pdfGenerator;

        public UserService(IPdfGenerator pdfGenerator, 
            LearningSystemDbContext db)
        {
            _pdfGenerator = pdfGenerator;
            _db = db;
        }

        public async Task<UserProfileServiceModel> ProfileAsync(string id)
            => await _db
                .Users
                .Where(u => u.Id == id)
                .ProjectTo<UserProfileServiceModel>(new { studentId = id })
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<UserListingServiceModel>> FindAsync(string searchText)
            => await _db
                .Users
                .OrderBy(u => u.UserName)
                .Where(u => u.UserName.ToLower().Contains((searchText ?? string.Empty).ToLower()))
                .ProjectTo<UserListingServiceModel>()
                .ToListAsync();

        public Task<byte[]> GetPdfCertificate(int courseId, string studentId)
        {
            throw new System.NotImplementedException();
        }
    }
}