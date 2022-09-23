using DTO.Claim;
using DTO.Dashboard;
using DTO.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.CandidateForOpportunity;
using Services.Opportunity;
using Services.Person;
using System;
using System.Threading.Tasks;

namespace VenturaHR.ViewComponents.Dashboard
{
    [ViewComponent(Name = "Dashboard")]
    public class IndexViewComponent : ViewComponent
    {
        readonly PersonService personService;
        readonly PersonTypeService personTypeService;
        readonly OpportunityService opportunityService;
        readonly CandidateForOpportunityService candidateForOpportunityService;
        readonly UserManager<AspNetIdentityDbContext.User> userManager;

        public IndexViewComponent(PersonService personService, PersonTypeService personTypeService, OpportunityService opportunityService, CandidateForOpportunityService candidateForOpportunityService, UserManager<AspNetIdentityDbContext.User> userManager)
        {
            this.personService = personService;
            this.personTypeService = personTypeService;
            this.opportunityService = opportunityService;
            this.candidateForOpportunityService = candidateForOpportunityService;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var dashboardViewModel = new DashboardViewModel();

            var personCandidateTypeId = (await personTypeService.GetDataByExternalCode("CANDIDATE")).PersonTypeId;
            var personCompanyTypeId = (await personTypeService.GetDataByExternalCode("COMPANY")).PersonTypeId;

            if (HttpContext.User.IsAdministrator())
            {
                dashboardViewModel.TotalOpportunitys = await opportunityService.CountAsync(x => !x.IsDeleted);
                dashboardViewModel.TotalOpportunitysToday = await opportunityService.CountAsync(x => !x.IsDeleted && x.CreatedDate.Date == DateTime.Now.Date);
                dashboardViewModel.TotalActiveOpportunitys = await opportunityService.CountAsync(x => !x.IsDeleted && x.ExpirationDate > DateTime.Today);

                dashboardViewModel.TotalCompanys = await personService.CountAsync(x => x.PersonTypeId == personCompanyTypeId && !x.IsDeleted);
                dashboardViewModel.TotalCompanysToday = await personService.CountAsync(x => x.PersonTypeId == personCompanyTypeId && !x.IsDeleted && x.CreatedDate.Date == DateTime.Now.Date );
                dashboardViewModel.TotalActiveCompanys = await personService.CountAsync(x => x.PersonTypeId == personCompanyTypeId && !x.IsDeleted && x.IsActive);

                dashboardViewModel.TotalCandidates = await personService.CountAsync(x => x.PersonTypeId == personCandidateTypeId && !x.IsDeleted);
                dashboardViewModel.TotalCandidatesToday = await personService.CountAsync(x => x.PersonTypeId == personCandidateTypeId && !x.IsDeleted && x.CreatedDate.Date == DateTime.Now.Date);
                dashboardViewModel.TotalActiveCandidates = await personService.CountAsync(x => x.PersonTypeId == personCandidateTypeId && !x.IsDeleted && x.IsActive);

                dashboardViewModel.TotalCandidateForOpportunitys = await candidateForOpportunityService.CountAsync(x => !x.IsDeleted);
                dashboardViewModel.TotalCandidateForOpportunitysToday = await candidateForOpportunityService.CountAsync(x => !x.IsDeleted && x.CreatedDate.Date == DateTime.Now.Date);
            }
            //else if (HttpContext.User.IsCompany())
            //{
            //    var user = await userManager.GetUserAsync(HttpContext.User);

            //    dashboardViewModel.TotalOpportunitys = await opportunityService.CountAsync(x => !x.IsDeleted && x.CompanyId == user.PersonId);
            //    dashboardViewModel.TotalActiveOpportunitys = await opportunityService.CountAsync(x => !x.IsDeleted && x.ExpirationDate > DateTime.Today && x.CompanyId == user.PersonId);
            //}

            //else if (HttpContext.User.IsCandidate())
            //{
            //    var user = await userManager.GetUserAsync(HttpContext.User);
            //    var opportunityIds = await candidateForOpportunityService.GetOpportunityIdByCandidateId(user.PersonId.Value);

            //    dashboardViewModel.TotalOpportunitys = await opportunityService.CountAsync(x => !x.IsDeleted && opportunityIds.Contains(x.OpportunityId));
            //    dashboardViewModel.TotalActiveOpportunitys = await opportunityService.CountAsync(x => !x.IsDeleted && x.ExpirationDate > DateTime.Today && opportunityIds.Contains(x.OpportunityId));
            //}

            return await Task.Run(() => View("Dashboard", dashboardViewModel));
        }
    }
}
