using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationDbContext.Models
{
    public partial class Person
    {
        public int PersonId { get; set; }
        public int PersonTypeId { get; set; }
        public string CompanyName { get; set; }
        public string TradeName { get; set; }
        public string Cnpj { get; set; }
        public string FullName { get; set; }
        public string Cpf { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public bool IsActive { get; set; }
        public string BlockReason { get; set; }
        public DateTime? BlockDate { get; set; }
    }
}
