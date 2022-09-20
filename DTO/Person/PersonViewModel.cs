using DTO.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Person
{
    public class PersonViewModel
    {
        public int? PersonId { get; set; }
        public int PersonTypeId { get; set; }
        [Update]
        public string CompanyName { get; set; }
        [Update]
        public string TradeName { get; set; }
        [Update]
        public string Cnpj { get; set; }
        public string CnpjNumbers { get => Cnpj.NumbersOnly(); set => Cnpj = value.NumbersOnly(); }
        [Update]
        public string FullName { get; set; }
        [Update]
        public string Cpf { get; set; }
        public string CpfNumbers { get => Cpf.NumbersOnly(); set => Cpf = value.NumbersOnly(); }
        [Update]
        public string PhoneNumber { get; set; }
        public string PhoneNumber_NumbersOnly { get => PhoneNumber.NumbersOnly(); set => PhoneNumber = value.NumbersOnly(); }
        [Update]
        public string MobileNumber { get; set; }
        public string MobileNumber_NumbersOnly { get => MobileNumber.NumbersOnly(); set => MobileNumber = value.NumbersOnly(); }
        [Update]
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        [Update]
        public bool IsActive { get; set; }

        public bool IsCompany => string.IsNullOrWhiteSpace(Cpf);
        public string Name => IsCompany ? CompanyName : FullName;
        public string AlternativeName => IsCompany ? TradeName : FullName; 
        public string Document => IsCompany ? Cnpj : Cpf;
        public string DocumentFormated => IsCompany ? Cnpj.CNPJMask() : Cpf.CPFMask();

    }
}
