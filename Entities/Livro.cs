using Sistema_de_Biblioteca.Entities.MainMenu;
using Sistema_de_Biblioteca.Exceptions;

namespace Sistema_de_Biblioteca.Entities
{
    public class Livro
    {
        public static readonly List<Livro> Livros = new();
        public string? NomeLivro { get; }
        public string? AutorLivro { get; }
        public int AnoPublicacao { get; }
        public int QuantidadeCopias { get; set; }

        internal Livro(string? nomeLivro, string? autorLivro, int anoPublicacao, int quantidadeCopias)
        {
            NomeLivro = nomeLivro;
            AutorLivro = autorLivro;
            AnoPublicacao = anoPublicacao;
            QuantidadeCopias = quantidadeCopias;
        }

        // 100% funcional
        protected internal static void NovoLivro()
        {
            try
            {
                Console.Clear();
                string nomeLivro = LerEntrada("Digite o nome do livro: ");
                string autorLivro = LerEntrada("Digite o nome do autor do livro: ");
                int anoPublicacao = LerNumero("Digite o ano de publicação do livro: ", 1, 9999);
                int quantidadeCopias = LerNumero("Quantidade de cópias: ", 1, int.MaxValue);

                // Adicionando em uma lista
                // Agora, não é necessário uma variável p/ criar um novo livro, ele vai direto para a lista.
                // Código anterior: var novoLivro = new Livro(nomeLivro, autorLivro, anoPublicacao, quantidadeCopias);
                Livros.Add(new Livro(nomeLivro, autorLivro, anoPublicacao, quantidadeCopias));

                Console.WriteLine("Livro catalogado com sucesso! Aguarde...");
                Thread.Sleep(2000);
                Menu.MainMenu();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
            }
        }

        // 100% funcional
        protected internal static void LivrosCadastrados()
        {
            try
            {
                Console.Clear();

                // Caso não haja livros catalogados
                if (!Livros.Any()) throw new LibraryExceptions("Não há livros para serem listados.");
                ListarLivros(Livros.OrderBy(l => l.NomeLivro));

                Console.WriteLine("O que deseja fazer?");
                Console.WriteLine("1 - Realizar/Verificar um Empréstimo");
                Console.WriteLine("2 - Registrar uma Devolução"); // descacoplando da opção 1 para ficar menos verbosa
                Console.WriteLine("3 - Voltar ao Menu Principal");
                Console.WriteLine("4 - Sair");

                int op = LerNumero("Escolha uma opção: ", 1, 4);
                switch (op)
                {
                    case 1 or 2: Emprestimo.Menu(); break;
                    case 3: Menu.MainMenu(); break;
                    case 4: Environment.Exit(0); break;
                    default: Console.WriteLine("Número inválido!"); break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
            }
        }

        // 100% funcional
        protected internal static void DeletarLivros()
        {
            try
            {
                Console.Clear();

                // Caso não haja livros catalogados
                if (!Livros.Any()) throw new LibraryExceptions("Não há livros para serem listados.");

                // Exibe todos os livros cadastrados (antes de qualquer remoção) para verificar
                ListarLivros(Livros.OrderBy(l => l.NomeLivro));

                // removendo o livro pelo seu nome
                string nomeLivro = LerEntrada("Digite o nome do livro para removê-lo: ");
                var livro = Livros.FirstOrDefault(l => l.NomeLivro.Equals(nomeLivro, StringComparison.OrdinalIgnoreCase));

                // Agora, a string livro ou é nula ou o nome armazenado nela for diferente do nome do livro
                // Código anterior: if (livro == null) throw new LibraryExceptions("Livro não encontrado!")
                if (livro == null || nomeLivro != livro.NomeLivro) throw new LibraryExceptions("Livro não encontrado! Verifique se o nome está digitado corretamente.");
                Livros.Remove(livro);
                Console.WriteLine("Livro removido com sucesso!");

                Console.WriteLine();

                // Exibe todos os livros cadastrados (depois da remoção) para verificar
                ListarLivros(Livros);

                Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal...");
                Console.ReadKey();
                Menu.MainMenu();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
            }
        }

        // 100% funcional
        private static void ListarLivros(IEnumerable<Livro> livros)
        {
            Console.WriteLine("LIVROS CATALOGADOS:");
            foreach (var l in livros)
            {
                Console.WriteLine($"Nome: {l.NomeLivro} " +
                                  $"| Autor: {l.AutorLivro} " +
                                  $"| Ano de Publicação: {l.AnoPublicacao} " +
                                  $"| Quantidade de Cópias: {l.QuantidadeCopias}");
            }
            Console.WriteLine();
        }

        private static string LerEntrada(string mensagem)
        {
            Console.Write(mensagem);
            string? entrada = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(entrada)) throw new LibraryExceptions("O campo não pode estar vazio.");
            return entrada;
        }

        private static int LerNumero(string mensagem, int min, int max)
        {
            Console.Write(mensagem);
            if (!int.TryParse(Console.ReadLine(), out int valor) || valor < min || valor > max) throw new LibraryExceptions("Valor inválido! Tente novamente.");
            return valor;
        }
    }
}