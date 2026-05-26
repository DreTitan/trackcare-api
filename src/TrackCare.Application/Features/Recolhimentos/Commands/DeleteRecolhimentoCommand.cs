using MediatR;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Recolhimentos.Commands;

public record DeleteRecolhimentoCommand(int Id) : IRequest<bool>;

public class DeleteRecolhimentoCommandHandler : IRequestHandler<DeleteRecolhimentoCommand, bool>
{
    private readonly IRecolhimentoRepository _repository;
    private readonly IFileStorageService _fileStorage;

    public DeleteRecolhimentoCommandHandler(
        IRecolhimentoRepository repository,
        IFileStorageService fileStorage)
    {
        _repository = repository;
        _fileStorage = fileStorage;
    }

    public async Task<bool> Handle(DeleteRecolhimentoCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdWithDetailsAsync(request.Id);
        if (entity == null) return false;

        foreach (var anexo in entity.Anexos)
        {
            await _fileStorage.DeleteAsync(anexo.NomeArquivo);
        }

        await _repository.DeleteAsync(request.Id);
        return true;
    }
}
