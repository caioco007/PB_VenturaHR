using DTO.OpportunityCriterion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.OpportunityCriterion
{
    public class OpportunityCriterionService : Shared.RepositoryService<ApplicationDbContext.Models.OpportunityCriterion, OpportunityCriterionViewModel, int>
    {
        public OpportunityCriterionService(ApplicationDbContext.Context.ApplicationDbContext context) : base(context, "OpportunityCriterionId")
        {
        }

        public async Task SaveCriteriaToOpportunity(int opportunityId, List<OpportunityCriterionViewModel> criteria)
        {
            foreach (var criterion in criteria)
            {
                var opportunityCriterion = new ApplicationDbContext.Models.OpportunityCriterion();

                opportunityCriterion.OpportunityId = opportunityId;
                opportunityCriterion.Criterion = criterion.Criterion;
                opportunityCriterion.Description = criterion.Description;
                opportunityCriterion.Weight = criterion.Weight;
                opportunityCriterion.CreatedDate = DateTime.Now;

                await this.context.OpportunityCriterion.AddAsync(opportunityCriterion);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<List<OpportunityCriterionViewModel>> GetOpportunityCriterionByOpportunityId(int opportunityId)
        {
            List<OpportunityCriterionViewModel> opportunityCriterionViewModelList = new List<OpportunityCriterionViewModel>();
            var model = GetViewModel(x => x.OpportunityId == opportunityId);
            opportunityCriterionViewModelList.AddRange(model);
            return opportunityCriterionViewModelList;
        }
    }
}
