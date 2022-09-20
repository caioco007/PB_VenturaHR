using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Account
{
    public class PasswordRecoveryEmailViewModel
    {
        public User.UserViewModel User { get; set; }
        public string EmailCallback { get; set; }

        public PasswordRecoveryEmailViewModel(User.UserViewModel user, string emailCallback)
        {
            User = user;
            EmailCallback = emailCallback;
        }
    }
}
