using Microsoft.AspNetCore.Mvc;
using Services.CandidateForOpportunity;
using Services.Opportunity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VenturaHR.ViewComponents.CandidateForOpportunityProcedureViewComponent
{
    public class CandidateForOpportunityProcedureViewComponent : ViewComponent
    {
        readonly CandidateForOpportunityService candidateForOpportunityService;

        public CandidateForOpportunityProcedureViewComponent(CandidateForOpportunityService candidateForOpportunityService)
        {
            this.candidateForOpportunityService = candidateForOpportunityService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int opportunityId) => await Task.Run(async () => View(await candidateForOpportunityService.GetCandidateForOpportunity(opportunityId)));
    }
}
