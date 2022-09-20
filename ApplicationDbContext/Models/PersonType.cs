using System;
using System.Collections.Generic;

namespace ApplicationDbContext.Models
{
    public partial class PersonType
    {
        public int PersonTypeId { get; set; }
        public string Name { get; set; }
        public string ExternalCode { get; set; }
    }
}
