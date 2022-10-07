using DTO.Response;
using DTO.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Response
{
    public class ResponseService : Shared.RepositoryService<ApplicationDbContext.Models.Response, ResponseViewModel, int>
    {
        readonly ApplicationDbContext.Context.ApplicationDbContext context;
        public ResponseService(ApplicationDbContext.Context.ApplicationDbContext context) : base(context, "Id")
        {
            this.context = context;
        }

        public async Task<ApplicationDbContext.Models.Response> CreateCandidateForOpportunity(int? candidateId, int? opportunityId, double? notesByOpportunity)
        {
            if (!candidateId.HasValue || !candidateId.HasValue) return null;

            var candidateForOpportunity = new ApplicationDbContext.Models.Response
            {
                CandidateId = candidateId.Value,
                OpportunityId = opportunityId.Value,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                NotesByOpportunity = notesByOpportunity
            };
            this.context.Response.Add(candidateForOpportunity);
            this.context.SaveChanges();

            return candidateForOpportunity;
        }

        public async Task<List<DTO.Response.ResponseProcedureViewModel>> GetCandidateForOpportunity(int opportunityId)
        {

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {

                command.CommandText = "pr_GetResponse @OpportunityId = @_OpportunityId";

                command.Parameters.Add(new SqlParameter("@_OpportunityId", opportunityId));

                await context.Database.OpenConnectionAsync();

                var models = new List<DTO.Response.ResponseProcedureViewModel>();

                using (var result = await command.ExecuteReaderAsync())
                {
                    while (await result.ReadAsync())
                        models.Add(result.CopyToEntity<DTO.Response.ResponseProcedureViewModel>());
                }
                return models;
            }

        }

        public async Task<int> GetCountCandidateForOpportunity(int opportunityId) => this.context.Response.Where(x => x.OpportunityId == opportunityId).Count();

        public async Task<List<int>> GetOpportunityIdByCandidateId(int candidateId)
        {
            List<int> opportunityIds = new List<int>();
            var candidateForOpportunity = this.context.Response.Where(x => x.CandidateId == candidateId && !x.IsDeleted).ToList();
            if (candidateForOpportunity.Count > 0) opportunityIds.AddRange(candidateForOpportunity.Select(x => x.OpportunityId));
            return opportunityIds;
        }
        
        public async Task<bool> ExistCandidateForOpportunity(int candidateId, int opportunityId)
        {
            var candidateForOpportunity = this.context.Response.Any(x => x.CandidateId == candidateId && x.OpportunityId == opportunityId && !x.IsDeleted);
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
