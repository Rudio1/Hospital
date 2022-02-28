using Projeto_2___AED_1.src.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_2___AED_1.src.Funcionarios
{
    class Medico:Funcionario
    {
        public int CRM { get; set; }
        
        public Medico(bool registrar = false)
        {
            if(registrar)
                RegisterMedico();
        }

        private void RegisterMedico()
        {
            DBConnect connection = new DBConnect();
            connection.Insert("INSERT INTO medicos(crm, funcionarios_matricula) VALUES(" + this.CRM+","+this.matricula+");");
        }
        
        public int GetCRM()
        {
            return this.CRM;
        }
        
    }
}
