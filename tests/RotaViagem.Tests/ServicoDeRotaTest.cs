using Microsoft.EntityFrameworkCore;
using RotaViagem.Application;
using RotaViagem.Domain;
using RotaViagem.Infra;

namespace RotaViagem.Tests;

public class ServicoDeRotaTest
{
    private RotaDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<RotaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new RotaDbContext(options);
    }


    [Fact]
    public async Task Deve_Adicionar_Uma_Rota()
    {
        var contexto = CriarContextoEmMemoria();
        var servico = new ServicoDeRota(contexto);

        var novaRota = new Rota { Origem = "GRU", Destino = "BRC", Custo = 10 };
        await servico.AdicionarAsync(novaRota);

        var rotas = await servico.ListarTodasAsync();

        Assert.Single(rotas);
        Assert.Equal("GRU", rotas[0].Origem);
    }

    [Fact]
    public async Task Deve_Retornar_Melhor_Rota_Entre_GRU_e_CDG()
    {
        var contexto = CriarContextoEmMemoria();
        var servico = new ServicoDeRota(contexto);

        await servico.AdicionarAsync(new Rota { Origem = "GRU", Destino = "BRC", Custo = 10 });
        await servico.AdicionarAsync(new Rota { Origem = "BRC", Destino = "SCL", Custo = 5 });
        await servico.AdicionarAsync(new Rota { Origem = "SCL", Destino = "ORL", Custo = 20 });
        await servico.AdicionarAsync(new Rota { Origem = "ORL", Destino = "CDG", Custo = 5 });
        await servico.AdicionarAsync(new Rota { Origem = "GRU", Destino = "CDG", Custo = 75 });

        var (caminho, custo) = await servico.ObterMelhorRotaAsync("GRU", "CDG");

        Assert.Equal(40, custo);
        Assert.Equal(new List<string> { "GRU", "BRC", "SCL", "ORL", "CDG" }, caminho);
    }

    [Fact]
    public async Task Nao_Deve_Retornar_Rota_Quando_Nao_Existe()
    {
        var contexto = CriarContextoEmMemoria();
        var servico = new ServicoDeRota(contexto);

        var (caminho, custo) = await servico.ObterMelhorRotaAsync("AAA", "ZZZ");

        Assert.Empty(caminho);
        Assert.Equal(0, custo);
    }
}
