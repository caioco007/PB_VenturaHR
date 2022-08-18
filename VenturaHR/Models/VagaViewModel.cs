using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VenturaHR.Models
{
    public class VagaViewModel
    {
        public int Id { get; set; }
        public string Cargo { get; set; }
        public string FormaContratacao { get; set; }
        public DateTime DataExpiracao { get; set; }
    }
}
