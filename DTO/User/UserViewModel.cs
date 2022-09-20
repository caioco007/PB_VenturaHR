using DTO.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.User
{
    public class UserViewModel
    {
        public int? Id { get; set; }
        [Update]
        public string FirstName { get; set; }
        [Update]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        [Update]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        [Update]
        public int? PersonId { get; set; }
        [Update]
        public DateTime? Birthday { get; set; }
        public string BirthdayFormated { get => Birthday.ToBrazilianDateFormat(); set => Birthday = value.FromBrazilianDateFormatNullable(); }
        [Update]
        public bool? IsActive { get; set; }
        public string IsActiveText => IsActive == true ? "Sim" : "Não";
        public bool IsDeleted { get; set; }
        public string PersonName { get; set; }

    }
}
