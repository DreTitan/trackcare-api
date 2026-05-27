using MediatR;
using TrackCare.Application.DTOs;
using TrackCare.Domain.Enums;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Recolhimentos.Commands;

public record UpdateRecolhimentoCommand(UpdateRecolhimentoDto Dto) : IRequest<RecolhimentoDto>;

public class UpdateRecolhimentoCommandHandler : IRequestHandler<UpdateRecolhimentoCommand, RecolhimentoDto>
{
    private readonly IRecolhimentoRepository _repository;
    private readonly AutoMapper.IMapper _mapper;

    public UpdateRecolhimentoCommandHandler(IRecolhimentoRepository repository, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RecolhimentoDto> Handle(UpdateRecolhimentoCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Dto.Id)
            ?? throw new KeyNotFoundException($"Recolhimento {request.Dto.Id} não encontrado");

        entity.Hgid = request.Dto.Hgid;
        entity.NumeroSerie = request.Dto.NumeroSerie;
        entity.Modelo = request.Dto.Modelo;
        entity.ClienteNome = request.Dto.ClienteNome;
        entity.ClienteContato = request.Dto.ClienteContato;
        entity.ClienteEmail = request.Dto.ClienteEmail;
        entity.ClienteTelefone = request.Dto.ClienteTelefone;
        entity.ClientePlano = Enum.Parse<TipoPlano>(request.Dto.ClientePlano);
        entity.TicketHub = request.Dto.TicketHub;
        entity.TicketBlip = request.Dto.TicketBlip;
        entity.DescricaoProblema = request.Dto.DescricaoProblema;
        entity.RelatorioN3 = request.Dto.RelatorioN3;
        entity.JaRecolhido = request.Dto.JaRecolhido;
        entity.DataPrevistaColeta = request.Dto.DataPrevistaColeta;
        entity.Observacoes = request.Dto.Observacoes;
        entity.LastModified = DateTimeOffset.UtcNow;

        await _repository.UpdateAsync(entity);
        return _mapper.Map<RecolhimentoDto>(entity);
    }
}
