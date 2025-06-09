using Microsoft.EntityFrameworkCore;
using RotaViagem.Domain;
using RotaViagem.Infra;

namespace RotaViagem.Application;

public class ServicoDeRota : IServicoDeRota
{
    private readonly RotaDbContext _context;

    public ServicoDeRota(RotaDbContext contexto)
    {
        _context = contexto;
    }

    public async Task<List<Rota>> ListarTodasAsync()
    {
        return await _context.Rotas.ToListAsync();
    }

    public async Task<Rota?> ObterAsync(string origem, string destino)
    {
        return await _context.Rotas.FindAsync(origem, destino);
    }

    public async Task AdicionarAsync(Rota rota)
    {
        _context.Rotas.Add(rota);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Rota rota)
    {
        var existente = await ObterAsync(rota.Origem, rota.Destino);
        if (existente != null)
        {
            existente.Custo = rota.Custo;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoverAsync(string origem, string destino)
    {
        var rota = await ObterAsync(origem, destino);
        if (rota != null)
        {
            _context.Rotas.Remove(rota);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(List<string> Caminho, decimal CustoTotal)> ObterMelhorRotaAsync(string origem, string destino)
    {
        var rotas = await _context.Rotas.ToListAsync();

        var visitados = new HashSet<string>();
        var caminhoAtual = new List<string>();
        var todasAsRotas = new List<(List<string>, decimal)>();

        caminhoAtual.Add(origem);
        visitados.Add(origem);

        BuscarRotaMaisBarata(rotas, origem, destino, visitados, caminhoAtual, 0, todasAsRotas);

        if (todasAsRotas.Count == 0)
            return (new List<string>(), 0);

        var melhor = todasAsRotas.OrderBy(x => x.Item2).First();
        return melhor;
    }

    private void BuscarRotaMaisBarata(
        List<Rota> rotas,
        string atual,
        string destino,
        HashSet<string> visitados,
        List<string> caminho,
        decimal custo,
        List<(List<string>, decimal)> resultados)
    {
        if (atual == destino)
        {
            resultados.Add((new List<string>(caminho), custo));
            return;
        }

        var proximas = rotas.Where(r => r.Origem == atual);

        foreach (var rota in proximas)
        {
            if (!visitados.Contains(rota.Destino))
            {
                visitados.Add(rota.Destino);
                caminho.Add(rota.Destino);

                BuscarRotaMaisBarata(rotas, rota.Destino, destino, visitados, caminho, custo + rota.Custo, resultados);

                caminho.RemoveAt(caminho.Count - 1);
                visitados.Remove(rota.Destino);
            }
        }
    }
}