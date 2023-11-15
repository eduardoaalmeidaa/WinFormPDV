using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDV.Relatorios
{
    public class Cliente
    {
        public int IDCliente { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string ValorAberto { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string StatusCliente { get; set; }
        public string Inadimplente { get; set; }
        public string Endereco { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
