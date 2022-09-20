using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CandidateForOpportunity
{
    public partial class CandidateForOpportunityViewModel
    {
        public int CandidateForOpportunityId { get; set; }
        public int OpportunityId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CandidateId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
