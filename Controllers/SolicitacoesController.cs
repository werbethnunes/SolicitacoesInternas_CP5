using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolicitacoesInternas.Data;
using SolicitacoesInternas.Models;

namespace SolicitacoesInternas.Controllers;

[ApiController]
[Route("api/v1/solicitacoes")]
public class SolicitacoesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SolicitacoesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Solicitacao>>> GetTodas()
    {
        var solicitacoes = await _context.Solicitacoes
            .OrderByDescending(x => x.DataSolicitacao)
            .ToListAsync();

        return Ok(solicitacoes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Solicitacao>> GetPorId(int id)
    {
        var solicitacao = await _context.Solicitacoes.FindAsync(id);

        if (solicitacao is null)
            return NotFound();

        return Ok(solicitacao);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<Solicitacao>>> GetPorStatus(StatusSolicitacao status)
    {
        var solicitacoes = await _context.Solicitacoes
            .Where(x => x.Status == status)
            .OrderByDescending(x => x.DataSolicitacao)
            .ToListAsync();

        return Ok(solicitacoes);
    }

    [HttpPost]
    public async Task<ActionResult<Solicitacao>> Criar(Solicitacao solicitacao)
    {
        solicitacao.Id = 0;
        solicitacao.Status = StatusSolicitacao.Aberta;
        solicitacao.DataSolicitacao = DateTime.Now;

        if (string.IsNullOrWhiteSpace(solicitacao.Solicitante) ||
            string.IsNullOrWhiteSpace(solicitacao.Descricao))
        {
            return BadRequest("Solicitante e descrição são obrigatórios.");
        }

        _context.Solicitacoes.Add(solicitacao);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPorId), new { id = solicitacao.Id }, solicitacao);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, Solicitacao dados)
    {
        var solicitacao = await _context.Solicitacoes.FindAsync(id);

        if (solicitacao is null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(dados.Solicitante) ||
            string.IsNullOrWhiteSpace(dados.Descricao))
        {
            return BadRequest("Solicitante e descrição são obrigatórios.");
        }

        solicitacao.Solicitante = dados.Solicitante;
        solicitacao.Tipo = dados.Tipo;
        solicitacao.Descricao = dados.Descricao;
        solicitacao.Status = dados.Status;

        await _context.SaveChangesAsync();

        return Ok(solicitacao);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var solicitacao = await _context.Solicitacoes.FindAsync(id);

        if (solicitacao is null)
            return NotFound();

        _context.Solicitacoes.Remove(solicitacao);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
