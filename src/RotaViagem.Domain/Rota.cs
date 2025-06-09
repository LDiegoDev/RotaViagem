namespace RotaViagem.Domain;

public class Rota
{
    public string Origem { get; set; } = string.Empty;
    public string Destino { get; set; } = string.Empty;
    public decimal Custo { get; set; }
}