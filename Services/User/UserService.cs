using ApplicationDbContext.Models;
using DTO.Claim;
using DTO.User;
using DTO.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.User
{
    public class UserService : Shared.RepositoryService<AspNetUsers, UserViewModel, int>
    {
        readonly AspNetIdentityDbContext.ApplicationDbContext identityContext;
        readonly UserManager<AspNetIdentityDbContext.User> userManager;

        public UserService(ApplicationDbContext.Context.ApplicationDbContext context, UserManager<AspNetIdentityDbContext.User> userManager, AspNetIdentityDbContext.ApplicationDbContext identityContext) : base(context, "Id")
        {
            this.userManager = userManager;
            this.identityContext = identityContext;
        }

        public override async Task DeleteAsync(int id, int? userId = null)
        {
            await base.DeleteAsync(id, userId);
            var user = await GetDataByIdAsync(id);

            // TODO: Pensar em uma maneira do Identity não criar usuário quando IsDeleted = 1
            user.UserName = "-";
            user.NormalizedEmail = "-";
            user.NormalizedUserName = "-";

            await this.context.SaveChangesAsync();
        }
        public override async Task<int> CreateAsync(UserViewModel model)
        {
            var result = await this.userManager.CreateAsync(new AspNetIdentityDbContext.User()
            {
                FirstName = model.FirstName,
                UserName = model.Email,
                Email = model.Email,
                IsActive = true,
                PersonId = model.PersonId,
                PhoneNumber = model.PhoneNumber,
                Birthday = model.Birthday
            }, model.Password);

            if (!result.Succeeded)
                throw new Exception("Não foi possível criar o usuário");

            var user = await this.userManager.FindByNameAsync(model.Email);
            await AttachUserToRole(user.Id, model.RoleName);

            return user.Id;
        }
        public override async Task UpdateAsync(UserViewModel model)
        {
            await base.UpdateAsync(model);

            var user = await userManager.FindByIdAsync(model.Id.ToString());

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, token, model.Password);
                if (!result.Succeeded) throw new Exception("Houve um erro ao alterar a senha.");
            }

            await AttachUserToRole(user.Id, model.RoleName);
        }

        public override List<UserViewModel> ToViewModel(IEnumerable<AspNetUsers> datas)
        {
            return (from u in datas
                    from ur in this.identityContext.UserRoles.Where(x => u.Id == x.UserId).DefaultIfEmpty()
                    from r in this.identityContext.Roles.Where(x => ur != null && ur.RoleId == x.Id).DefaultIfEmpty()
                    from p in this.context.Person.Where(x => x.PersonId == u.PersonId).DefaultIfEmpty()
                    where !u.IsDeleted
                    select new DTO.User.UserViewModel()
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        RoleName = (r == null ? "-" : r.Name),
                        RoleDescription = (r == null ? "-" : r.Description),
                        IsActive = u.IsActive,
                        PersonId = u.PersonId,
                        IsDeleted = u.IsDeleted,
                        PhoneNumber = u.PhoneNumber,
                        PersonName = p?.TradeName.IfNullChange(p?.FullName),
                        Birthday = u.Birthday
                    }).ToList();
        }

        public override UserViewModel ToViewModel(AspNetUsers data)
        {
            var userRoles = this.identityContext.UserRoles.Where(x => data.Id == x.UserId).FirstOrDefault();
            var role = this.identityContext.Roles.SingleOrDefault(x => userRoles != null && userRoles.RoleId == x.Id);

            return new DTO.User.UserViewModel()
            {
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                RoleName = (role == null ? "-" : role.Name),
                RoleDescription = (role == null ? "-" : role.Description),
                IsActive = data.IsActive,
                PersonId = data.PersonId,
                IsDeleted = data.IsDeleted,
                PhoneNumber = data.PhoneNumber,
                Birthday = data.Birthday
            };
        }

        public async Task<bool> EmailExists(string email) => await this.dbSet.AnyAsync(x => x.Email.ToUpper().Equals(email.ToUpper()) && !x.IsDeleted);
        public async Task AttachUserToRole(int id, string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName)) return;

            var user = await userManager.FindByIdAsync(Convert.ToString(id));
            var roles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRolesAsync(user, roles);
            await userManager.AddToRoleAsync(user, roleName);

            await userManager.UpdateAsync(user);
        }
        public async Task<bool> IsActive(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null && user.IsActive;
        }
        public async Task<bool> IsDeleted(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null && user.IsDeleted;
        }

        public async Task<bool> ValidateUserPerson(int userId, int personId)
        {
            var user = await GetViewModelByIdAsync(userId);
            
            if (ClaimHelper.AdministratorRoles.Any(x => x == user.RoleName)) return true; //Administrator

            if (ClaimHelper.CompanyRoles.Any(x => x == user.RoleName)) //Company
            {
                if (user.PersonId == personId) return true;

                return false;
            }

            if (ClaimHelper.CandidateRoles.Any(x => x == user.RoleName)) //Candidate
            {
                if (user.PersonId == personId) return true;

                return false;
            }

            return false;
        }

        public async Task<bool> UserHasAccessToManagePersonType(int userId, int personTypeId)
        {
            var user = await GetViewModelByIdAsync(userId);
            var companyType = await this.context.PersonType.SingleAsync(x => x.PersonTypeId == personTypeId);

            if (ClaimHelper.AdministratorRoles.Any(x => x == user.RoleName)) return true; //Administrator

            if (ClaimHelper.CompanyRoles.Any(x => x == user.RoleName)) //Candidate
            {
                if (companyType.ExternalCode == "CANDIDATE") return true;

                return false;
            }

            if (ClaimHelper.CompanyRoles.Any(x => x == user.RoleName)) //Company
            {
                if (companyType.ExternalCode == "COMPANY") return true;

                return false;
            }

            return false;
        }
        
        public async Task<bool> Exists(int? userId)
        {
            if (userId == null) return false;

            var user = GetDataById(userId.Value);

            if(user == null) return false;

            return true;
        }

        public async Task BlockUser(int userId)
        {
            var user = GetDataById(userId);

            user.IsActive = false;

            this.dbSet.Update(user);
            this.context.SaveChanges();
        }

        public async Task ActiveUser(int userId)
        {
            var user = GetDataById(userId);

            user.IsActive = true;

            this.dbSet.Update(user);
            this.context.SaveChanges();
        }
    }
}
