using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Response
{
    public partial class ResponseProcedureViewModel
    {
        public int CandidateId { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public double? NotesByOpportunity { get; set; }
    }
}
