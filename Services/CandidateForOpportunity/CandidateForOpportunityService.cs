using DTO.CandidateForOpportunity;
using DTO.ResponseCriterion;
using DTO.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CandidateForOpportunity
{
    public class CandidateForOpportunityService : Shared.RepositoryService<ApplicationDbContext.Models.CandidateForOpportunity, CandidateForOpportunityViewModel, int>
    {
        readonly ApplicationDbContext.Context.ApplicationDbContext context;
        public CandidateForOpportunityService(ApplicationDbContext.Context.ApplicationDbContext context) : base(context, "CandidateForOpportunityId")
        {
            this.context = context;
        }

        public async Task<ApplicationDbContext.Models.CandidateForOpportunity> CreateCandidateForOpportunity(int? candidateId, int? opportunityId, double? notesByOpportunity)
        {
            if (!candidateId.HasValue || !candidateId.HasValue) return null;

            var candidateForOpportunity = new ApplicationDbContext.Models.CandidateForOpportunity
            {
                CandidateId = candidateId.Value,
                OpportunityId = opportunityId.Value,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                NotesByOpportunity = notesByOpportunity
            };
            this.context.CandidateForOpportunity.Add(candidateForOpportunity);
            this.context.SaveChanges();

            return candidateForOpportunity;
        }

        public async Task<List<DTO.CandidateForOpportunity.CandidateForOpportunityProcedureViewModel>> GetCandidateForOpportunity(int opportunityId)
        {

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {

                command.CommandText = "pr_GetCandidateForOpportunity @OpportunityId = @_OpportunityId";

                command.Parameters.Add(new SqlParameter("@_OpportunityId", opportunityId));

                await context.Database.OpenConnectionAsync();

                var models = new List<DTO.CandidateForOpportunity.CandidateForOpportunityProcedureViewModel>();

                using (var result = await command.ExecuteReaderAsync())
                {
                    while (await result.ReadAsync())
                        models.Add(result.CopyToEntity<DTO.CandidateForOpportunity.CandidateForOpportunityProcedureViewModel>());
                }
                return models;
            }

        }

        public async Task<int> GetCountCandidateForOpportunity(int opportunityId) => this.context.CandidateForOpportunity.Where(x => x.OpportunityId == opportunityId).Count();

        public async Task<List<int>> GetOpportunityIdByCandidateId(int candidateId)
        {
            List<int> opportunityIds = new List<int>();
            var candidateForOpportunity = this.context.CandidateForOpportunity.Where(x => x.CandidateId == candidateId && !x.IsDeleted).ToList();
            if (candidateForOpportunity.Count > 0) opportunityIds.AddRange(candidateForOpportunity.Select(x => x.OpportunityId));
            return opportunityIds;
        }
        
        public async Task<bool> ExistCandidateForOpportunity(int candidateId, int opportunityId)
        {
            var candidateForOpportunity = this.context.CandidateForOpportunity.Any(x => x.CandidateId == candidateId && x.OpportunityId == opportunityId && !x.IsDeleted);
            return candidateForOpportunity;
        }
    
        public async Task<float> CalculateNotesByOpportunity(int oppotunityId)
        {
            var opportunityCriterion = this.context.OpportunityCriterion.Where(x => x.OpportunityId == oppotunityId);

            List<int> Sum_WeightAndAnswerCriterion = new List<int>();

            foreach (var item in opportunityCriterion)
            {
                var answerCriterion = this.context.ResponseCriterion.Where(x => x.OpportunityCriterionId == item.OpportunityCriterionId).Select(c => c.AnswerCriterion);
                var caclulo = answerCriterion.Single() * item.Weight;

                Sum_WeightAndAnswerCriterion.Add(caclulo);
            }

            var sum_WeightAndAnswerCriterion = Sum_WeightAndAnswerCriterion.Sum();
            var reponseCriterion_Sum = opportunityCriterion.Sum(y => y.Weight);

            var divide = Decimal.Divide(sum_WeightAndAnswerCriterion,reponseCriterion_Sum);

            return (float)Decimal.Round(divide, 2); ;
        }
    }
}
