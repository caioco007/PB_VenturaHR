using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Account
{
    public class SignInViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
