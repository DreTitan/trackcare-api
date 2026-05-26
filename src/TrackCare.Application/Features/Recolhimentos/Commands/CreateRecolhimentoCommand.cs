using MediatR;
using TrackCare.Application.DTOs;
using TrackCare.Domain.Entities;
using TrackCare.Domain.Enums;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Recolhimentos.Commands;

public record CreateRecolhimentoCommand(CreateRecolhimentoDto Dto) : IRequest<RecolhimentoDto>;

public class CreateRecolhimentoCommandHandler : IRequestHandler<CreateRecolhimentoCommand, RecolhimentoDto>
{
    private readonly IRecolhimentoRepository _repository;
    private readonly IHistoricoStatusRepository _historicoRepository;
    private readonly AutoMapper.IMapper _mapper;

    public CreateRecolhimentoCommandHandler(
        IRecolhimentoRepository repository,
        IHistoricoStatusRepository historicoRepository,
        AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _historicoRepository = historicoRepository;
        _mapper = mapper;
    }

    public async Task<RecolhimentoDto> Handle(CreateRecolhimentoCommand request, CancellationToken cancellationToken)
    {
        var entity = new Recolhimento
        {
            Hgid = request.Dto.Hgid,
            NumeroSerie = request.Dto.NumeroSerie,
            Modelo = request.Dto.Modelo,
            ClienteNome = request.Dto.ClienteNome,
            ClienteContato = request.Dto.ClienteContato,
            ClienteEmail = request.Dto.ClienteEmail,
            ClienteTelefone = request.Dto.ClienteTelefone,
            ClientePlano = Enum.Parse<TipoPlano>(request.Dto.ClientePlano),
            TicketHub = request.Dto.TicketHub,
            TicketBlip = request.Dto.TicketBlip,
            DescricaoProblema = request.Dto.DescricaoProblema,
            RelatorioN3 = request.Dto.RelatorioN3,
            JaRecolhido = request.Dto.JaRecolhido,
            DataPrevistaColeta = request.Dto.DataPrevistaColeta,
            Observacoes = request.Dto.Observacoes,
            Status = StatusRecolhimento.N3_Enviou,
            DataSolicitacao = DateTime.UtcNow,
            CriadoPor = request.Dto.Usuario,
            CriadoEm = DateTime.UtcNow,
            AtualizadoEm = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(entity);

        await _historicoRepository.AddAsync(new HistoricoStatus
        {
            RecolhimentoId = created.Id,
            StatusAnterior = (StatusRecolhimento)(-1),
            StatusNovo = StatusRecolhimento.N3_Enviou,
            Observacao = "Recolhimento criado",
            Usuario = request.Dto.Usuario,
            DataAlteracao = DateTime.UtcNow
        });

        return _mapper.Map<RecolhimentoDto>(created);
    }
}
