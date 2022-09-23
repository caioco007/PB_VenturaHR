using DTO.Opportunity;
using DTO.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.User
{
    public class UserListViewModel
    {
        public int UserId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cnpj { get; set; }
        public string CnpjNumbers { get => Cnpj.NumbersOnly(); set => Cnpj = value.NumbersOnly(); }
        public string Cpf { get; set; }
        public string CpfNumbers { get => Cpf.NumbersOnly(); set => Cpf = value.NumbersOnly(); }
        public string PhoneNumber { get; set; }
        public string PhoneNumber_NumbersOnly { get => PhoneNumber.NumbersOnly(); set => PhoneNumber = value.NumbersOnly(); }
        public string MobileNumber { get; set; }
        public string MobileNumber_NumbersOnly { get => MobileNumber.NumbersOnly(); set => MobileNumber = value.NumbersOnly(); }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}