using DTO.Opportunity;
using DTO.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Opportunity
{
    public class OpportunityViewModel : DTO.Shared.AddressViewModel
    {
        public int? OpportunityId { get; set; }
        [Update]
        public StatusTypes StatusType { get; set; }
        public int StatusId
        {
            get => (int)StatusType;
            set => StatusType = (StatusTypes)value;
        }
        public string Office { get; set; }
        public string Description { get; set; }
        public EmploymentTypes EmploymentType { get; set; }
        public int EmploymentId
        {
            get => (int)EmploymentType;
            set => EmploymentType = (EmploymentTypes)value;
        }
        public double? Salary { get; set; }
        public string _Salary { get { return Salary.ToPtBR(); } }
        public DateTime? ExpirationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        [Update]
        public int? CandidateId { get; set; }
        public int CompanyId { get; set; }
        [Update]
        public bool IsDeleted { get; set; }
        public bool IsActive => ExpirationDate > DateTime.Today;
    }
}