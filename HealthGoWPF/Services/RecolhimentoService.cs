using Microsoft.EntityFrameworkCore;
using HealthGoWPF.Data;
using HealthGoWPF.Models;
using System.IO;

namespace HealthGoWPF.Services;

public class RecolhimentoService
{
    private readonly HealthGoDbContext _context;

    public RecolhimentoService()
    {
        _context = new HealthGoDbContext();
        _context.Database.EnsureCreated();
    }

    public List<Recolhimento> GetAll()
    {
        return _context.Recolhimentos
            .OrderByDescending(r => r.DataSolicitacao)
            .ToList();
    }

    public Recolhimento? GetById(int id)
    {
        return _context.Recolhimentos.Find(id);
    }

    public List<Recolhimento> Search(string? termo, StatusRecolhimento? status)
    {
        var query = _context.Recolhimentos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(termo))
        {
            termo = termo.ToLower();
            query = query.Where(r =>
                r.Hgid.ToLower().Contains(termo) ||
                r.NumeroSerie.ToLower().Contains(termo) ||
                r.ClienteNome.ToLower().Contains(termo) ||
                (r.TicketHub != null && r.TicketHub.ToLower().Contains(termo)) ||
                (r.TicketBlip != null && r.TicketBlip.ToLower().Contains(termo)));
        }

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        return query.OrderByDescending(r => r.DataSolicitacao).ToList();
    }

    public Recolhimento Create(Recolhimento recolhimento, string usuario)
    {
        recolhimento.CriadoPor = usuario;
        recolhimento.CriadoEm = DateTime.Now;
        recolhimento.AtualizadoEm = DateTime.Now;

        _context.Recolhimentos.Add(recolhimento);
        _context.SaveChanges();

        // Registrar no histórico
        var historico = new HistoricoStatus
        {
            RecolhimentoId = recolhimento.Id,
            StatusAnterior = (StatusRecolhimento)(-1),
            StatusNovo = recolhimento.Status,
            Observacao = "Recolhimento criado",
            Usuario = usuario
        };
        _context.HistoricoStatus.Add(historico);
        _context.SaveChanges();

        return recolhimento;
    }

    public Recolhimento Update(Recolhimento recolhimento)
    {
        var existente = _context.Recolhimentos.Find(recolhimento.Id);
        if (existente == null)
            throw new Exception("Recolhimento não encontrado");

        existente.Hgid = recolhimento.Hgid;
        existente.NumeroSerie = recolhimento.NumeroSerie;
        existente.Modelo = recolhimento.Modelo;
        existente.ClienteNome = recolhimento.ClienteNome;
        existente.ClienteContato = recolhimento.ClienteContato;
        existente.ClienteEmail = recolhimento.ClienteEmail;
        existente.ClienteTelefone = recolhimento.ClienteTelefone;
        existente.ClientePlano = recolhimento.ClientePlano;
        existente.TicketHub = recolhimento.TicketHub;
        existente.TicketBlip = recolhimento.TicketBlip;
        existente.DescricaoProblema = recolhimento.DescricaoProblema;
        existente.RelatorioN3 = recolhimento.RelatorioN3;
        existente.JaRecolhido = recolhimento.JaRecolhido;
        existente.Status = recolhimento.Status;
        existente.DataPrevistaColeta = recolhimento.DataPrevistaColeta;
        existente.DataColetaReal = recolhimento.DataColetaReal;
        existente.DataPrevistaDevolucao = recolhimento.DataPrevistaDevolucao;
        existente.Observacoes = recolhimento.Observacoes;
        existente.AtualizadoEm = DateTime.Now;

        _context.SaveChanges();
        return existente;
    }

    public bool UpdateStatus(int id, StatusRecolhimento novoStatus, string? observacao, string usuario)
    {
        var recolhimento = _context.Recolhimentos.Find(id);
        if (recolhimento == null)
            return false;

        var statusAnterior = recolhimento.Status;
        recolhimento.Status = novoStatus;
        recolhimento.AtualizadoEm = DateTime.Now;

        var historico = new HistoricoStatus
        {
            RecolhimentoId = id,
            StatusAnterior = statusAnterior,
            StatusNovo = novoStatus,
            Observacao = observacao,
            Usuario = usuario
        };
        _context.HistoricoStatus.Add(historico);

        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var recolhimento = _context.Recolhimentos.Find(id);
        if (recolhimento == null)
            return false;

        // Excluir anexos do Storage
        var anexos = _context.Anexos.Where(a => a.RecolhimentoId == id).ToList();
        foreach (var anexo in anexos)
        {
            _ = SupabaseService.DeleteFileAsync(anexo.NomeArquivo);
        }

        _context.Recolhimentos.Remove(recolhimento);
        _context.SaveChanges();
        return true;
    }

    public List<HistoricoStatus> GetHistorico(int recolhimentoId)
    {
        return _context.HistoricoStatus
            .Where(h => h.RecolhimentoId == recolhimentoId)
            .OrderByDescending(h => h.DataAlteracao)
            .ToList();
    }

    public (int totalAtivos, int aguardaColeta, int emReparo, int encerradosMes) GetStats()
    {
        var totalAtivos = _context.Recolhimentos
            .Where(r => r.Status != StatusRecolhimento.Encerrado)
            .Count();

        var aguardaColeta = _context.Recolhimentos
            .Where(r => r.Status == StatusRecolhimento.Aguarda_Coleta ||
                       r.Status == StatusRecolhimento.Aguarda_Admin ||
                       r.Status == StatusRecolhimento.N3_Enviou)
            .Count();

        var emReparo = _context.Recolhimentos
            .Where(r => r.Status == StatusRecolhimento.Em_Reparo ||
                       r.Status == StatusRecolhimento.Recebido_Fabrica ||
                       r.Status == StatusRecolhimento.Em_Transito)
            .Count();

        var primeiroDiaMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var encerradosMes = _context.Recolhimentos
            .Where(r => r.Status == StatusRecolhimento.Encerrado &&
                       r.AtualizadoEm >= primeiroDiaMes)
            .Count();

        return (totalAtivos, aguardaColeta, emReparo, encerradosMes);
    }

    // ========== MÉTODOS DE ANEXOS ==========

    public List<Anexo> GetAnexos(int recolhimentoId)
    {
        return _context.Anexos
            .Where(a => a.RecolhimentoId == recolhimentoId)
            .OrderByDescending(a => a.DataUpload)
            .ToList();
    }

    public Anexo? AdicionarAnexo(int recolhimentoId, string caminhoOrigem, string nomeOriginal, string usuario)
    {
        var recolhimento = _context.Recolhimentos.Find(recolhimentoId);
        if (recolhimento == null)
            return null;

        // Verificar se é PDF
        var extensao = Path.GetExtension(caminhoOrigem).ToLower();
        if (extensao != ".pdf")
            throw new Exception("Apenas arquivos PDF são permitidos.");

        // Determinar tipo
        var tipo = TipoAnexo.PDF;
        if (nomeOriginal.ToLower().Contains("relatório") || nomeOriginal.ToLower().Contains("relatorio"))
            tipo = TipoAnexo.RelatorioN3;
        else if (nomeOriginal.ToLower().Contains("nota fiscal") || nomeOriginal.ToLower().Contains("nf"))
            tipo = TipoAnexo.NotaFiscal;
        else if (nomeOriginal.ToLower().Contains("foto"))
            tipo = TipoAnexo.Foto;

        // Upload para Supabase Storage (síncrono via .GetAwaiter().GetResult())
        var urlPublica = SupabaseService.UploadFileAsync(caminhoOrigem, nomeOriginal).GetAwaiter().GetResult();

        // Extrair nome do arquivo da URL
        var uri = new Uri(urlPublica);
        var nomeArquivo = uri.Segments.Last();

        var anexo = new Anexo
        {
            RecolhimentoId = recolhimentoId,
            NomeOriginal = nomeOriginal,
            NomeArquivo = nomeArquivo,
            TamanhoBytes = new FileInfo(caminhoOrigem).Length,
            CaminhoCompleto = urlPublica,
            Tipo = tipo,
            UsuarioUpload = usuario,
            DataUpload = DateTime.Now
        };

        _context.Anexos.Add(anexo);
        _context.SaveChanges();

        return anexo;
    }

    public bool ExcluirAnexo(int anexoId)
    {
        var anexo = _context.Anexos.Find(anexoId);
        if (anexo == null)
            return false;

        // Excluir do Storage
        _ = SupabaseService.DeleteFileAsync(anexo.NomeArquivo);

        _context.Anexos.Remove(anexo);
        _context.SaveChanges();
        return true;
    }

    public string? ObterCaminhoAnexo(int anexoId)
    {
        var anexo = _context.Anexos.Find(anexoId);
        if (anexo == null)
            return null;

        // Retornar URL pública do Supabase Storage
        return SupabaseService.GetPublicUrl(anexo.NomeArquivo);
    }

    // ========== MÉTODOS DE COMENTÁRIOS ==========

    public List<Comentario> GetComentarios(int recolhimentoId)
    {
        return _context.Comentarios
            .Where(c => c.RecolhimentoId == recolhimentoId)
            .OrderByDescending(c => c.DataCriacao)
            .ToList();
    }

    public Comentario? AdicionarComentario(int recolhimentoId, string texto, string usuario, string? setor = null)
    {
        var recolhimento = _context.Recolhimentos.Find(recolhimentoId);
        if (recolhimento == null)
            return null;

        if (string.IsNullOrWhiteSpace(texto))
            throw new Exception("O texto do comentário não pode estar vazio.");

        var comentario = new Comentario
        {
            RecolhimentoId = recolhimentoId,
            Texto = texto.Trim(),
            Usuario = usuario,
            Setor = setor,
            DataCriacao = DateTime.Now
        };

        _context.Comentarios.Add(comentario);
        _context.SaveChanges();

        return comentario;
    }

    public bool ExcluirComentario(int comentarioId)
    {
        var comentario = _context.Comentarios.Find(comentarioId);
        if (comentario == null)
            return false;

        _context.Comentarios.Remove(comentario);
        _context.SaveChanges();
        return true;
    }
}
