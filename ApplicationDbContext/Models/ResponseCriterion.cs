using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationDbContext.Models
{
    public partial class ResponseCriterion
    {
        public int ResponseCriterionId { get; set; }
        public int CandidateId { get; set; }
        public int OpportunityCriterionId { get; set; }
        public int AnswerCriterion { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
