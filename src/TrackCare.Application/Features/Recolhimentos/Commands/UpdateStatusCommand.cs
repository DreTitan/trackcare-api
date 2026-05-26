using MediatR;
using TrackCare.Application.DTOs;
using TrackCare.Domain.Entities;
using TrackCare.Domain.Enums;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Recolhimentos.Commands;

public record UpdateStatusCommand(int Id, string NovoStatus, string? Observacao, string Usuario) : IRequest<bool>;

public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand, bool>
{
    private readonly IRecolhimentoRepository _repository;
    private readonly IHistoricoStatusRepository _historicoRepository;

    public UpdateStatusCommandHandler(
        IRecolhimentoRepository repository,
        IHistoricoStatusRepository historicoRepository)
    {
        _repository = repository;
        _historicoRepository = historicoRepository;
    }

    public async Task<bool> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null) return false;

        var statusAnterior = entity.Status;
        entity.Status = Enum.Parse<StatusRecolhimento>(request.NovoStatus);
        entity.AtualizadoEm = DateTime.UtcNow;

        await _repository.UpdateAsync(entity);

        await _historicoRepository.AddAsync(new HistoricoStatus
        {
            RecolhimentoId = entity.Id,
            StatusAnterior = statusAnterior,
            StatusNovo = entity.Status,
            Observacao = request.Observacao,
            Usuario = request.Usuario,
            DataAlteracao = DateTime.UtcNow
        });

        return true;
    }
}
