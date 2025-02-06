namespace Sistema_de_Biblioteca.Entities;

public class RegistroEmprestimo
{
    public int UsuarioId { get; set; }
    public string? UsuarioNome { get; set; }
    public string? NomeLivro { get; set; }
    public int QuantidadeEmprestimo { get; set; }
    public DateTime DataEmprestimo { get; set; }
    public DateTime DataDevolucao { get; set; }

    public RegistroEmprestimo(int usuarioId, string? usuarioNome, string? nomeLivro, int quantidadeEmprestimo, DateTime dataEmprestimo, DateTime dataDevolucao)
    {
        UsuarioId = usuarioId;
        UsuarioNome = usuarioNome;
        NomeLivro = nomeLivro;
        QuantidadeEmprestimo = quantidadeEmprestimo;
        DataEmprestimo = dataEmprestimo;
        DataDevolucao = dataDevolucao;
    }

    public RegistroEmprestimo()
    {
    }
}