using Sistema_de_Biblioteca.Entities;

public interface IMenuUsuario 
{
    void Cadastrar();
    void Listar();
    void Deletar();
}

public class MenuUsuario : IMenuUsuario
{
    public void Cadastrar() => Usuario.NovoUsuario();

    public void Listar() => Usuario.UsuariosCadastrados();

    public void Deletar() => Usuario.DeletarUsuarios();
}