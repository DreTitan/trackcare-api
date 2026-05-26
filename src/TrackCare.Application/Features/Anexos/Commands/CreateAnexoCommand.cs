using MediatR;
using TrackCare.Application.DTOs;
using TrackCare.Domain.Interfaces;

namespace TrackCare.Application.Features.Anexos.Commands;

public record CreateAnexoCommand(CreateAnexoDto Dto) : IRequest<AnexoDto>;

public class CreateAnexoCommandHandler : IRequestHandler<CreateAnexoCommand, AnexoDto>
{
    private readonly IAnexoRepository _repository;
    private readonly AutoMapper.IMapper _mapper;

    public CreateAnexoCommandHandler(IAnexoRepository repository, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AnexoDto> Handle(CreateAnexoCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Anexo>(request.Dto);
        entity.DataUpload = DateTime.UtcNow;
        var created = await _repository.AddAsync(entity);
        return _mapper.Map<AnexoDto>(created);
    }
}
