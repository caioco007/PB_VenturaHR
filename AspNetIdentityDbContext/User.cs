using Microsoft.AspNetCore.Identity;
using System;

namespace AspNetIdentityDbContext
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? PersonId { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
