using Sistema_de_Biblioteca.Entities;

public interface IMenuLivro
{
    void Cadastrar();
    void Listar();
    void Deletar();
}

public class MenuLivro : IMenuLivro
{
    public void Cadastrar() => Livro.NovoLivro();

    public void Listar() => Livro.LivrosCadastrados();

    public void Deletar() => Livro.DeletarLivros();
}