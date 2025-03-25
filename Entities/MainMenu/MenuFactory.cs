using Sistema_de_Biblioteca.Entities.MainMenu;

// dip
internal static class MenuFactory
{
    public static IMenuHandler CriarMenuHandler()
    {
        IMenuUsuario menuUsuario = new MenuUsuario();
        IMenuLivro menuLivro = new MenuLivro();
        IMenuEmprestimo menuEmprestimo = new MenuEmprestimo();

        return new MenuHandler(menuUsuario, menuLivro, menuEmprestimo);
    }
}