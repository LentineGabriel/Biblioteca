﻿using System.Globalization;
using Sistema_de_Biblioteca.Entities.MainMenu;
using Sistema_de_Biblioteca.Exceptions;

namespace Sistema_de_Biblioteca.Entities
{
    public class Usuario
    {
        protected static readonly List<Usuario> Usuarios = new();
        internal int Id { get; }
        internal string? Nome { get; }
        private DateTime DataNascimento { get; }

        // verificar o uso desse primeiro construtor posteriormente
        protected Usuario() { }

        private Usuario(int id, string? nome, DateTime dataNascimento)
        {
            Id = id;
            Nome = nome;
            DataNascimento = dataNascimento;
        }

        // 100% funcional
        protected internal static void NovoUsuario()
        {
            try
            {
                Console.Clear();
                int id = LerIdUsuario();
                VerificarIdExistente(id);
                string? nome = LerNomeUsuario();
                DateTime dataNascimento = LerDataNascimento();

                // adicionando em uma lista
                // Agora, não é necessário uma variável p/ criar um novo usuário, ele vai direto para a lista.
                // Código anterior: var novoUsuario = new Usuario(id, nome, dataNascimento);
                Usuarios.Add(new Usuario(id, nome, dataNascimento));
                Console.WriteLine("Usuário adicionado à lista! Aguarde...");
                Thread.Sleep(2000);
                Menu.MainMenu();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Erro: Nenhuma entrada foi fornecida. {e.Message}");
            }
            catch (OverflowException e)
            {
                Console.WriteLine($"Erro: O número inserido é muito grande. {e.Message}");
            }
            catch (ThreadInterruptedException e)
            {
                Console.WriteLine($"Erro: O processo de espera foi interrompido. {e.Message}");
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Erro: Entrada inválida! Certifique-se de inserir um número para o ID e/ou uma data válida. {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
            }
        }

        // 100% funcional
        protected internal static void UsuariosCadastrados()
        {
            try
            {
                Console.Clear();
                if (!Usuarios.Any()) throw new LibraryExceptions("Não há usuários cadastrados!");
                ListarUsuarios();
                ExibirOpcoesMenu();
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Erro: Entrada inválida! Insira um número válido. {e.Message}");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Erro: Nenhuma entrada foi fornecida. {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
            }
        }

        // 100% funcional
        protected internal static void DeletarUsuarios()
        {
            try
            {
                Console.Clear();
                ListarUsuarios();
                int id = LerIdUsuario();
                var usuario = Usuarios.FirstOrDefault(u => u.Id == id);
                if (usuario == null) throw new LibraryExceptions("Usuário não encontrado! Verifique se o ID está digitado corretamente.");

                Usuarios.Remove(usuario);
                Console.WriteLine("Usuário removido com sucesso!");
                ListarUsuarios();
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Erro: Entrada inválida! O ID deve ser um número. {e.Message}");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Erro: nenhuma entrada foi fornecida. {e.Message}");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"Erro: Operação inválida ao buscar o usuário. {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
            }
        }

        private static int LerIdUsuario()
        {
            Console.Write("Identifique o usuário com um ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id) || id < 0) throw new LibraryExceptions("ID inválido!");
            return id;
        }

        private static void VerificarIdExistente(int id)
        {
            if (Usuarios.Any(u => u.Id == id)) throw new LibraryExceptions("Já existe um usuário com este ID!");
        }

        private static string LerNomeUsuario()
        {
            Console.Write("Digite o nome do usuário: ");
            string? nome = Console.ReadLine();
            if (string.IsNullOrEmpty(nome)) throw new LibraryExceptions("O nome do usuário não pode ser vazio!");
            return nome;
        }

        private static DateTime LerDataNascimento()
        {
            Console.Write("Digite a data de nascimento do usuário (dd/MM/yyyy): ");
            return DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        // 100% funcional
        private static void ListarUsuarios()
        {
            Console.WriteLine("USUÁRIOS CADASTRADOS:");
            foreach (var u in Usuarios)
            {
                Console.WriteLine($"ID: {u.Id} " +
                                  $"| Nome : {u.Nome} " +
                                  $"| Data de Nascimento: {u.DataNascimento.ToString("dd/MM/yyyy")}");
                // agora, só exibe a data (antes exibia um horário)
            }
            Console.WriteLine();
        }

        private static void ExibirOpcoesMenu()
        {
            Console.WriteLine("O que deseja fazer?");
            Console.WriteLine("1 - Voltar ao Menu Principal");
            Console.WriteLine("2 - Realizar/Verificar um Empréstimo/Registrar uma Devolução");
            Console.WriteLine("3 - Sair");
            if (!int.TryParse(Console.ReadLine(), out int op))
            {
                Console.WriteLine("Por favor, insira um número válido!");
                return;
            }

            switch (op)
            {
                case 1: Menu.MainMenu(); break;
                case 2: Emprestimo.Menu(); break;
                case 3: Environment.Exit(0); break;
                default: Console.WriteLine("Número inválido!"); break;
            }
        }
    }
}