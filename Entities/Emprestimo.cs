﻿using System.Globalization;
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
            int op = LerOpcaoMenu();

            switch (op)
            {
                case 1: RealizarEmprestimo(); break;
                case 2: VerificarEmprestimo(); break;
                case 3: RegistrarDevolucao(); break;
                default: Console.WriteLine("Número inválido!"); break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro: {e.Message}");
        }
    }

    // 100% funcional
    private static void RealizarEmprestimo()
    {
        try
        {
            Console.Clear();

            // Escolhendo um usuário pelo ID
            int id = LerIdUsuario();
            var usuario = Usuarios.Find(u => u.Id == id);
            // Agora, a int id ou é nulo ou o id armazenado nele for diferente do id do usuário
            // Código anterior: if (usuario == null) throw new LibraryExceptions("Usuário não encontrado!")
            if (usuario == null || usuario.Id != id) throw new LibraryExceptions("Usuário não encontrado! Verifique se o ID está digitado corretamente.");

            Console.WriteLine($"Usuário encontrado: {usuario.Id} | {usuario.Nome}");

            // Verificar se o usuário já possui empréstimos pendentes
            /* Código anterior:
            var emprestimosPendentes = RegistroEmprestimo.Where(e => e.UsuarioId == id && e.QuantidadeEmprestimo > 0).ToList();
            if (emprestimosPendentes.Any())
            {
                Console.WriteLine("Este usuário possui empréstimos pendentes e não pode realizar um novo empréstimo até que os anteriores sejam devolvidos.");
                return;
            }
            */
            // Agora, há um método para verificar isso
            if (VerificarEmprestimoPendente(usuario)) return;

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
            // Agora, a nome do livro ou é nulo ou é diferente do nome salvo.
            // código anterior: if (livroEmprestado == null) throw new LibraryExceptions("Livro não encontrado!");
            if (livroEmprestado == null || livroEmprestado.NomeLivro != nomeLivro) throw new LibraryExceptions("Livro não encontrado! Verifique se o nome está digitado corretamente.");

            int quantidade = LerQuantidadeEmprestimo();
            if (livroEmprestado.QuantidadeCopias < quantidade) throw new LibraryExceptions("Quantidade inválida! Verifique se a quantidade digitada está correta.");

            livroEmprestado.QuantidadeCopias -= quantidade; // atualizando a quantidade de livros conforme o empréstimo

            // Agora, não é necessário criar uma variável para salvar o emprestimo na lista, ele vai direto.
            RegistroEmprestimo.Add(new RegistroEmprestimo(usuario.Id, usuario.Nome, livroEmprestado.NomeLivro, quantidade, HoraDoEmprestimo, HoraDeDevolucao));
            Console.WriteLine("Livro emprestado com sucesso!");

            /* Códigos anteriores:
            var emprestimo = new RegistroEmprestimo(usuario.Id, usuario.Nome, livroEmprestado.NomeLivro, quantidade, HoraDoEmprestimo, HoraDeDevolucao);
            RegistroEmprestimo.Add(emprestimo);
            */

            Console.WriteLine();

            ExibirDetalhesEmprestimo(usuario, livroEmprestado, quantidade);

            VoltarMenu();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro: {e.Message}");
        }
    }

    private static void ExibirDetalhesEmprestimo(Usuario usuario, Livro livroEmprestado, int quantidade)
    {
        Console.WriteLine();
        Console.WriteLine("DETALHES DO EMPRÉSTIMO: ");
        Console.WriteLine($"Livro emprestado {livroEmprestado.NomeLivro}, de {livroEmprestado.AutorLivro}. \nEmprestado ao ID: {usuario.Id} | Nome: {usuario.Nome} \nHora do empréstimo: {HoraDoEmprestimo} | Devolução: {HoraDeDevolucao}");
    }

    private static void VoltarMenu()
    {
        Console.WriteLine("Voltar ao menu principal? (s/n)");
        if (!char.TryParse(Console.ReadLine(), out char op) || (op != 's' && op != 'n'))
        {
            Console.WriteLine("Por favor, insira um caractere válido!");
            return;
        }

        switch (op)
        {
            case 's' or 'S': MainMenu.Menu.MainMenu(); break;
            case 'n' or 'N': Environment.Exit(0); break;
            default: Console.WriteLine("Opção inválida!"); break;
        }
    }

    // 100 % funcional
    private static void VerificarEmprestimo()
    {
        try
        {
            Console.Clear();

            int id = LerIdUsuario();
            var usuario = Usuarios.Find(u => u.Id == id);
            // Agora, o id ou é nulo ou é diferente do id cadastrado
            if (usuario == null || usuario.Id != id) throw new LibraryExceptions("ID não encontrado! Verifique se o ID digitado está correto.");
            // código anterior: if (usuario == null) throw new LibraryExceptions("ID não encontrado!");

            Console.WriteLine($"ID: {usuario.Id} | Nome: {usuario.Nome}");
            Console.WriteLine("LIVROS EMPRESTADOS: ");

            /* Códigos anteriores:
            var userEmprestimo = RegistroEmprestimo.Where(e => e.UsuarioId == id).ToList();
            if (!userEmprestimo.Any()) throw new LibraryExceptions("Nenhum empréstimo encontrado com este usuário.");
            */

            // Verificando se o usuário tem empréstimos no seu nome
            VerificarEmprestimoPendente(usuario);

            // Agora, ao invés de pesquisar dentro de uma variável, ele vai pesquisar diretamente no RegistroEmprestimo
            foreach (var ue in RegistroEmprestimo.Where(e => e.UsuarioId == usuario.Id))
            {
                Console.WriteLine($"Livro: {ue.NomeLivro} | Quantidade {ue.QuantidadeEmprestimo} " +
                                  $"| Data do Empréstimo: {ue.DataEmprestimo} | Data do Devolucao: {ue.DataDevolucao}");
            }
            Console.WriteLine();

            VoltarMenu();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro: {e.Message}");
        }
    }

    private static bool VerificarEmprestimoPendente(Usuario usuario)
    {
        var emprestimosPendentes = RegistroEmprestimo.Where(e => e.UsuarioId == usuario.Id && e.QuantidadeEmprestimo > 0).ToList();
        // retorno true se houver empréstimos pendentes e false caso não haja
        if (emprestimosPendentes.Any())
        {
            Console.WriteLine("Este usuário possui empréstimos pendentes e não pode realizar um novo empréstimo até que os anteriores sejam devolvidos.");
            return true;
        }
        else
        {
            Console.WriteLine("Nenhum empréstimo com este usuário!");
            return false;
        }
    }

    private static void RegistrarDevolucao()
    {
        try
        {
            Console.Clear();

            int id = LerIdUsuario();

            // Verificando todos os empréstimos do usuário
            var emprestimosDoUsuario = RegistroEmprestimo.Where(e => e.UsuarioId == id).ToList();
            if (!emprestimosDoUsuario.Any())
            {
                Console.WriteLine("Nenhum empréstimo encontrado para este usuário.");
                return;
            }
            Console.WriteLine();

            // Pegando o nome do usuário
            var nomeUsuario = emprestimosDoUsuario.FirstOrDefault()?.UsuarioNome; // a ? evita exceções caso a lista esteja vazia
            // Tratamento de nomes ausentes
            if (string.IsNullOrWhiteSpace(nomeUsuario))
            {
                Console.WriteLine($"O nome de usuário do ID {id} não está registrado. Verifique os dados.");
                return;
            }

            Console.WriteLine($"Total de empréstimos: {emprestimosDoUsuario.Count}");
            foreach (var eu in emprestimosDoUsuario)
            {
                Console.WriteLine($"Livro: {eu.NomeLivro} | Quantidade emprestada: {eu.QuantidadeEmprestimo}");
            }
            Console.WriteLine();

            Console.Write("Digite o nome do livro para registrar a devolução: ");
            var nomeLivro = Console.ReadLine();

            var buscarEmprestimo = emprestimosDoUsuario.FirstOrDefault(e => e.NomeLivro == nomeLivro);
            if (buscarEmprestimo == null || nomeLivro != buscarEmprestimo.NomeLivro) throw new LibraryExceptions($"Este livro {nomeLivro} não está com este usuário. ID: {id}");
            // código anterior: if (buscarEmprestimo == null) throw new LibraryExceptions($"Este livro {nomeLivro} não está com este usuário. ID: {id}");

            // verificando e validando as cópias a serem devolvidas
            Console.Write("Quantas cópias irão ser devolvidas? ");
            var quantidade = LerQuantidadeDevolucao();

            // Agora, só há um if para as duas verificações
            if (quantidade <= 0 || quantidade > buscarEmprestimo.QuantidadeEmprestimo) throw new LibraryExceptions("Quantidade para devolução é menor que 0 ou maior do que foi emprestado");
            // if (quantidade <= 0) throw new LibraryExceptions("A quantidade deve ser maior que zero.");

            // Verificando se a data de devolução não é maior que o prazo estipulado
            Console.Write("Data da Devolução: ");
            DateTime dataDevolucao = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            VerificarAtraso(HoraDeDevolucao, dataDevolucao);

            // Atualizar o registro de empréstimo
            buscarEmprestimo.QuantidadeEmprestimo -= quantidade;

            // Remover registro caso não haja mais livros a serem devolvidos
            if (buscarEmprestimo.QuantidadeEmprestimo == 0) RegistroEmprestimo.Remove(buscarEmprestimo);

            Console.WriteLine();

            // Atualizando a lista de livros
            var livro = Livro.Livros.FirstOrDefault(l => l.NomeLivro == nomeLivro);
            if (livro != null)
            {
                livro.QuantidadeCopias += quantidade;
                Console.WriteLine($"Livro: {livro.NomeLivro} agora possui {livro.QuantidadeCopias} livros disponíveis para empréstimo.");

                Console.WriteLine();

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

            VoltarMenu();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro: {e.Message}");
        }
    }

    private static int LerIdUsuario()
    {
        Console.Write("Digite o ID do usuário: ");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
        {
            Console.WriteLine("ID inválido. Tente novamente: ");
        }
        return id;
    }

    private static int LerQuantidadeEmprestimo()
    {
        Console.Write("Digite a quantidade que deseja emprestar: ");
        int quantidade;
        while (!int.TryParse(Console.ReadLine(), out quantidade) || quantidade <= 0)
        {
            Console.WriteLine("Quantidade inválida! Tente novamente: ");
        }
        return quantidade;
    }

    private static int LerQuantidadeDevolucao()
    {
        Console.Write("Digite a quantidade que deseja devolver: ");
        int quantidade;
        while (!int.TryParse(Console.ReadLine(), out quantidade) || quantidade <= 0)
        {
            Console.WriteLine("Quantidade inválida! Tente novamente: ");
        }
        return quantidade;
    }

    private static int LerOpcaoMenu()
    {
        int op;
        while (!int.TryParse(Console.ReadLine(), out op) || op <= 0 || op > 3)
        {
            Console.WriteLine("Por favor, insira uma opção válida (1, 2 ou 3): ");
        }
        return op;
    }
}