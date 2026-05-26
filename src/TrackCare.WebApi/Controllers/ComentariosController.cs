using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrackCare.Application.DTOs;
using TrackCare.Application.Features.Comentarios.Commands;
using TrackCare.Application.Features.Comentarios.Queries;

namespace TrackCare.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComentariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ComentariosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{recolhimentoId:int}")]
    public async Task<IActionResult> GetByRecolhimento(int recolhimentoId)
        => Ok(await _mediator.Send(new GetComentariosByRecolhimentoQuery(recolhimentoId)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateComentarioDto dto)
    {
        var result = await _mediator.Send(new CreateComentarioCommand(dto));
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteComentarioCommand(id));
        return result ? NoContent() : NotFound();
    }
}
