using MediatR;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Anexos.Commands;

public record DeleteAnexoCommand(int Id) : IRequest<bool>;

public class DeleteAnexoCommandHandler : IRequestHandler<DeleteAnexoCommand, bool>
{
    private readonly IAnexoRepository _repository;
    private readonly IFileStorageService _fileStorage;

    public DeleteAnexoCommandHandler(IAnexoRepository repository, IFileStorageService fileStorage)
    {
        _repository = repository;
        _fileStorage = fileStorage;
    }

    public async Task<bool> Handle(DeleteAnexoCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null) return false;

        await _fileStorage.DeleteAsync(entity.NomeArquivo);
        await _repository.DeleteAsync(request.Id);
        return true;
    }
}
