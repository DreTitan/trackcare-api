namespace TrackCare.Application.DTOs;

public record CreateAnexoDto(
    int RecolhimentoId,
    string NomeOriginal,
    string CaminhoCompleto,
    long TamanhoBytes,
    string Tipo,
    string? UsuarioUpload
);
