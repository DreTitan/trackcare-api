using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrackCare.Application.DTOs;
using TrackCare.Application.Features.Recolhimentos.Commands;
using TrackCare.Application.Features.Recolhimentos.Queries;

namespace TrackCare.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecolhimentosController : ControllerBase
{
    private readonly IMediator _mediator;

    public RecolhimentosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllRecolhimentosQuery()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetRecolhimentoByIdQuery(id));
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? termo, [FromQuery] string? status)
        => Ok(await _mediator.Send(new SearchRecolhimentosQuery(termo, status)));

    [HttpGet("dashboard/stats")]
    public async Task<IActionResult> GetDashboardStats()
        => Ok(await _mediator.Send(new GetDashboardStatsQuery()));

    [HttpGet("recent/{count:int}")]
    public async Task<IActionResult> GetRecent(int count)
        => Ok(await _mediator.Send(new GetRecentRecolhimentosQuery(count)));

    [HttpGet("{id:int}/historico")]
    public async Task<IActionResult> GetHistorico(int id)
        => Ok(await _mediator.Send(new GetHistoricoByRecolhimentoQuery(id)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRecolhimentoDto dto)
    {
        var result = await _mediator.Send(new CreateRecolhimentoCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRecolhimentoDto dto)
    {
        var updatedDto = new UpdateRecolhimentoDto(
            id, dto.Hgid, dto.NumeroSerie, dto.Modelo, dto.ClienteNome,
            dto.ClienteContato, dto.ClienteEmail, dto.ClienteTelefone,
            dto.ClientePlano, dto.TicketHub, dto.TicketBlip, dto.DescricaoProblema,
            dto.RelatorioN3, dto.JaRecolhido, dto.DataPrevistaColeta, dto.Observacoes);
        return Ok(await _mediator.Send(new UpdateRecolhimentoCommand(updatedDto)));
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
    {
        var result = await _mediator.Send(new UpdateStatusCommand(id, dto.NovoStatus, dto.Observacao, dto.Usuario));
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteRecolhimentoCommand(id));
        return result ? NoContent() : NotFound();
    }
}

public record UpdateStatusDto(string NovoStatus, string? Observacao, string Usuario);
