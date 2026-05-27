using AutoMapper;
using TrackCare.Application.DTOs;
using TrackCare.Domain.Entities;

namespace TrackCare.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Recolhimento, RecolhimentoDto>()
            .ForMember(d => d.ClientePlano, opt => opt.MapFrom(s => s.ClientePlano.ToString()))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.StatusTexto, opt => opt.MapFrom(s => s.StatusTexto))
            .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s => s.CreatedBy))
            .ForMember(d => d.Created, opt => opt.MapFrom(s => s.Created))
            .ForMember(d => d.LastModified, opt => opt.MapFrom(s => s.LastModified));

        CreateMap<Anexo, AnexoDto>()
            .ForMember(d => d.Tipo, opt => opt.MapFrom(s => s.Tipo.ToString()));

        CreateMap<CreateAnexoDto, Anexo>()
            .ForMember(d => d.Tipo, opt => opt.MapFrom(s => Enum.Parse<Domain.Enums.TipoAnexo>(s.Tipo)));

        CreateMap<Comentario, ComentarioDto>();
        CreateMap<CreateComentarioDto, Comentario>();

        CreateMap<HistoricoStatus, HistoricoStatusDto>()
            .ForMember(d => d.StatusAnterior, opt => opt.MapFrom(s => s.StatusAnterior.ToString()))
            .ForMember(d => d.StatusNovo, opt => opt.MapFrom(s => s.StatusNovo.ToString()));
    }
}
