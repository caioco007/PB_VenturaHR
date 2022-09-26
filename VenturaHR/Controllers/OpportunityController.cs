using DTO.Claim;
using DTO.Opportunity;
using DTO.Person;
using DTO.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.CandidateForOpportunity;
using Services.Opportunity;
using Services.OpportunityCriterion;
using Services.Person;
using Services.ResponseCriterion;
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
        readonly OpportunityCriterionService opportunityCriterionService;
        readonly CandidateForOpportunityService candidateForOpportunityService;
        readonly ResponseCriterionService responseCriterionService;
        readonly ViewEngineHelper viewEngineHelper;
        private readonly UserManager<AspNetIdentityDbContext.User> userManager;

        public OpportunityController(PersonService personService, PersonTypeService personTypeService, OpportunityService opportunityService, OpportunityListService opportunityListService, OpportunityCriterionService opportunityCriterionService, CandidateForOpportunityService candidateForOpportunityService, ResponseCriterionService responseCriterionService, ViewEngineHelper viewEngineHelper, UserManager<AspNetIdentityDbContext.User> userManager)
        {
            this.personService = personService;
            this.personTypeService = personTypeService;
            this.opportunityService = opportunityService;
            this.opportunityListService = opportunityListService;
            this.opportunityCriterionService = opportunityCriterionService;
            this.candidateForOpportunityService = candidateForOpportunityService;
            this.responseCriterionService = responseCriterionService;
            this.viewEngineHelper = viewEngineHelper;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int? personId)
        {
            if (personId.HasValue)
            {
                var personTypeId = personService.GetDataById(personId.Value).PersonTypeId;
                ViewData["PersonId"] = personId.Value;
                ViewData["PersonTypeId"] = personTypeId;
            }
            return await Task.Run(() => View());
        } 

        [HttpPost]
        [ActionName("CompanyManage")]
        [Authorize(Roles = ClaimHelper.AuthorizationCompanyRoles + ", " + ClaimHelper.AuthorizationAdministratorRoles)]
        public async Task<IActionResult> _Manage(DTO.Opportunity.OpportunityViewModel model)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                model.CompanyId = user.PersonId.Value;
                if (model.OpportunityId.HasValue)
                {
                    int countCandidateForOpportunity = await candidateForOpportunityService.GetCountCandidateForOpportunity(model.OpportunityId.Value);

                    if (countCandidateForOpportunity > 0)
                    {
                        ViewData["ErrorMessage"] = "Não foi possível fazer alteração, têm candidato inscrito nessa vaga.";
                        return await Task.Run(() => View("Manage", model));
                    }
                }
                model.OpportunityId = await this.opportunityService.CreateOrUpdateAsync(model);

                await opportunityCriterionService.SaveCriteriaToOpportunity(model.OpportunityId.Value, model.Criteria);
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
            model.Criteria = await opportunityCriterionService.GetOpportunityCriterionByOpportunityId(model.OpportunityId.Value);

            return await Task.Run(() => View("Manage", model));
        }

        public async Task<IActionResult> Detail(int? id)
        {
            var model = await opportunityService.GetViewModelByIdAsync(id.Value);

            model.Criteria = await opportunityCriterionService.GetOpportunityCriterionByOpportunityId(model.OpportunityId.Value);

            return await Task.Run(() => View("Detail", model));
        }

        public async Task<IActionResult> LoadCandidateForOpportunityProcedureViewComponent(int opportunityId) => await Task.Run(() => ViewComponent(typeof(ViewComponents.CandidateForOpportunityProcedureViewComponent.CandidateForOpportunityProcedureViewComponent), new { opportunityId }));

        [HttpPost]
        [Authorize(Roles = ClaimHelper.AuthorizationCandidateRoles)]
        public async Task<IActionResult> InterestedOpportunity(DTO.Opportunity.OpportunityViewModel model)
        {
            var user = await userManager.GetUserAsync(User);

            var notesByOpportunity = await SaveResponseCriterion(model.ResponseCriteria, user.PersonId, model.OpportunityId);
            await candidateForOpportunityService.CreateCandidateForOpportunity(user.PersonId, model.OpportunityId, notesByOpportunity);


            return await Task.Run(() => View("Detail", model.OpportunityId));
        }

        private async Task<float> SaveResponseCriterion(List<DTO.ResponseCriterion.ResponseCriterionViewModel> responseCriteria, int? candidateId, int? opportunityId)
        {
            await responseCriterionService.SaveResponseCriteriaToCandidate(responseCriteria, candidateId.Value);

            return await candidateForOpportunityService.CalculateNotesByOpportunity(opportunityId.Value);

        }

        public virtual async Task<IActionResult> List(DataTablesAjaxPostModel filter, int? personId)
        {
            var person = personId.HasValue && personId > 0? personService.GetDataById(personId.Value) : null;
            string query = "";
            var parameters = new SqlParameterList();
            parameters.AddParameter("IsDeleted", false);
            if (personId.HasValue && personId > 0)
            {
                if (person.PersonTypeId == (int)DTO.Person.PersonType.Company) 
                    parameters.AddParameter("CompanyId", personId);
                else if (person.PersonTypeId == (int)DTO.Person.PersonType.Candidate)
                {
                    var opportunityIds = await candidateForOpportunityService.GetOpportunityIdByCandidateId(personId.Value);
                    if (opportunityIds.Count > 0)
                    {
                        query = $"OpportunityId IN ({string.Join(",", opportunityIds.Select(x => x.ToString()))}) ";
                        //parameters.AddParameter("OpportunityId", string.Join(",", opportunityIds.Select(x => x.ToString())));
                    }
                    else
                    {
                        query = $"OpportunityId = 0 ";
                    }
                }
            }

            List<OpportunityListViewModel> data = new List<OpportunityListViewModel>();
            int recordsTotal = 0, recordsFiltered = 0;

            var filterData = opportunityListService.GetDataFiltered(filter, out recordsTotal, out recordsFiltered, query, parameters.GetParameters());

            data = opportunityListService.ToViewModel(filterData);

            return await Task.Run(() => Json(new
            {
                recordsTotal,
                recordsFiltered,
                data
            }));
        }

        [HttpPost]
        [Authorize(Roles = ClaimHelper.AuthorizationCompanyRoles)]
        public async Task<IActionResult> FinalizarOpportunity(int? opportunityId)
        {
            if(!opportunityId.HasValue) return Json(false);

            await opportunityService.UpdateStatusOpportunity(opportunityId.Value, DTO.Opportunity.StatusTypes.Finalizada);

            return Json(true);
        }

        [HttpPost]
        [Authorize(Roles = ClaimHelper.AuthorizationCompanyRoles)]
        public async Task<IActionResult> RenovarOpportunity(int? opportunityId)
        {
            if (!opportunityId.HasValue) return Json(false);

            await opportunityService.UpdateStatusOpportunity(opportunityId.Value, DTO.Opportunity.StatusTypes.Publicada);

            return Json(true);
        }
    }
}
