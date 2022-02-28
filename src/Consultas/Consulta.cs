using Projeto_2___AED_1.src.Funcionarios;
using Projeto_2___AED_1.src.Services;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_2___AED_1.src.Consultas
{
    class Consulta
    {
        private long id;
        private Medico medico;
        private Paciente paciente;
        private string diagnostico;
        public long GetID()
        {
            return this.id;
        }

        public Medico GetMedico()
        {
            return this.medico;
        }

        public Consulta(Paciente pac, Medico med, bool insert = true)
        {
            DBConnect connection = new DBConnect();

            this.medico = med;
            this.paciente = pac;

            if(insert)
            {
                this.diagnostico = "";
                this.id = connection.Insert("INSERT INTO consultas(medicos_crm, pacientes_matricula) VALUES(" + med.GetCRM() + ", " + pac.GetID() + ")");
            }
        }

        private void DeletarConsulta()
        {
            DBConnect connection = new DBConnect();

            connection.Delete("DELETE FROM consultas WHERE id=" + this.GetID());
        }

        public void IniciarConsulta()
        {
            Console.Clear();

            Console.WriteLine("=================[Diagnóstico]=================");
            Console.Write("\nDigite o diagnóstico do paciente: ");
            this.diagnostico = Console.ReadLine();

            Console.WriteLine("\nConsulta finalizada com sucesso!");
            Console.ReadKey(true); 

            try
            {
                string nome_arquivo = this.GetID()+"_"+this.paciente.GetNome().Replace(" ", "")+".txt";

                string path = "C:/Users/Administrator/Desktop/Projeto-2---AED-1/Diagnosticos/"+nome_arquivo;

                using (StreamWriter sw = new StreamWriter(path, append: true))
                {
                    sw.WriteLine("Médico: " + medico.GetName());
                    sw.WriteLine("Paciente: " + paciente.GetNome());
                    sw.WriteLine("Diagnóstico: " + this.diagnostico);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Não foi possível registrar o diagnóstico.");
            }            
        }

        public void Desmarcar()
        {
            DBConnect connection = new DBConnect();

            this.id = connection.Insert("DELETE FROM consultas WHERE id="+this.id);
        }

        public void SetID(long new_id)
        {
            this.id = new_id;
        }

        public Paciente GetPaciente()
        {
            return this.paciente;
        }
    }
}
