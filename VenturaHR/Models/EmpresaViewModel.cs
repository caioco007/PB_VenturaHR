using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VenturaHR.Models
{
    public class EmpresaViewModel
    {
        public int EmpresaId { get; set; }
        public string Nome { get; set; }
        public string CNPJ { get; set; }
        public string Area { get; set; }
        public string RazaoSocial { get; set; }
    }
}
