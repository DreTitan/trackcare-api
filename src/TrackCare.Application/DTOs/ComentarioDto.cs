namespace TrackCare.Application.DTOs;

public record ComentarioDto(
    int Id,
    int RecolhimentoId,
    string Texto,
    string? Usuario,
    string? Setor,
    DateTime DataCriacao
);
