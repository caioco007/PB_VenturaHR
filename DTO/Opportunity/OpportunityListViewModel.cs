using DTO.Opportunity;
using DTO.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Opportunity
{
    public class OpportunityListViewModel : DTO.Shared.AddressViewModel
    {
        public int OpportunityId { get; set; }
        public string Office { get; set; }
        public string Description { get; set; }
        public int EmploymentId { get; set; }
        public string EmploymentIdName { get; set; }
        public int Experience { get; set; }
        public string Local { get; set; }
        public int CompanyId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public double? Salary { get; set; }
        public string _Salary { get { return Salary.ToPtBR(); } }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public bool IsDeleted { get; set; }
        public string CompanyName { get; set; }
        public bool IsActive => ExpirationDate > DateTime.Today;
    }
}