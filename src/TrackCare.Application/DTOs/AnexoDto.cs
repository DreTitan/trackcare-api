namespace TrackCare.Application.DTOs;

public record AnexoDto(
    int Id,
    int RecolhimentoId,
    string Tipo,
    string NomeOriginal,
    string NomeArquivo,
    long TamanhoBytes,
    string? UsuarioUpload,
    DateTime DataUpload,
    string CaminhoCompleto
);
