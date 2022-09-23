using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationDbContext.Models
{
    public partial class UserList
    {
        [Key]
        public int UserId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cnpj { get; set; }
        public string Cpf { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
