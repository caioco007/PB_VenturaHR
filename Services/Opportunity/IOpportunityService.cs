using DTO.Opportunity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Opportunity
{
    public interface IOpportunityService : Shared.IRepositoryService<ApplicationDbContext.Models.Opportunity, OpportunityViewModel, int>
    {
        Task<OpportunityViewModel> GetInitialInfo(int? id, int companyTypeId);
        Task<List<int>> ObterOpportunityExpired();
        Task<List<int>> ObterOpportunityFinished();
        Task UpdateStatusOpportunity(int opportunityId, DTO.Opportunity.StatusTypes statusTypes);   
    }
}
