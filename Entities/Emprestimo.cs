using System.Globalization;
using Sistema_de_Biblioteca.Exceptions;
using static Sistema_de_Biblioteca.Services.Atraso;

namespace Sistema_de_Biblioteca.Entities;

public abstract class Emprestimo : Usuario
{
    private static readonly List<RegistroEmprestimo> RegistroEmprestimo = new();
    private static readonly DateTime HoraDoEmprestimo = DateTime.Now;
    private static readonly DateTime HoraDeDevolucao = HoraDoEmprestimo.AddDays(30); // 30 dias p/ devolução

    // 100% funcional - Menu de Empréstimo
    protected internal static void Menu()
    {
        try
        {
            Console.Clear();

            Console.WriteLine("LIVROS CATALOGADOS:");
            foreach (var l in Livro.Livros)
            {
                Console.WriteLine($"Nome: {l.NomeLivro} " +
                                  $"| Autor: {l.AutorLivro} " +
                                  $"| Ano de Publicação: {l.AnoPublicacao} " +
                                  $"| Quantidade copias: {l.QuantidadeCopias}");
            }
            Console.WriteLine();

            Console.WriteLine("1 - Realizar um Emprestimo");
            Console.WriteLine("2 - Verificar um Emprestimo");
            Console.WriteLine("3 - Registrar uma Devolução");
            if (!int.TryParse(Console.ReadLine(), out int op))
            {
                Console.WriteLine("Por favor, insira um número válido!");
                return;
            }

            switch (op)
            {
                case 1: RealizarEmprestimo(); break;
                case 2: VerificarEmprestimo(); break;
                case 3: RegistrarDevolucao(); break;
                default: Console.WriteLine("Número inválido"); break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
        }
    }

    // 100% funcional
    private static void RealizarEmprestimo()
    {
        try
        {
            Console.Clear();

            // Escolhendo um usuário pelo ID
            Console.Write("Digite o ID do usuário para realizar o emprestimo: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
            {
                Console.WriteLine("ID inválido. Tente novamente: ");
            }

            var usuario = Usuarios.Find(u => u.Id == id);
            if (usuario == null) throw new LibraryExceptions("Usuário não encontrado!");
            Console.WriteLine($"Usuário encontrado: {usuario.Id} | {usuario.Nome}");

            // Verificar se o usuário já possui empréstimos pendentes
            var emprestimosPendentes = RegistroEmprestimo.Where(e => e.UsuarioId == id && e.QuantidadeEmprestimo > 0).ToList();
            if (emprestimosPendentes.Any())
            {
                Console.WriteLine("Este usuário possui empréstimos pendentes e não pode realizar um novo empréstimo até que os anteriores sejam devolvidos.");
                return;
            }

            Console.WriteLine();

            Console.WriteLine("LIVROS CATALOGADOS:");
            foreach (var l in Livro.Livros.OrderBy(l => l.NomeLivro))
            {
                Console.WriteLine($"Nome: {l.NomeLivro} " +
                                  $"| Autor: {l.AutorLivro} " +
                                  $"| Ano de Publicação: {l.AnoPublicacao} " +
                                  $"| Quantidade copias: {l.QuantidadeCopias}");
            }
            Console.WriteLine();

            Console.Write("Digite o nome do livro: ");
            string? nomeLivro = Console.ReadLine();

            var livroEmprestado = Livro.Livros.Find(l => l.NomeLivro == nomeLivro);
            if (livroEmprestado == null) throw new LibraryExceptions("Livro não encontrado!");

            Console.Write("Digite a quantidade que deseja emprestar: ");
            int quantidade = int.Parse(Console.ReadLine());
            if (quantidade <= 0 || livroEmprestado.QuantidadeCopias < quantidade) throw new LibraryExceptions("Quantidade inválida!");
            livroEmprestado.QuantidadeCopias -= quantidade; // atualizando a quantidade de livros conforme o empréstimo

            var emprestimo = new RegistroEmprestimo(usuario.Id, usuario.Nome, livroEmprestado.NomeLivro, quantidade, HoraDoEmprestimo, HoraDeDevolucao);
            RegistroEmprestimo.Add(emprestimo);
            Console.WriteLine("Livro emprestado com sucesso!");

            Console.WriteLine();

            Console.WriteLine("DETALHES DO EMPRÉSTIMO: ");
            Console.WriteLine($"Livro emprestado: {livroEmprestado.NomeLivro}, de {livroEmprestado.AutorLivro}." +
                              $"\nEmprestado ao ID: {usuario.Id} | Nome: {usuario.Nome}" +
                              $"\nHora do empréstimo: {HoraDoEmprestimo} | Devolução: {HoraDeDevolucao}");
            Console.WriteLine();

            Console.WriteLine("Voltar ao menu principal? (s/n)");
            if (!char.TryParse(Console.ReadLine(), out char op))
            {
                Console.WriteLine("Por favor, insira um caractere válido!");
                return;
            }

            switch (op)
            {
                case 's': MainMenu.Menu.MainMenu(); break;
                case 'n': Environment.Exit(0); break;
                default: Console.WriteLine("Opção inválida!"); break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
        }
    }

    // 100 % funcional
    private static void VerificarEmprestimo()
    {
        try
        {
            Console.Clear();

            Console.Write("Digite o ID do usuário para verificar os empréstimos: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
            {
                Console.WriteLine("ID inválido. Tente novamente: ");
            }

            var usuario = Usuarios.Find(u => u.Id == id);
            if (usuario == null) throw new LibraryExceptions("ID não encontrado!");

            Console.WriteLine();

            // Verificando se o usuário tem empréstimos no seu nome
            var userEmprestimo = RegistroEmprestimo.Where(e => e.UsuarioId == id).ToList();
            if (!userEmprestimo.Any()) throw new LibraryExceptions("Nenhum empréstimo encontrado com este usuário.");

            Console.WriteLine($"ID: {usuario.Id} | Nome: {usuario.Nome}");
            Console.WriteLine("LIVROS EMPRESTADOS: ");

            foreach (var ue in userEmprestimo)
            {
                Console.WriteLine($"Livro: {ue.NomeLivro} | Quantidade {ue.QuantidadeEmprestimo} " +
                                  $"| Data do Empréstimo: {ue.DataEmprestimo} | Data do Devolucao: {ue.DataDevolucao}");
            }
            Console.WriteLine();

            Console.WriteLine("Voltar ao menu principal? (s/n)");
            if (!char.TryParse(Console.ReadLine(), out var op))
            {
                Console.WriteLine("Por favor, insira um caractere válido!");
                return;
            }

            switch (op)
            {
                case 's': MainMenu.Menu.MainMenu(); break;
                case 'n': Environment.Exit(0); break;
                default: Console.WriteLine("Opção inválida!"); break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
        }
    }

    private static void RegistrarDevolucao()
    {
        try
        {
            Console.Clear();

            Console.Write("Digite o ID do usuário para verificar se há um empréstimo: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
            {
                Console.WriteLine("ID inválido. Tente novamente: ");
            }

            // Filtrando todos os empréstimos do usuário
            var emprestimosDoUsuario = RegistroEmprestimo.Where(e => e.UsuarioId == id).ToList();
            if (!emprestimosDoUsuario.Any())
            {
                throw new LibraryExceptions($"Não há empréstimos com este usuário. ID de identificação {id}");
            }
            Console.WriteLine();

            // Pegando o nome do usuário
            var nomeUsuario = emprestimosDoUsuario.FirstOrDefault()?.UsuarioNome; // a ? evita exceções caso a lista esteja vazia

            // Tratamento de nomes ausentes
            if (string.IsNullOrWhiteSpace(nomeUsuario))
            {
                Console.WriteLine($"O nome do usuário com ID {id} não está registrado. Verifique os dados.");
                return;
            }

            Console.WriteLine($"Total de empréstimos: {emprestimosDoUsuario.Count}");
            Console.WriteLine($"Empréstimos com o usuário {id} | {nomeUsuario}:");
            foreach (var eu in emprestimosDoUsuario)
            {
                Console.WriteLine($"Livro: {eu.NomeLivro} | Quantidade emprestada: {eu.QuantidadeEmprestimo}");
            }
            Console.WriteLine();

            Console.Write("Digite o nome do livro para registrar a devolução: ");
            var nomeLivro = Console.ReadLine();

            var buscarEmprestimo = emprestimosDoUsuario.FirstOrDefault(e => e.NomeLivro == nomeLivro);
            if (buscarEmprestimo == null) throw new LibraryExceptions($"Este livro {nomeLivro} não está com este usuário. ID: {id}");

            // verificando e validando as cópias a serem devolvidas
            Console.Write("Quantas cópias irão ser devolvidas? ");
            var quantidade = int.Parse(Console.ReadLine());

            if(quantidade > buscarEmprestimo.QuantidadeEmprestimo) throw new LibraryExceptions("Quantidade para devolução é maior do que foi emprestado");
            if (quantidade <= 0) throw new LibraryExceptions("A quantidade deve ser maior que zero.");

            // Atualizar o registro de empréstimo
            buscarEmprestimo.QuantidadeEmprestimo -= quantidade;

            // Remover registro caso não haja mais livros a serem devolvidos
            if(buscarEmprestimo.QuantidadeEmprestimo == 0) RegistroEmprestimo.Remove(buscarEmprestimo);

            Console.WriteLine();

            // Atualizando a lista de livros
            var livro = Livro.Livros.FirstOrDefault(l => l.NomeLivro == nomeLivro);
            if (livro != null)
            {
                livro.QuantidadeCopias += quantidade;
                Console.WriteLine($"Livro: {livro.NomeLivro} agora possui {livro.QuantidadeCopias} livros disponíveis para empréstimo.");

                Console.WriteLine();

                // Verificando se a data de devolução não é maior que o prazo estipulado
                Console.Write("Data da Devolução: ");
                DateTime dataDevolucao = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                VerificarAtraso(HoraDeDevolucao, dataDevolucao);

                Console.WriteLine("Devolução registrada com sucessso!");
                Console.WriteLine("LISTA DE LIVROS ATUALIZADAS:");
                foreach (var l in Livro.Livros.OrderBy(l => l.NomeLivro))
                {
                    Console.WriteLine($"Nome: {l.NomeLivro} " +
                                      $"| Autor: {l.AutorLivro} " +
                                      $"| Ano de Publicação: {l.AnoPublicacao} " +
                                      $"| Quantidade: {l.QuantidadeCopias}");
                }
            }
            else throw new LibraryExceptions("Livro não encontrado no catálogo!");
            Console.WriteLine();

            Console.WriteLine("Voltar ao menu principal? (s/n)");
            if (!char.TryParse(Console.ReadLine(), out var op))
            {
                Console.WriteLine("Por favor, insira um caractere válido!");
                return;
            }

            switch (op)
            {
                case 's': MainMenu.Menu.MainMenu(); break;
                case 'n': Environment.Exit(0); break;
                default: Console.WriteLine("Opção inválida!"); break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
        }
    }
}