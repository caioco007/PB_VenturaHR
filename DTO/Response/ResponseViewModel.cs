using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Response
{
    public partial class ResponseViewModel
    {
        public int ResponseId { get; set; }
        public int OpportunityId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CandidateId { get; set; }
        public bool IsDeleted { get; set; }
        public double? NotesByOpportunity { get; set; }
    }
}
