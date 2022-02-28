using Projeto_2___AED_1.src.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_2___AED_1.src.Funcionarios
{
    abstract class Funcionario
    {
        public long matricula { get; set; }
        private string nome { get; set; }
        private long cpf { get; set; }
        private double Salario { get; set; }
        
        /*public Funcionario(string fName, long fCPF, double fSalario, bool register = false)
        {
            this.nome = fName;
            this.cpf = fCPF;
            this.Salario = fSalario;

            if(register)
                this.RegisterFunc();
        }*/

        public Funcionario(bool register = false)
        {
            if (register)
                RegisterFunc();
        }

        public void AtualizarFuncionario(string fName, long fCPF, double fSalario, long fmatricula)
        {
            this.nome = fName;
            this.cpf = fCPF;
            this.Salario = fSalario;
            this.matricula = fmatricula;
        }

        public void RegisterFunc()
        {
            DBConnect connection = new DBConnect();
            this.matricula = connection.Insert("INSERT INTO funcionarios(nome, cpf, salario) VALUES('" + this.nome + "', " + this.cpf + ", " + this.Salario + ")");
        }

        public string GetName()
        {
            return this.nome;
        }

        public double GetSalario()
        {
            return this.Salario;
        }

    }
}
