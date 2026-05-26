namespace TrackCare.Application.DTOs;

public record HistoricoStatusDto(
    int Id,
    int RecolhimentoId,
    string StatusAnterior,
    string StatusNovo,
    string? Observacao,
    string? Usuario,
    DateTime DataAlteracao
);
