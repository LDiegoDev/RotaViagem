using RotaViagem.Domain;

namespace RotaViagem.Application;

public interface IServicoDeRota
{
    Task<List<Rota>> ListarTodasAsync();
    Task<Rota?> ObterAsync(string origem, string destino);
    Task AdicionarAsync(Rota rota);
    Task AtualizarAsync(Rota rota);
    Task RemoverAsync(string origem, string destino);
    Task<(List<string> Caminho, decimal CustoTotal)> ObterMelhorRotaAsync(string origem, string destino);
}
