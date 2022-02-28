using Projeto_2___AED_1.src.Funcionarios;
using System.Collections.Generic;
using System;
using Projeto_2___AED_1.src.Services;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using Projeto_2___AED_1.src.Consultas;

namespace Projeto_2___AED_1
{
    class Program
    {
        static void Main(string[] args)
        {
            DBConnect connect = new DBConnect();
            Console.WriteLine("Carregando médicos...");
            List<Medico> medicos = connect.CarregarMedicos();
            Console.WriteLine("Carregando secretárias...");
            List<Secretaria> secretarias = connect.CarregarSecretarias();
            Console.WriteLine("Carregando pacientes...");
            List<Paciente> pacientes = connect.CarregarPacientes();
            Console.WriteLine("Carregando consultas...");
            List<Consulta> consultas = connect.CarregarConsultas(pacientes, medicos);
            Console.Clear();
            
            ReturnERROR:
            Console.WriteLine("==========================================");
            Console.WriteLine("=============[Menu principal]=============\n");
            Console.WriteLine("1 - Entrar como secretária");
            Console.WriteLine("2 - Entrar como médico");
            Console.Write("\nDigite a opção desejada: ");

            string optionSTR = Console.ReadLine();
            int option;
            bool isNumber = Int32.TryParse(optionSTR, out option);
            if (!isNumber) //Caso ele digita algo que não seja numérico
            {
                Console.Clear();
                goto ReturnERROR;
            }

            Console.WriteLine(option);

            switch (option)
            {
                case 1:
                    Console.Clear();
                    MenuSecretaria();
                    Console.Clear();
                    goto ReturnERROR;
                case 2:
                    VoltarMedico:
                    Console.Clear();

                    Console.WriteLine("- Lista dos médicos");
                    foreach(Medico medico in medicos)
                        Console.WriteLine("CRM:["+medico.GetCRM()+"] - Nome:["+medico.GetName()+"]");
                    Console.Write("\nDigite o seu CRM: ");
                    optionSTR = Console.ReadLine();
                    isNumber = Int32.TryParse(optionSTR, out option);
                    if (!isNumber)
                    {
                        Console.WriteLine("Você digitou algo inválido!");
                        Console.ReadKey();
                        goto VoltarMedico;
                    }

                    Medico medico_AUX = GetMedico(option);
                    if (medico_AUX == null)
                    {
                        Console.WriteLine("Médico não encontrado!");
                        Console.ReadKey();
                        goto VoltarMedico;
                    }
                    else
                    {
                        Console.Clear();
                        MenuMedico(medico_AUX, consultas);
                    }
                    Console.Clear();
                    goto ReturnERROR;
                case 3:
                    Console.WriteLine("Entrar como administrador");
                    break;
                default: //Caso digite uma opção inválida
                    Console.Clear();
                    goto ReturnERROR;
            }
            
            void MenuSecretaria()
            {
                VoltarLista:
                Console.WriteLine("==========================================");
                Console.WriteLine("=============[Menu SECRETÁRIA]=============\n");
                Console.WriteLine("1 - Cadastrar paciente");
                Console.WriteLine("2 - Marcar consulta");
                Console.WriteLine("3 - Desmarcar consulta");
                Console.WriteLine("4 - Visualizar agenda");
                Console.WriteLine("5 - Voltar");
                Console.Write("\nDigite a opção desejada: ");

                optionSTR = Console.ReadLine();
                isNumber = Int32.TryParse(optionSTR, out option);
                if (!isNumber)
                {
                    Console.Clear();
                    goto VoltarLista;
                }

                switch (option)
                {
                    case 1: //Cadastrar paciente
                        {
                            Console.WriteLine("\n# Cadastrando paciente\n");

                            VoltarNome:
                            Console.Write("Digite o nome do paciente: ");
                            string nome = Console.ReadLine();
                            if (nome == "")
                            {
                                if (TentarNovamente("Você digitou um nome errado, deseja continuar (Y/N) ?\n"))
                                    goto VoltarNome;
                                else
                                    goto VoltarLista;
                            }

                            VoltarCPF:
                            Console.Write("Digite o CPF do paciente: ");
                            string cpf = Console.ReadLine();
                            if (cpf == "")
                            {
                                if (TentarNovamente("Você digitou um CPF errado, deseja continuar? (Y/N)\n"))
                                    goto VoltarCPF;
                                else
                                    goto VoltarLista;
                            }

                            VoltarNascimento:
                            Console.Write("Digite a data de nascimento do paciente: ");
                            string nascimento = Console.ReadLine();
                            if (nascimento == "")
                            {
                                if (TentarNovamente("Você digitou uma data errada, deseja continuar? (Y/N)\n"))
                                    goto VoltarNascimento;
                                else
                                    goto VoltarLista;
                            }

                            VoltarPlano:
                            Console.Write("O paciente possuí plano de saúde? (Y/N): ");
                            string plano = Console.ReadLine();
                            if (plano == "" || (plano != "Y" && plano != "N"))
                            {
                                if (TentarNovamente("Você digitou uma informação inválida, deseja continuar? (Y/N)\n"))
                                    goto VoltarPlano;
                                else
                                    goto VoltarLista;
                            }
                            bool possui_plano;
                            if (plano.ToUpper() == "Y")
                                possui_plano = true;
                            else
                                possui_plano = false;

                            Paciente paciente = new Paciente(nome, long.Parse(cpf), nascimento, possui_plano, true);
                            Console.WriteLine("# PACIENTE CADASTRADO COM SUCESSO! PRESSIONE QUALQUER TECLA PARA CONTINUAR\n");
                            pacientes.Add(paciente);
                            Console.ReadKey(true);
                            Console.Clear();
                            goto VoltarLista;
                        }
                    case 2: //Agendar consulta
                        {
                            Console.WriteLine("\nPacientes cadastrados:");
                            foreach(Paciente paciente in pacientes)
                            {
                                Console.WriteLine("Matricula:["+paciente.GetID()+"] - Paciente:["+paciente.GetNome()+"]");
                            }
                            Console.Write("\nDigite a matricula do paciente que você deseja marcar a consulta: ");
                            optionSTR = Console.ReadLine();
                            isNumber = Int32.TryParse(optionSTR, out option);
                            if (!isNumber)
                            {
                                Console.WriteLine("Você digitou algo inválido!");
                                Console.ReadKey();
                                Console.Clear();
                                goto VoltarLista;
                            }

                            Paciente pacienteEX = GetPaciente(option);
                            if(pacienteEX == null)
                            {
                                Console.WriteLine("Você digitou um paciente inválido!");
                                Console.ReadKey(true);
                                Console.Clear();
                                goto VoltarLista;
                            }
                            else
                            {
                                VoltarMedico:
                                Console.WriteLine("\n- Marcando consulta para o paciente: " + pacienteEX.GetNome());
                                foreach(Medico medico in medicos)
                                {
                                    Console.WriteLine("CRM:["+medico.GetCRM()+"] - Médico:["+medico.GetName()+"]");
                                }
                                Console.Write("\nDigite o CRM do médico que vai atende-lo: ");

                                optionSTR = Console.ReadLine();
                                isNumber = Int32.TryParse(optionSTR, out option);
                                if (!isNumber)
                                {
                                    Console.WriteLine("Você digitou algo inválido!");
                                    Console.ReadKey();
                                    goto VoltarMedico;
                                }

                                Medico medico_AUX = GetMedico(option);
                                if(medico_AUX == null)
                                {
                                    Console.WriteLine("Médico não encontrado!");
                                    Console.ReadKey();
                                    goto VoltarMedico;
                                }
                                else
                                {
                                    Consulta consulta = new Consulta(pacienteEX, medico_AUX);
                                    consultas.Add(consulta);
                                    Console.WriteLine("A consulta do paciente " + pacienteEX.GetNome() + " com o médico " + medico_AUX.GetName() + " foi marcada com sucesso!");
                                    Console.ReadKey(true);
                                    Console.Clear();
                                    goto VoltarLista;
                                }
                            }
                        }
                    case 3:
                        {
                            if(consultas.Count == 0)
                            {
                                Console.WriteLine("Não tem nenhuma consulta marcada");
                                Console.ReadKey(true);
                                Console.Clear();
                                goto VoltarLista;
                            }

                            Console.WriteLine("\n- Consultas marcadas:");
                            foreach(Consulta consulta in consultas)
                            {
                                Console.WriteLine("ID:["+consulta.GetID()+"] - Médico:["+consulta.GetMedico().GetName()+"] Paciente:["+consulta.GetPaciente().GetNome()+"]");
                            }
                            Console.Write("Digite o ID da consulta que você deseja remover: ");

                            optionSTR = Console.ReadLine();
                            isNumber = Int32.TryParse(optionSTR, out option);
                            if (!isNumber)
                            {
                                Console.WriteLine("Digite apenas número!");
                                Console.ReadKey();
                                goto VoltarLista;
                            }

                            Consulta consultaSelect = null;

                            foreach(Consulta consulta in consultas)
                            {
                                if(consulta.GetID() == option)
                                {
                                    consultaSelect = consulta;
                                    break;
                                }
                            }
                            if(consultaSelect == null)
                            {
                                Console.WriteLine("Não foi possível encontrar a consulta que você digitou");
                                Console.ReadKey(true);
                                Console.Clear();
                                goto VoltarLista;
                            }
                            else
                            {
                                Console.WriteLine("Consulta desmarcada com sucesso!");
                                consultas.Remove(consultaSelect);
                                consultaSelect.Desmarcar();
                                consultaSelect = null;
                                Console.ReadKey(true);
                                Console.Clear();
                                goto VoltarLista;
                            }
                        }
                    case 4: //Listar a agenda dos médicos
                        {
                            Console.WriteLine("\n- Médicos disponíveis:");
                            foreach (Medico medico in medicos)
                            {
                                Console.WriteLine("CRM:[" + medico.GetCRM() + "] - Médico:[" + medico.GetName() + "]");
                            }
                            Console.Write("\nDigite o CRM do médico para visualizar a agenda: ");

                            optionSTR = Console.ReadLine();
                            isNumber = Int32.TryParse(optionSTR, out option);
                            if (!isNumber)
                            {
                                Console.WriteLine("Você digitou algo inválido!");
                                Console.ReadKey();
                                goto VoltarLista;
                            }

                            Medico medico_AUX = GetMedico(option);
                            if(medico_AUX == null)
                            {
                                Console.WriteLine("Médico não encontrado");
                                Console.ReadKey(true);
                                goto VoltarLista;
                            }
                            else
                            {
                                int count = 0;
                                Console.WriteLine("- Listando as consultas do médico: " + medico_AUX.GetName());
                                foreach(Consulta consulta in consultas)
                                {
                                    if(consulta.GetMedico() == medico_AUX)
                                    {
                                        Console.WriteLine("- "+consulta.GetID()+": "+consulta.GetPaciente().GetNome()+"");
                                        count++;
                                    }
                                }
                                if (count == 0)
                                    Console.Write("\nEsse médico não tem nenhuma consulta marcada!");
                                else
                                    Console.Write("\nEsse médico tem " + count + " consultas marcadas!");

                                Console.ReadKey(true);
                                Console.Clear();
                                goto VoltarLista;
                            }
                        }
                    case 5:
                        break;
                    default:
                        {
                            Console.Clear();
                            goto VoltarLista;
                        }
                }
            }

            Medico GetMedico(long CRM)
            {
                foreach(Medico medico in medicos)
                {
                    if(medico.GetCRM() == CRM)
                    {
                        return medico;
                    }
                }
                return null;
            }

            Paciente GetPaciente(long id)
            {
                foreach(Paciente paciente in pacientes)
                {
                    if(paciente.GetID() == id)
                    {
                        return paciente;
                    }
                }
                return null;
            }
        }
        private static bool TentarNovamente(string message)
        {
            Console.Write(message);
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key.ToString().ToUpper() == "Y")
                return true;
            else
                return false;
        }

