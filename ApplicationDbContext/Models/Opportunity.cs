using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationDbContext.Models
{
    public partial class Opportunity
    {
        public int OpportunityId { get; set; }
        public int StatusId { get; set; }
        public string Office { get; set; }
        public string Description { get; set; }
        public int EmploymentId { get; set; }
        public double? Salary { get; set; }
        public int Experience { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CandidateId { get; set; }
        public int CompanyId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
