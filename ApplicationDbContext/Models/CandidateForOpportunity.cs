using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationDbContext.Models
{
    public partial class CandidateForOpportunity
    {
        public int CandidateForOpportunityId { get; set; }
        public int OpportunityId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CandidateId { get; set; }
        public bool IsDeleted { get; set; }
        public double? NotesByOpportunity { get; set; }        
    }
}
