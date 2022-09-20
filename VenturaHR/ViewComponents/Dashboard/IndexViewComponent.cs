using DTO.Claim;
using DTO.Dashboard;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        readonly UserManager<AspNetIdentityDbContext.User> userManager;

        public IndexViewComponent(PersonService personService, PersonTypeService personTypeService, OpportunityService opportunityService, UserManager<AspNetIdentityDbContext.User> userManager)
        {
            this.personService = personService;
            this.personTypeService = personTypeService;
            this.opportunityService = opportunityService;
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
                dashboardViewModel.TotalActiveOpportunitys = await opportunityService.CountAsync(x => !x.IsDeleted && x.ExpirationDate > DateTime.Today);

                dashboardViewModel.TotalCompanys = await personService.CountAsync(x => x.PersonTypeId == personCompanyTypeId && !x.IsDeleted);
                dashboardViewModel.TotalActiveCompanys = await personService.CountAsync(x => x.PersonTypeId == personCompanyTypeId && !x.IsDeleted && x.IsActive);

                dashboardViewModel.TotalCandidates = await personService.CountAsync(x => x.PersonTypeId == personCandidateTypeId && !x.IsDeleted);
                dashboardViewModel.TotalActiveCandidates = await personService.CountAsync(x => x.PersonTypeId == personCandidateTypeId && !x.IsDeleted && x.IsActive);
            }
            else if (HttpContext.User.IsCompany())
            {
                var user = await userManager.GetUserAsync(HttpContext.User);

                dashboardViewModel.TotalOpportunitys = await opportunityService.CountAsync(x => !x.IsDeleted && x.CompanyId == user.PersonId);
                dashboardViewModel.TotalActiveOpportunitys = await opportunityService.CountAsync(x => !x.IsDeleted && x.ExpirationDate > DateTime.Today && x.CompanyId == user.PersonId);
            }

            else if (HttpContext.User.IsCandidate())
            {
                var user = await userManager.GetUserAsync(HttpContext.User);

                dashboardViewModel.TotalOpportunitys = await opportunityService.CountAsync(x => !x.IsDeleted && x.CandidateId == user.PersonId);
                dashboardViewModel.TotalActiveOpportunitys = await opportunityService.CountAsync(x => !x.IsDeleted && x.ExpirationDate > DateTime.Today && x.CandidateId == user.PersonId);
            }

            return await Task.Run(() => View("Index", dashboardViewModel));
        }
    }
}
