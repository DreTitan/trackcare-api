using Supabase;
using System.IO;

namespace HealthGoWPF.Services;

public class SupabaseService
{
    private static Supabase.Client? _client;
    private const string SUPABASE_URL = "https://ewcldebtsrjfdkckvqin.supabase.co";
    private const string SUPABASE_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImV3Y2xkZWJ0c3JqZmRrY2t2cWluIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Nzg4MTcxNzksImV4cCI6MjA5NDM5MzE3OX0.PhQPmhTrhNMGRLEtrOBAc-9heIf451rKkL0KzPu7r4s";

    public static Supabase.Client Client
    {
        get
        {
            if (_client == null)
            {
                var options = new SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = false
                };
                _client = new Supabase.Client(SUPABASE_URL, SUPABASE_KEY, options);
            }
            return _client;
        }
    }

    public static async Task<string> UploadFileAsync(string filePath, string fileName)
    {
        var bucket = Client.Storage.From("anexos");

        // Gerar nome único
        var nomeUnico = $"{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}_{fileName}";

        // Ler arquivo como bytes
        var bytes = await File.ReadAllBytesAsync(filePath);
        var response = await bucket.Upload(bytes, nomeUnico);

        // Retornar URL pública
        return bucket.GetPublicUrl(nomeUnico);
    }

    public static string GetPublicUrl(string fileName)
    {
        var bucket = Client.Storage.From("anexos");
        return bucket.GetPublicUrl(fileName);
    }

    public static async Task DeleteFileAsync(string fileName)
    {
        var bucket = Client.Storage.From("anexos");
        await bucket.Remove(fileName);
    }
}
