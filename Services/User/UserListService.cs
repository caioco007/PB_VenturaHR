using DTO.Opportunity;
using DTO.Shared;
using DTO.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.User
{
    public class UserListService : Shared.RepositoryService<ApplicationDbContext.Models.UserList, UserListViewModel, int>
    {
        public UserListService(ApplicationDbContext.Context.ApplicationDbContext context) : base(context, "UserId")
        {
        }
    }
}
