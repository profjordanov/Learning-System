using System.Collections.Generic;
using LearningSystem.Services.Admin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearningSystem.Web.Areas.Admin.Models.Users
{
    public class UserListingViewModel
    {
        public IEnumerable<AdminUserListingServiceModel> Users { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}