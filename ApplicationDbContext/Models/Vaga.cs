using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationDbContext.Models
{
    public partial class Vaga
    {
        public int Id { get; set; }
        public string Cargo { get; set; }
        public string FormaContratacao { get; set; }
        public DateTime DataExpiracao { get; set; }
    }
}
