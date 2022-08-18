using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VenturaHR.Models
{
    public class CandidatoViewModel
    {
        public int CandidatoId { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Qualificacao { get; set; }
    }
}
