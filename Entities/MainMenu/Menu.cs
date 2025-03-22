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
                Console.WriteLine("Por favor, selecione entre 1-8");

                // Menu para os Usuários (1-3)
                Console.WriteLine("1 - Cadastrar Usuário");
                Console.WriteLine("2 - Visualizar Usuários Cadastrados");
                Console.WriteLine("3 - Deletar Usuário Cadastrado");

                // Menu para os Livros (4-6)
                Console.WriteLine("4 - Cadastrar Livro");
                Console.WriteLine("5 - Visualizar Livros Cadastrados");
                Console.WriteLine("6 - Deletar Livro Cadastrado");

                // Menu para os Empréstimos
                Console.WriteLine("7 - Realizar/Verificar um Empréstimo");
                // agora, "Realizar/Verificar um Empréstimo" será desacoplada de "Visualizar Livros Cadastrados" no menu, isso causava muita confusão e o texto do campo ficava muito extenso

                Console.WriteLine("8 - Sair");
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

                    // Menu para os Empréstimos
                    case 7: Emprestimo.Menu(); break; // agora, ele vai direto para a verificação

                    // Sair
                    case 8: Environment.Exit(0); break;
                    default: Console.WriteLine("Opção inválida!"); break;
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Erro: Nenhuma entrada foi recebida. {e.Message}");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"Erro: Operação inválida. {e.Message}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"Erro de entrada/saída: {e.Message}");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Erro de permissão: {e.Message}");
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Erro de formatação: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
            }
        }
    }
}
