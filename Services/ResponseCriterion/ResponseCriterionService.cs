using DTO.ResponseCriterion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.ResponseCriterion
{
    public class ResponseCriterionService : Shared.RepositoryService<ApplicationDbContext.Models.ResponseCriterion, ResponseCriterionViewModel, int>
    {
        public ResponseCriterionService(ApplicationDbContext.Context.ApplicationDbContext context) : base(context, "ResponseCriterionId")
        {
        }

        public async Task SaveResponseCriteriaToCandidate(List<ResponseCriterionViewModel> model, int candidateId)
        {
            foreach (var responseCriterion in model)
            {
                var entity = new ApplicationDbContext.Models.ResponseCriterion();

                entity.CandidateId = candidateId;
                entity.OpportunityCriterionId = responseCriterion.OpportunityCriterionId.Value;
                entity.AnswerCriterion = responseCriterion.AnswerCriterion.Value;
                entity.CreatedDate = DateTime.Now;

                await this.context.ResponseCriterion.AddAsync(entity);
            }

            await this.context.SaveChangesAsync();
        }

    }
}
