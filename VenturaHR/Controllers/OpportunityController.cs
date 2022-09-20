using DTO.Claim;
using DTO.Opportunity;
using DTO.Person;
using DTO.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Opportunity;
using Services.Person;
using Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VenturaHR.Helpers;
using VenturaHR.Models;

namespace VenturaHR.Controllers
{
    public class OpportunityController : Controller
    {
        readonly PersonService personService;
        readonly PersonTypeService personTypeService;
        readonly OpportunityService opportunityService;
        readonly OpportunityListService opportunityListService;
        readonly ViewEngineHelper viewEngineHelper;
        private readonly UserManager<AspNetIdentityDbContext.User> userManager;

        public OpportunityController(PersonService personService, PersonTypeService personTypeService, OpportunityService opportunityService, OpportunityListService opportunityListService, ViewEngineHelper viewEngineHelper, UserManager<AspNetIdentityDbContext.User> userManager)
        {
            this.personService = personService;
            this.personTypeService = personTypeService;
            this.opportunityService = opportunityService;
            this.opportunityListService = opportunityListService;
            this.viewEngineHelper = viewEngineHelper;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index() => await Task.Run(() => View());

        [HttpPost]
        [ActionName("CompanyManage")]
        [Authorize(Roles = ClaimHelper.AuthorizationCompanyRoles + ", " + ClaimHelper.AuthorizationAdministratorRoles)]
        public async Task<IActionResult> _Manage(DTO.Opportunity.OpportunityViewModel model)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                model.CompanyId = user.PersonId.Value;
                model.OpportunityId = await this.opportunityService.CreateOrUpdateAsync(model);
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return await Task.Run(() => RedirectToAction("Index"));
        }

        [Authorize(Roles = ClaimHelper.AuthorizationCompanyRoles + ", " + ClaimHelper.AuthorizationAdministratorRoles)]
        public async Task<IActionResult> CompanyManage(int? id)
        {
            var personTypeId = (await personTypeService.GetDataByExternalCode("COMPANY")).PersonTypeId;
            ViewBag.PersonTypeId = personTypeId;
            var model = await opportunityService.GetInitialInfo(id, personTypeId);

            return await Task.Run(() => View("Manage", model));
        }

        [Authorize(Roles = ClaimHelper.AuthorizationCandidateRoles)]
        public async Task<IActionResult> Detail(int? id)
        {
            var model = await opportunityService.GetViewModelByIdAsync(id.Value);

            return await Task.Run(() => View("Detail", model));
        }

        [HttpPost]
        [Authorize(Roles = ClaimHelper.AuthorizationCandidateRoles)]
        public async Task<IActionResult> InterestedOpportunity(DTO.Opportunity.OpportunityViewModel model)
        {
            var user = await userManager.GetUserAsync(User);

            await opportunityService.CreateCandidateForOpportunity(user.Id, model.OpportunityId.Value);

            return await Task.Run(() => View("Detail", model));
        }

        public virtual async Task<IActionResult> List(DataTablesAjaxPostModel filter)
        {
            var parameters = new SqlParameterList();
            parameters.AddParameter("IsDeleted", false);

            List<OpportunityListViewModel> data = new List<OpportunityListViewModel>();
            int recordsTotal = 0, recordsFiltered = 0;

            var filterData = opportunityListService.GetDataFiltered(filter, out recordsTotal, out recordsFiltered, parameters.GetQuery(), parameters.GetParameters());

            data = opportunityListService.ToViewModel(filterData);

            return await Task.Run(() => Json(new
            {
                recordsTotal,
                recordsFiltered,
                data
            }));
        }
    }
}
