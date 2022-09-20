using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Account
{
    public class ResetPasswordViewModel
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string NewPassword2 { get; set; }
        public ResetPasswordViewModel() { }
        public ResetPasswordViewModel(string token, string email)
        {
            this.Token = token;
            this.Email = email;
        }
    }
}
