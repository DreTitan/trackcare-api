using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrackCare.Application.DTOs;
using TrackCare.Application.Features.Anexos.Commands;
using TrackCare.Application.Features.Anexos.Queries;
using TrackCare.Domain.Interfaces;

namespace TrackCare.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnexosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IFileStorageService _fileStorage;

    public AnexosController(IMediator mediator, IFileStorageService fileStorage)
    {
        _mediator = mediator;
        _fileStorage = fileStorage;
    }

    [HttpGet("{recolhimentoId:int}")]
    public async Task<IActionResult> GetByRecolhimento(int recolhimentoId)
        => Ok(await _mediator.Send(new GetAnexosByRecolhimentoQuery(recolhimentoId)));

    [HttpPost("{recolhimentoId:int}")]
    public async Task<IActionResult> Upload(int recolhimentoId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Arquivo não fornecido");

        var extensao = Path.GetExtension(file.FileName).ToLower();
        if (extensao != ".pdf")
            return BadRequest("Apenas arquivos PDF são permitidos");

        using var stream = file.OpenReadStream();
        var url = await _fileStorage.UploadAsync(stream, file.FileName, file.ContentType);

        var nomeUnico = Path.GetFileName(new Uri(url).LocalPath);
        var tipo = file.FileName.ToLower().Contains("relatório") || file.FileName.ToLower().Contains("relatorio")
            ? "RelatorioN3"
            : file.FileName.ToLower().Contains("nota fiscal") || file.FileName.ToLower().Contains("nf")
                ? "NotaFiscal"
                : file.FileName.ToLower().Contains("foto")
                    ? "Foto"
                    : "PDF";

        var dto = new CreateAnexoDto(recolhimentoId, file.FileName, url, file.Length, tipo, "system");
        var result = await _mediator.Send(new CreateAnexoCommand(dto));

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteAnexoCommand(id));
        return result ? NoContent() : NotFound();
    }
}
