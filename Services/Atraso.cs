using System.Globalization;

namespace Sistema_de_Biblioteca.Services;

public class Atraso
{
    // para o caso do usuário devolver o livro com atraso
    // será cobrado uma taxa de R$5 se o dia da entrega for < 10; e uma taxa de R$15 se for maior que > 10 dias.
    protected internal static void VerificarAtraso(DateTime dataDevolucao, DateTime dataAtraso)
    {
        decimal multaPorAtraso;

        if (dataAtraso <= dataDevolucao) return;
        int quantosDias = dataAtraso.Subtract(dataDevolucao).Days;

        if (quantosDias < 10)
        {
            multaPorAtraso = 5.00m;
            Console.WriteLine($"Há uma multa a ser paga no valor de R${multaPorAtraso.ToString("f2", CultureInfo.InvariantCulture)}. " +
                $"O atraso é de {quantosDias} dias.");
        }
        else
        {
            multaPorAtraso = 15.00m;
            Console.WriteLine($"Há uma multa a ser paga no valor de R${multaPorAtraso.ToString("f2", CultureInfo.InvariantCulture)}. " +
                $"O atraso é de {quantosDias} dias.");
        }
    }
}