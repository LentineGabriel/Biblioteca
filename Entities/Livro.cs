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

        private Livro(string? nomeLivro, string? autorLivro, int anoPublicacao, int quantidadeCopias)
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
                Console.Write("Digite o nome do livro: ");
                string? nomeLivro = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nomeLivro)) throw new LibraryExceptions("O nome do livro não pode estar vazio!");

                Console.Write("Digite o nome do autor do livro: ");
                string? autorLivro = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(autorLivro)) throw new LibraryExceptions("O nome do autor não pode estar vazio!");

                Console.Write("Digite o ano de publicação do livro: ");
                int anoPublicacao = int.Parse(Console.ReadLine());
                if(anoPublicacao is <= 0 or > 9999) throw new LibraryExceptions("Ano de publicação inválido!");

                Console.Write("Quantidade de copias: ");
                int quantidadeCopias = int.Parse(Console.ReadLine());
                if(quantidadeCopias <= 0) throw new LibraryExceptions("Não há cópias para ser catalogado (não é preciso o registro)");

                // Adicionando em uma lista
                var novoLivro = new Livro(nomeLivro, autorLivro, anoPublicacao, quantidadeCopias);
                Livros.Add(novoLivro);

                Console.WriteLine("Livro catalogado com sucesso! Aguarde...");
                Thread.Sleep(2000);
                Menu.MainMenu();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        // 100% funcional
        protected internal static void LivrosCadastrados()
        {
            try
            {
                Console.Clear();

                // Caso não haja livros catalogados
                if (Livros.Count == 0) throw new LibraryExceptions("Não há livros para serem listados.");
                ListarLivros(Livros.OrderBy(l => l.NomeLivro));

                Console.WriteLine("O que deseja fazer?");
                Console.WriteLine("1 - Realizar/Verificar um Empréstimo/Registrar uma Devolução");
                Console.WriteLine("2 - Voltar ao Menu Principal");
                Console.WriteLine("3 - Sair");
                int op;
                if (!int.TryParse(Console.ReadLine(), out op))
                {
                    Console.WriteLine("Por favor, insira um número válido!");
                    return;
                }

                switch (op)
                {
                    case 1: Emprestimo.Menu(); break;
                    case 2: Menu.MainMenu(); break;
                    case 3: Environment.Exit(0); break;
                    default: Console.WriteLine("Número inválido!"); break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        // 100% funcional
        protected internal static void DeletarLivros()
        {
            Console.Clear();

            // Caso não haja livros catalogados
            if (Livros.Count == 0) throw new LibraryExceptions("Não há livros para serem listados.");

            // Exibe todos os livros cadastrados (antes de qualquer remoção) para verificar
            ListarLivros(Livros.OrderBy(l => l.NomeLivro));

            // removendo o livro pelo seu nome
            Console.Write("Digite o nome do livro para removê-lo: ");
            var nomeLivro = Console.ReadLine();
            if (string.IsNullOrEmpty(nomeLivro)) throw new LibraryExceptions("Insira um nome válido!");

            var livro = Livros.Find(l => l.NomeLivro == nomeLivro);

            if (livro == null) throw new LibraryExceptions("Livro não encontrado!");
            Livros.Remove(livro);
            Console.WriteLine("Livro removido com sucesso!");

            Console.WriteLine();

            // Exibe todos os livros cadastrados (depois da remoção) para verificar
            ListarLivros(Livros);

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal...");
            Console.ReadKey();
            Menu.MainMenu();
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
    }
}
