using MediatR;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Comentarios.Commands;

public record DeleteComentarioCommand(int Id) : IRequest<bool>;

public class DeleteComentarioCommandHandler : IRequestHandler<DeleteComentarioCommand, bool>
{
    private readonly IComentarioRepository _repository;

    public DeleteComentarioCommandHandler(IComentarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteComentarioCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null) return false;
        await _repository.DeleteAsync(request.Id);
        return true;
    }
}
