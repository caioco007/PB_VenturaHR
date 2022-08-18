using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationDbContext.Models
{
    public partial class Candidato
    {
        public int CandidatoId { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Qualificacao { get; set; }
    }
}
