namespace TrackCare.Application.DTOs;

public record CreateComentarioDto(
    int RecolhimentoId,
    string Texto,
    string? Usuario,
    string? Setor
);
