using HealthGoCollection.Models;

namespace HealthGoCollection.Services;

public interface IAnexoService
{
    Task<Anexo> UploadAsync(int recolhimentoId, IFormFile file, TipoAnexo tipo, string usuario);
    Task<bool> DeleteAsync(int id);
    Task<string?> GetFilePathAsync(int id);
}

public class AnexoService : IAnexoService
{
    private readonly IRecolhimentoService _recolhimentoService;
    private readonly string _uploadPath;

    public AnexoService(IRecolhimentoService recolhimentoService, IWebHostEnvironment environment)
    {
        _recolhimentoService = recolhimentoService;
        _uploadPath = Path.Combine(environment.ContentRootPath, "wwwroot", "uploads");
        Directory.CreateDirectory(_uploadPath);
    }

    public async Task<Anexo> UploadAsync(int recolhimentoId, IFormFile file, TipoAnexo tipo, string usuario)
    {
        var recolhimento = await _recolhimentoService.GetByIdAsync(recolhimentoId);
        if (recolhimento == null)
            throw new Exception("Recolhimento não encontrado");

        // Gerar nome único
        var extensao = Path.GetExtension(file.FileName);
        var nomeUnico = $"{recolhimentoId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{extensao}";
        var caminhoCompleto = Path.Combine(_uploadPath, nomeUnico);

        // Copiar arquivo
        using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var anexo = new Anexo
        {
            RecolhimentoId = recolhimentoId,
            Tipo = tipo,
            CaminhoArquivo = nomeUnico,
            NomeOriginal = file.FileName,
            ConteudoTipo = file.ContentType,
            TamanhoBytes = file.Length,
            UsuarioUpload = usuario,
            DataUpload = DateTime.Now
        };

        return anexo;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var path = await GetFilePathAsync(id);
        if (path != null && File.Exists(path))
        {
            File.Delete(path);
        }
        return true;
    }

    public async Task<string?> GetFilePathAsync(int id)
    {
        // Esta implementação precisa do DbContext, será ajustada
        await Task.CompletedTask;
        return null;
    }
}
