using MediatR;
using TrackCare.Application.DTOs;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Comentarios.Commands;

public record CreateComentarioCommand(CreateComentarioDto Dto) : IRequest<ComentarioDto>;

public class CreateComentarioCommandHandler : IRequestHandler<CreateComentarioCommand, ComentarioDto>
{
    private readonly IComentarioRepository _repository;
    private readonly AutoMapper.IMapper _mapper;

    public CreateComentarioCommandHandler(IComentarioRepository repository, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ComentarioDto> Handle(CreateComentarioCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Comentario>(request.Dto);
        entity.DataCriacao = DateTime.UtcNow;
        var created = await _repository.AddAsync(entity);
        return _mapper.Map<ComentarioDto>(created);
    }
}
