using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ResponseCriterion
{
    public class ResponseCriterionViewModel
    {
        public int? ResponseCriterionId { get; set; }
        public int? CandidateId { get; set; }
        public int? OpportunityCriterionId { get; set; }
        public string Criterion { get; set; }
        public string Description { get; set; }
        public int? AnswerCriterion { get; set; }
        public int Weight { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
