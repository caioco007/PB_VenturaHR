using DTO.Opportunity;
using DTO.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Opportunity
{
    public class OpportunityService : Shared.RepositoryService<ApplicationDbContext.Models.Opportunity, OpportunityViewModel, int>
    {
        public OpportunityService(ApplicationDbContext.Context.ApplicationDbContext context) : base(context, "OpportunityId")
        {
        }

        public async Task<OpportunityViewModel> GetInitialInfo(int? id, int companyTypeId)
        {
            var model = GetViewModel(x => x.OpportunityId == id).SingleOrDefault();
            if (model == null)
            {
                return new DTO.Opportunity.OpportunityViewModel { CompanyId = companyTypeId };
            }

            return model;
        }

        public override async Task<int> CreateAsync(OpportunityViewModel model)
        {
            var opportunity = new ApplicationDbContext.Models.Opportunity
            {
                StatusId = (int)StatusTypes.Publicada,
                Office = model.Office,
                Description = model.Description,
                EmploymentId = model.EmploymentId,
                ExpirationDate = DateTime.Now.AddDays(30),
                CreatedDate = DateTime.Now,
                CompanyId = model.CompanyId,
                IsDeleted = model.IsDeleted,
                Salary = model.Salary,
                Address = model.Address,
                City = model.City,
                Neighborhood = model.Neighborhood,
                Complement = model.Complement,
                State = model.State,
                Number = model.Number,
                ZipCode = model.ZipCode
            };

            this.dbSet.Add(opportunity);
            this.context.SaveChanges();

            return opportunity.OpportunityId;
        }

        public override async Task UpdateAsync(OpportunityViewModel model)
        {
            var opportunity = GetDataById(model.OpportunityId.Value);


            this.dbSet.Update(opportunity);
            this.context.SaveChanges();
        }

        public async Task<ApplicationDbContext.Models.CandidateForOpportunity> CreateCandidateForOpportunity(int? candidateId, int? opportunityId)
        {
            if(!candidateId.HasValue || !candidateId.HasValue) return null;

            var candidateForOpportunity = new ApplicationDbContext.Models.CandidateForOpportunity
            {
                CandidateId = candidateId.Value,
                OpportunityId = opportunityId.Value,
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };
            this.context.CandidateForOpportunity.Add(candidateForOpportunity);
            this.context.SaveChanges();

            return candidateForOpportunity;
        }
    }
}
