using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Projeto_2___AED_1.src.Consultas;
using Projeto_2___AED_1.src.Funcionarios;

namespace Projeto_2___AED_1.src.Services
{
    class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string user;
        private string password;

        public DBConnect()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.server = "198.100.155.70";
            this.database = "user_aed1";
            this.user = "user_aed1";
            this.password = "12345";

            string connectionString = "SERVER=" + server + ";" + "DATABASE=" +database + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch(MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Falha ao se conectar com o servidor");
                        break;

                    case 1045:
                        Console.WriteLine("Usuário ou Senha inválidos");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public long Insert(string query)
        {
            if(this.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, this.connection);

                cmd.ExecuteNonQuery();
                long id = cmd.LastInsertedId;

                this.CloseConnection();
                return id;
            }
            return -1;
        }

        public long Delete(string query)
        {
            if (this.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, this.connection);

                cmd.ExecuteNonQuery();
                long id = cmd.LastInsertedId;

                this.CloseConnection();
                return id;
            }
            return -1;
        }

        public List<Medico> CarregarMedicos()
        {
            List<Medico> listaDeRetorno = new List<Medico>();

            if (this.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM funcionarios a INNER JOIN medicos b ON b.funcionarios_matricula=a.matricula", this.connection);
                MySqlDataReader dtreader = cmd.ExecuteReader();

                int crm_medico;
                long matricula;
                string nome;
                long cpf;
                double salario;

                while(dtreader.Read())
                {
                    Medico medico = new Medico();
                    matricula = Convert.ToInt64(dtreader["matricula"]);
                    nome = Convert.ToString(dtreader["nome"]);
                    cpf = Convert.ToInt64(dtreader["cpf"]);
                    salario = Convert.ToDouble(dtreader["salario"]);
                    crm_medico = Convert.ToInt32(dtreader["crm"]);

                    medico.AtualizarFuncionario(nome, cpf, salario, matricula);
                    medico.CRM = crm_medico;
                    listaDeRetorno.Add(medico);
                }

                this.CloseConnection();
            }

            return listaDeRetorno;
        }
        public List<Secretaria> CarregarSecretarias()
        {
            List<Secretaria> listaDeRetorno = new List<Secretaria>();

            if(this.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM funcionarios a INNER JOIN secretarias b ON a.matricula = b.funcionarios_matricula", this.connection);
                MySqlDataReader dtreader = cmd.ExecuteReader();

                long matricula;
                string nome;
                long cpf;
                double salario;
                Secretaria secretaria = new Secretaria();
                while (dtreader.Read())
                {
                    matricula = Convert.ToInt64(dtreader["matricula"]);
                    nome = Convert.ToString(dtreader["nome"]);
                    cpf = Convert.ToInt64(dtreader["cpf"]);
                    salario = Convert.ToDouble(dtreader["salario"]);


                    secretaria.AtualizarFuncionario(nome, cpf, salario, matricula);
                    listaDeRetorno.Add(secretaria);
                }

                this.CloseConnection();
            }

            return listaDeRetorno; 
        }

        public List<Paciente> CarregarPacientes()
        {
            List<Paciente> listaDeRetorno = new List<Paciente>();

            if (this.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM pacientes", this.connection);
                MySqlDataReader dtreader = cmd.ExecuteReader();
                
                while (dtreader.Read())
                {
                    bool possui_plano = Convert.ToBoolean(dtreader["possui_plano"]);
                    string data_nascimento = Convert.ToString(dtreader["data_nascimento"]);
                    long cpf = Convert.ToInt64(dtreader["cpf"]);
                    string nome = Convert.ToString(dtreader["nome"]);
                    long matricula = Convert.ToInt64(dtreader["matricula"]);

                    Paciente paciente = new Paciente(nome, cpf, data_nascimento, possui_plano);
                    paciente.SetMatricula(matricula);
                    listaDeRetorno.Add(paciente);
                }

                this.CloseConnection();
            }

            return listaDeRetorno;
        }

        public List<Consulta> CarregarConsultas(List<Paciente> pacientes, List<Medico> medicos)
        {
            List<Consulta> listaDeRetorno = new List<Consulta>();

            if (this.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM consultas", this.connection);
                MySqlDataReader dtreader = cmd.ExecuteReader();

                while (dtreader.Read())
                {
                    long id_consulta = Convert.ToInt64(dtreader["id"]);
                    int crm_medico = Convert.ToInt32(dtreader["medicos_crm"]);
                    int id_paciente = Convert.ToInt32(dtreader["pacientes_matricula"]);

                    foreach(Paciente x in pacientes)
                    {
                        if(x.GetID() == id_paciente)
                        {
                            foreach(Medico y in medicos)
                            {
                                if(y.GetCRM() == crm_medico)
                                {
                                    Consulta consulta = new Consulta(x, y, false);
                                    consulta.SetID(id_consulta);

                                    listaDeRetorno.Add(consulta);
                                    break;
                                }
                            }
                        }
                    }
                }
                this.CloseConnection();
            }

            return listaDeRetorno;
        }

        public void Update(string query)
        {
            if (this.OpenConnection())
            {

                this.CloseConnection();
            }
        }
    }
}
