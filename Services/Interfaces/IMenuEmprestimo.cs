using Sistema_de_Biblioteca.Entities;

public interface IMenuEmprestimo
{
    void Gerenciar();
}

public class MenuEmprestimo : IMenuEmprestimo
{
    public void Gerenciar() => Emprestimo.Menu();
}