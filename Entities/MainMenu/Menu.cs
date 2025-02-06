namespace Sistema_de_Biblioteca.Entities.MainMenu
{
    // Menu Principal do Programa
    internal static class Menu
    {
        public static void MainMenu()
        {
            try
            {
                Console.Clear();

                Console.WriteLine("Olá, seja bem-vindo(a) ao meu Sistema de Biblioteca!");
                Console.WriteLine("Por favor, selecione entre 1-7");

                // Menu para os Usuários (1-3)
                Console.WriteLine("1 - Cadastrar Usuário");
                Console.WriteLine("2 - Visualizar Usuários Cadastrados");
                Console.WriteLine("3 - Deletar Usuário Cadastrado");
                // Menu para os Livros (4-6)
                Console.WriteLine("4 - Cadastrar Livro");
                Console.WriteLine("5 - Visualizar Livros Cadastrados ou Realizar/Verificar um Empréstimo");
                Console.WriteLine("6 - Deletar Livro Cadastrado");
                Console.WriteLine("7 - Sair");
                if (!int.TryParse(Console.ReadLine(), out int op))
                {
                    Console.WriteLine("Por favor, insira um número válido!");
                    return;
                }

                switch (op)
                {
                    // Menu para os Usuários (1-3)
                    case 1: Usuario.NovoUsuario(); break;
                    case 2: Usuario.UsuariosCadastrados(); break;
                    case 3: Usuario.DeletarUsuarios(); break;
                    // Menu para os livros (4-6)
                    case 4: Livro.NovoLivro(); break;
                    case 5: Livro.LivrosCadastrados(); break;
                    case 6: Livro.DeletarLivros(); break;

                    case 7: Environment.Exit(0); break;
                    default: Console.WriteLine("Opção inválida!"); break;
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine("Erro: " + e.Message);
            }
        }
    }
}
