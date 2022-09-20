using DTO.Opportunity;
using DTO.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Opportunity
{
    public class OpportunityListService : Shared.RepositoryService<ApplicationDbContext.Models.OpportunityList, OpportunityListViewModel, int>
    {
        public OpportunityListService(ApplicationDbContext.Context.ApplicationDbContext context) : base(context, "OpportunityId")
        {
        }
    }
}
