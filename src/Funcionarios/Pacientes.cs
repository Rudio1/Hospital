using Projeto_2___AED_1.src.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_2___AED_1.src.Funcionarios
{
    class Paciente
    {
        private long matricula { get; set; }
        private string nome { get; set; }
        private long cpf { get; set; }
        private string data_nascimento { get; set; }
        private bool possui_plano { get; set; }

        public Paciente(string pName, long pCPF, string pNascimento, bool plano, bool register = false)
        {
            this.nome = pName;
            this.cpf = pCPF;
            this.data_nascimento = pNascimento;
            this.possui_plano = plano;

            if (register)
                RegisterPaciente();
        }

        public void SetMatricula(long new_Matricula)
        {
            this.matricula = new_Matricula;
        }

        public long GetID()
        {
            return this.matricula;
        }

        public string GetNome()
        {
            return this.nome;
        }
        
        private void RegisterPaciente()
        {
            DBConnect connection = new DBConnect();
            this.matricula = connection.Insert("INSERT INTO pacientes(nome,cpf,data_nascimento,possui_plano) VALUES('"+this.nome+"',"+this.cpf+",'" + this.data_nascimento + "'," + this.possui_plano+")");
        }
    }
}
