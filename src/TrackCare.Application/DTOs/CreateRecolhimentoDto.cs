namespace TrackCare.Application.DTOs;

public record CreateRecolhimentoDto(
    string Hgid,
    string NumeroSerie,
    string? Modelo,
    string ClienteNome,
    string? ClienteContato,
    string? ClienteEmail,
    string? ClienteTelefone,
    string ClientePlano,
    string? TicketHub,
    string? TicketBlip,
    string? DescricaoProblema,
    string? RelatorioN3,
    bool JaRecolhido,
    DateTime? DataPrevistaColeta,
    string? Observacoes,
    string Usuario
);
