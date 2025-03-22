using System.Globalization;
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
        protected Usuario()
        {

        }
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

                Console.Write("Identifique o usuário com um ID: ");
                int id = int.Parse(Console.ReadLine());
                if (id < 0) throw new LibraryExceptions("ID inválido!"); // IDs só podem ser 0 ou maior que eles

                // verificando se já não existe nenhum usuário com o mesmo ID
                if (Usuarios.Any(u => u.Id == id)) throw new LibraryExceptions("Já existe um usuário com esse ID!");

                Console.Write("Digite o nome do usuário: ");
                string? nome = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nome)) throw new LibraryExceptions("O nome do usuário não pode ser vazio ou conter espaços em branco!");

                Console.Write("Digite a data de nascimento do usuário: ");
                DateTime dataNascimento = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

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

                if (Usuarios.Count == 0) throw new LibraryExceptions("Não há usuários cadastrados!");

                // Listando os usuários
                ListarUsuarios(Usuarios);

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

                // Exibe todos os usuários cadastrados (antes de qualquer remoção) para verificar
                ListarUsuarios(Usuarios);

                // removendo o usuário pelo seu ID
                Console.Write("Digite o ID para remover o usuário: ");
                int id = int.Parse(Console.ReadLine());
                if (id < 0) throw new LibraryExceptions("ID invalido!");

                // Agora, a int id ou é nula ou o id armazenado nela for diferente do id do usuário
                // Código anterior: if (usuario == null) throw new LibraryExceptions("Usuário não encontrado!")
                var usuario = Usuarios.FirstOrDefault(u => u.Id == id);
                if (usuario == null || id != usuario.Id) Console.WriteLine("Usuário não encontrado! Verifique se o ID está digitado corretamente.");

                Usuarios.Remove(usuario);
                Console.WriteLine("Usuário removido com sucesso!");

                Console.WriteLine();

                // Exibe todos os IDs cadastrados (depois da remoção) para verificar
                ListarUsuarios(Usuarios);
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

        // 100% funcional
        private static void ListarUsuarios(IEnumerable<Usuario> usuarios)
        {
            Console.WriteLine("USUÁRIOS CADASTRADOS:");
            foreach (var u in usuarios)
            {
                Console.WriteLine($"ID: {u.Id} " +
                                  $"| Nome : {u.Nome} " +
                                  $"| Data de Nascimento: {u.DataNascimento.ToString("dd/MM/yyyy")}");
                // agora, só exibe a data (antes exibia um horário)
            }
            Console.WriteLine();
        }
    }
}