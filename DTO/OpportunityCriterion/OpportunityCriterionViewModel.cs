using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.OpportunityCriterion
{
    public class OpportunityCriterionViewModel
    {
        public int OpportunityCriterionId { get; set; }
        public int OpportunityId { get; set; }
        public string Criterion { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
