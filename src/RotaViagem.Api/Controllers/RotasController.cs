using Microsoft.AspNetCore.Mvc;
using RotaViagem.Application;
using RotaViagem.Domain;

namespace RotaViagem.Api.Controllers;

[ApiController]
[Route("api/rotas")]
public class RotasController : ControllerBase
{
    private readonly IServicoDeRota _servico;

    public RotasController(IServicoDeRota servico)
    {
        _servico = servico;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var rotas = await _servico.ListarTodasAsync();
        return Ok(rotas);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] Rota novaRota)
    {
        await _servico.AdicionarAsync(novaRota);

        return Created("", novaRota);
    }

    [HttpPut]
    public async Task<IActionResult> Atualizar([FromBody] Rota rota)
    {
        await _servico.AtualizarAsync(rota);
        return NoContent();
    }

    [HttpDelete("{origem}-{destino}")]
    public async Task<IActionResult> Remover(string origem, string destino)
    {
        await _servico.RemoverAsync(origem, destino);
        return NoContent();
    }

    [HttpGet("melhor-rota")]
    public async Task<IActionResult> ConsultarMelhorRota([FromQuery] string origem, [FromQuery] string destino)
    {
        var (caminho, custo) = await _servico.ObterMelhorRotaAsync(origem, destino);

        if (!caminho.Any())
            return NotFound("Não foi possível encontrar uma rota.");

        var rotaFormatada = string.Join(" - ", caminho);
        return Ok($"{rotaFormatada} ao custo de ${custo}");
    }
}
