using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApplicationDbContext.Models
{
    public class Response
    {
        public int ResponseId { get; set; }
        public int OpportunityId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CandidateId { get; set; }
        public bool IsDeleted { get; set; }
        public double? NotesByOpportunity { get; set; }
    }
}
