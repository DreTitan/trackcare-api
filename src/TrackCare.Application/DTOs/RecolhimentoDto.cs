namespace TrackCare.Application.DTOs;

public record RecolhimentoDto(
    int Id,
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
    string Status,
    string StatusTexto,
    DateTime DataSolicitacao,
    DateTime? DataPrevistaColeta,
    DateTime? DataColetaReal,
    DateTime? DataPrevistaDevolucao,
    DateTime? DataDevolucaoReal,
    string? Observacoes,
    string? CriadoPor,
    DateTime CriadoEm,
    DateTime AtualizadoEm
);