        private static void MenuMedico(Medico medico, List<Consulta> consultas)
        {
            Console.WriteLine("Seja bem vindo, doutor " + medico.GetName() + "!");
            Console.ReadKey(true);
            Console.Clear();

            ReturnERROR:
            Console.WriteLine("==========================================");
            Console.WriteLine("==============[Menu MÉDICO]===============\n");
            Console.WriteLine("1 - Visualizar agenda");
            Console.WriteLine("2 - Voltar");
            Console.Write("\nDigite a opção desejada: ");

            string optionSTR = Console.ReadLine();
            int option;
            bool isNumber = Int32.TryParse(optionSTR, out option);
            if (!isNumber)
            {
                Console.Clear();
                goto ReturnERROR;
            }
            
            switch(option)
            {
                case 1:
                    {
                        Console.WriteLine("\n- Minhas agenda: ");
                        int count = 0;
                        foreach(Consulta consulta in consultas)
                        {
                            if(consulta.GetMedico() == medico)
                            {
                                Console.WriteLine("Número " + consulta.GetID() + ": " + consulta.GetPaciente().GetNome());
                                count++;
                            }
                        }
                        if(count == 0)
                        {
                            Console.WriteLine("Você não tem nenhuma consulta marcada!");
                            Console.ReadKey(true);
                            Console.Clear();
                            goto ReturnERROR;
                        }
                        else
                        {
                            Console.Write("\nVocê deseja atender alguma das consultas acima? (Y/N)");

                            if(Console.ReadKey(true).Key == ConsoleKey.Y)
                            {
                                ReturnConsulta:
                                Console.Write("\nDigite o número da consulta que você deseja atender: ");

                                optionSTR = Console.ReadLine();
                                isNumber = Int32.TryParse(optionSTR, out option);
                                if (!isNumber)
                                    goto ReturnConsulta;

                                Consulta consultaselect = null;
                                foreach(Consulta consulta in consultas)
                                {
                                    if(consulta.GetID() == option && consulta.GetMedico() == medico)
                                    {
                                        consultaselect = consulta;
                                        break;
                                    }
                                }
                                if(consultaselect == null)
                                {
                                    Console.WriteLine("O número da consulta não foi encontrado.");
                                    Console.ReadKey(true);
                                    goto ReturnConsulta;
                                }

                                consultaselect.IniciarConsulta();

                                consultas.Remove(consultaselect);
                                consultaselect = null;
                            }
                            else
                            {
                                Console.Clear();
                                goto ReturnERROR;
                            }
                        }
                        goto ReturnERROR;
                    }
                case 2:
                    {
                        break;
                    }
                default:
                    Console.Clear();
                    goto ReturnERROR;
            }
        }
    }
}
