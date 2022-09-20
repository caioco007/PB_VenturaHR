using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Account
{
    public class RegisterViewModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
