using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationDbContext.Models
{
    public partial class OpportunityList
    {
        [Key]
        public int OpportunityId { get; set; }
        public string Office { get; set; }
        public string Description { get; set; }
        public int EmploymentId { get; set; }
        public string EmploymentName { get; set; }
        public int Experience { get; set; }
        public string Local { get; set; }
        public int CompanyId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public double Salary { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public bool IsDeleted { get; set; }
        public string CompanyName { get; set; }
    }
}
