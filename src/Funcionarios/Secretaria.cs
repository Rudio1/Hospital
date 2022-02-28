using Projeto_2___AED_1.src.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_2___AED_1.src.Funcionarios
{
    class Secretaria:Funcionario
    {
        /*public Secretaria(string fName, long fCPF, double fSalario, bool register = false) : base(fName, fCPF, fSalario, register) 
        {
            if (register)
                this.RegisterSecretaria();
        }*/

        private void RegisterSecretaria()
        {
            DBConnect connection = new DBConnect();
            connection.Insert("INSERT INTO secretarias(funcionarios_matricula) VALUES(" + this.matricula + ");");
        }
    }
}
