using DTO.Claim;
using DTO.Opportunity;
using DTO.Shared;
using DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Person;
using Services.Shared;
using Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VenturaHR.Models;

namespace VenturaHR.Controllers
{
    public class UserController : Controller
    {
        readonly UserListService userListService;
        readonly UserService userService;
        readonly PersonService personService;
        private readonly UserManager<AspNetIdentityDbContext.User> userManager;

        public UserController(UserListService userListService, UserService userService, PersonService personService, UserManager<AspNetIdentityDbContext.User> userManager)
        {
            this.userListService = userListService;
            this.userService = userService;
            this.personService = personService;
            this.userManager = userManager;
        }

        [Authorize(Roles = ClaimHelper.AuthorizationAdministratorRoles)]
        public async Task<IActionResult> Index() => await Task.Run(() => View());

        [Authorize(Roles = ClaimHelper.AuthorizationAdministratorRoles)]
        public async Task<IActionResult> Manage() => await Task.Run(() => View());

        public virtual async Task<IActionResult> List(DataTablesAjaxPostModel filter)
        {
            var parameters = new SqlParameterList();
            parameters.AddParameter("IsDeleted", false);

            List<UserListViewModel> data = new List<UserListViewModel>();
            int recordsTotal = 0, recordsFiltered = 0;

            var filterData = userListService.GetDataFiltered(filter, out recordsTotal, out recordsFiltered, parameters.GetQuery(), parameters.GetParameters());

            data = userListService.ToViewModel(filterData);

            return await Task.Run(() => Json(new
            {
                recordsTotal,
                recordsFiltered,
                data
            }));
        }

        [HttpPost]
        [ActionName("BlockUser")]
        [Authorize(Roles = ClaimHelper.AuthorizationAdministratorRoles)]
        public async Task<IActionResult> BlockUser(int id, string motivo)
        {
            if (!await userService.Exists(id)) return Json(false);

            var userViewModel = userService.GetViewModelById(id);

            await userService.BlockUser(id);
            await personService.BlockPerson(userViewModel.PersonId.Value, motivo);

            return Json(true);
        }

        [HttpPost]
        [ActionName("ActiveUser")]
        [Authorize(Roles = ClaimHelper.AuthorizationAdministratorRoles)]
        public async Task<IActionResult> ActiveUser(int id)
        {
            if (!await userService.Exists(id)) return Json(false);

            var userViewModel = userService.GetViewModelById(id);

            await userService.ActiveUser(id);
            await personService.ActivePerson(userViewModel.PersonId.Value);

            return Json(true);
        }
    }
}
