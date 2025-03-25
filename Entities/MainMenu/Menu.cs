namespace Sistema_de_Biblioteca.Entities.MainMenu
{
    // Interface do Menu
    internal static class MenuView
    {
        public static void ExibirMenu()
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
        }
    }

    // Entradas do usuário para acessar os outros menus
    internal static class MenuInput
    {
        public static int ValidarOpcao()
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int op) && op >= 1 && op <= 8) return op;
                Console.WriteLine("Entrada inválida! Por favor, insira um número entre 1 e 8");
            }
        }
    }

    // Navegação entre os outros menus
    internal class MenuHandler : IMenuHandler
    {
        private readonly IMenuUsuario _menuUsuario;
        private readonly IMenuLivro _menuLivro;
        private readonly IMenuEmprestimo _menuEmprestimo;
        private readonly Dictionary<int, Action> _opcoes;

        // cada número do menu mapeia para um método
        public MenuHandler(IMenuUsuario menuUsuario, IMenuLivro menuLivro, IMenuEmprestimo menuEmprestimo)
        {
            _menuUsuario = menuUsuario;
            _menuLivro = menuLivro;
            _menuEmprestimo = menuEmprestimo;

            // osp com isp
            _opcoes = new()
            {
                {1, menuUsuario.Cadastrar},
                {2, menuUsuario.Listar},
                {3, menuUsuario.Deletar},
                {4, menuLivro.Cadastrar},
                {5, menuLivro.Listar},
                {6, menuLivro.Deletar},
                {7, menuEmprestimo.Gerenciar}
            };
        }

        public bool ExecutarOpcao(int op)
        {
            if (op == 8) return false;

            // se a chave op existir, acao recebe a referência para o método correspondente
            if (_opcoes.TryGetValue(op, out Action? acao)) acao.Invoke();
            else Console.WriteLine("Opção inválida!");
            return true;
        }
    }

    // Menu Principal do Programa
    internal static class Menu
    {
        public static void MainMenu()
        {
            // Criação de instâncias para os menus
            IMenuHandler menuHandler = MenuFactory.CriarMenuHandler();

            while (true)
            {
                MenuView.ExibirMenu();
                int op = MenuInput.ValidarOpcao();
                if (!menuHandler.ExecutarOpcao(op)) break; // sai do loop ao selecionar "8"
            }
        }
    }
}