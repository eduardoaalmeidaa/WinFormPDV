using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDV.Relatorios
{
    public class Funcionario
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public string Cargo { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Observacao { get; set; }
    }
}
