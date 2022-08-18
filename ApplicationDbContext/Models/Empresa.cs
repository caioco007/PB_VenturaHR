using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationDbContext.Models
{
    public partial class Empresa
    {
        public int EmpresaId { get; set; }
        public string Nome { get; set; }
        public string CNPJ { get; set; }
        public string Area { get; set; }
        public string RazaoSocial { get; set; }
    }
}
